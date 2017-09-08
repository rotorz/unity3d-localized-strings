// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using Rotorz.Games.Reflection;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Rotorz.Games.Localization
{
    /// <summary>
    /// Manages <see cref="PackageLanguage"/> for Unity editor scripts allowing for
    /// automatic culture discovery and preferred language selection.
    /// </summary>
    [InitializeOnLoad]
    public static class PackageLanguageManager
    {
        /// <summary>
        /// The fallback culture to use when there was a problem decoding the user
        /// preferred culture ("en-US").
        /// </summary>
        public static readonly CultureInfo FallbackCulture = CultureInfo.GetCultureInfo("en-US");


        #region Package Language Discovery

        private static PackageLanguage[] s_Packages;


        /// <summary>
        /// Gets an array of discoverable <see cref="PackageLanguage"/> implementations.
        /// </summary>
        public static PackageLanguage[] Packages {

            get {
                if (s_Packages == null) {
                    s_Packages = GetDiscoverablePackageLanguageInstances();
                }
                return s_Packages.ToArray();
            }
        }


        private static PackageLanguage[] GetDiscoverablePackageLanguageInstances()
        {
            return GetDiscoverablePackageLanguageTypes()
                .Select(GetSingletonInstance)
                .Distinct()
                .ToArray();
        }

        private static Type[] GetDiscoverablePackageLanguageTypes()
        {
            return TypeMeta.DiscoverImplementations<PackageLanguage>()
                .Where(type => Attribute.IsDefined(type, typeof(DiscoverablePackageLanguageAttribute)))
                .ToArray();
        }

        private static PackageLanguage GetSingletonInstance(Type packageLanguageType)
        {
            var propertyInfo = packageLanguageType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            return propertyInfo.GetValue(null, null) as PackageLanguage;
        }

        #endregion


        #region Preferred Language Selection

        private static readonly string s_EditorPrefsPreferredCultureKey = typeof(PackageLanguageManager).FullName + ".PreferredCulture";

        private static CultureInfo s_PreferredCulture;
        private static CultureInfo s_PreferredSpecificCulture;


        private static void AutoInitializePreferredCulture()
        {
            if (s_PreferredCulture != null) {
                return;
            }

            string editorPrefsPreferredCulture = EditorPrefs.GetString(s_EditorPrefsPreferredCultureKey, FallbackCulture.Name);
            try {
                s_PreferredCulture = ResolveCultureInfo(editorPrefsPreferredCulture);
                s_PreferredSpecificCulture = s_PreferredCulture;
            }
            catch (Exception ex) {
                Debug.LogException(ex);

                s_PreferredCulture = FallbackCulture;
                s_PreferredSpecificCulture = FallbackCulture;
            }
        }


        /// <summary>
        /// Gets the user preferred culture. The value of this property should be shown
        /// as the user selected preferred language in language selection user interfaces.
        /// For example, if "fr" was selected then the culture returned by this property
        /// will still represent "fr".
        /// </summary>
        /// <seealso cref="PreferredSpecificCulture"/>
        public static CultureInfo PreferredCulture {
            get {
                AutoInitializePreferredCulture();
                return s_PreferredCulture;
            }
        }

        /// <summary>
        /// Gets the user preferred specific culture. The value of this property should
        /// be used when loading localized strings. For example, if "fr" was selected
        /// then the culture returned by this property will represent "fr-FR".
        /// </summary>
        /// <seealso cref="PreferredCulture"/>
        public static CultureInfo PreferredSpecificCulture {
            get {
                AutoInitializePreferredCulture();
                return s_PreferredSpecificCulture;
            }
        }


        public static event EventHandler PreferredCultureChanged;
        public static event EventHandler PreferredCultureReloaded;

        private static void OnPreferredCultureChanged()
        {
            var handler = PreferredCultureChanged;
            if (handler != null) {
                handler.Invoke(null, EventArgs.Empty);
            }
        }

        private static void OnPreferredCultureReloaded()
        {
            var handler = PreferredCultureReloaded;
            if (handler != null) {
                handler.Invoke(null, EventArgs.Empty);
            }
        }


        /// <summary>
        /// Sets the user preferred culture.
        /// </summary>
        /// <remarks>
        /// <para>Assumes culture from <see cref="CultureInfo.CurrentCulture"/> when
        /// <paramref name="culture"/> is <c>null</c>.</para>
        /// </remarks>
        /// <param name="culture">The culture.</param>
        public static void SetPreferredCulture(CultureInfo culture)
        {
            if (culture == null) {
                culture = CultureInfo.CurrentCulture;
            }
            if (culture == PreferredCulture) {
                return;
            }

            EditorPrefs.SetString(s_EditorPrefsPreferredCultureKey, culture.Name);

            s_PreferredCulture = culture;
            s_PreferredSpecificCulture = CultureInfo.CreateSpecificCulture(culture.Name);

            OnPreferredCultureChanged();

            ReloadAll();
        }

        /// <summary>
        /// Sets the user preferred culture.
        /// </summary>
        /// <remarks>
        /// <para>Assumes culture from <see cref="CultureInfo.CurrentCulture"/> when
        /// <paramref name="cultureName"/> is <c>null</c> or empty.</para>
        /// </remarks>
        /// <param name="cultureName">Name of the culture (for example, "fr-FR").</param>
        public static void SetPreferredCulture(string cultureName)
        {
            SetPreferredCulture(ResolveCultureInfo(cultureName));
        }


        private static CultureInfo ResolveCultureInfo(string name)
        {
            if (name != null && name != "") {
                try {
                    try {
                        return CultureInfo.CreateSpecificCulture(name);
                    }
                    catch {
                        return CultureInfo.GetCultureInfo(name);
                    }
                }
                catch { }
            }
            return CultureInfo.CurrentCulture;
        }

        #endregion


        /// <summary>
        /// Discovers the cultures that translations are available in across all
        /// discoverable <see cref="PackageLanguage"/> implementations.
        /// </summary>
        /// <returns>
        /// An array of zero-or-more cultures.
        /// </returns>
        /// <seealso cref="DiscoverablePackageLanguageAttribute"/>
        /// <seealso cref="PackageLanguage{TImplementation}"/>
        public static CultureInfo[] DiscoverAvailableCultures()
        {
            return Packages
                .SelectMany(package => package.DiscoverAvailableCultures())
                .Distinct()
                .ToArray();
        }

        /// <summary>
        /// Reloads all discoverable <see cref="PackageLanguage"/> implementations.
        /// </summary>
        public static void ReloadAll()
        {
            foreach (var package in Packages) {
                package.Reload();
            }
            OnPreferredCultureReloaded();
        }
    }
}
