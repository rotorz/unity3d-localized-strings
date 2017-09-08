// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System.Globalization;

namespace Rotorz.Games.Localization
{
    /// <summary>
    /// Base class for localizing a custom package.
    /// </summary>
    /// <see cref="PackageLanguage{TImplementation}"/>
    public abstract class PackageLanguage
    {
        private readonly string packageName;
        private readonly CultureInfo rootCulture;


        /// <summary>
        /// Initializes a new instance of the <see cref="PackageLanguage"/> class.
        /// </summary>
        /// <param name="packageName">Name of the package that the language is for.</param>
        /// <param name="rootCulture">Culture of the non-translated message text in code.</param>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="packageName"/> or <paramref name="rootCulture"/> are <c>null</c>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// If <paramref name="packageName"/> is an empty string.
        /// </exception>
        protected PackageLanguage(string packageName, CultureInfo rootCulture)
        {
            ExceptionUtility.CheckExpectedStringArgument(packageName, "packageName");
            ExceptionUtility.CheckArgumentNotNull(rootCulture, "rootCulture");

            this.packageName = packageName;
            this.rootCulture = rootCulture;
        }


        /// <summary>
        /// Gets the name of the associated package.
        /// </summary>
        public string GetPackageName()
        {
            return this.packageName;
        }

        /// <summary>
        /// Gets the root culture of the package. This is the culture that represents the
        /// non-translated text that is passed into the various text functions of this
        /// class. Typically this is "en-US".
        /// </summary>
        public CultureInfo GetRootCulture()
        {
            return this.rootCulture;
        }

        /// <summary>
        /// Gets the culture of the localization that is currently active.
        /// </summary>
        public abstract CultureInfo GetActiveCulture();


        /// <summary>
        /// Discovers the cultures that translations are available in.
        /// </summary>
        /// <returns>
        /// An array of zero-or-more cultures.
        /// </returns>
        public abstract CultureInfo[] DiscoverAvailableCultures();

        /// <summary>
        /// Reloads the language domain using the currently active culture.
        /// </summary>
        public abstract void Reload();
    }
}
