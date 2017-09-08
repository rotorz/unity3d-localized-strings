// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using Rotorz.Games.EditorExtensions;
using System.Globalization;

namespace Rotorz.Games.Localization
{
    /// <summary>
    /// Generic base class for localizing a custom package.
    /// </summary>
    /// <example>
    /// <para>Example of a custom package's language class:</para>
    /// <code language="csharp"><![CDATA[
    /// // Editor/MyPackageLang.cs
    /// using Rotorz.Games.Localization;
    /// using System.Globalization;
    ///
    /// [DiscoverablePackageLanguage]
    /// public sealed class MyPackageLang : PackageLanguage<MyPackageLang>
    /// {
    ///     public MyPackageLang()
    ///         : base("@my-vendor-name/my-package", CultureInfo.GetCultureInfo("en-US"))
    ///     {
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    /// <typeparam name="TImplementation">Type of the implementation.</typeparam>
    public abstract class PackageLanguage<TImplementation> : PackageLanguage
        where TImplementation : PackageLanguage<TImplementation>, new()
    {
        /// <summary>
        /// Name of the directory that language files are loaded from.
        /// </summary>
        public const string LanguagesDirectoryName = "Languages";


        private static TImplementation s_Instance = null;
        private static ILocalizedStrings s_LocalizedStrings = null;


        /// <summary>
        /// Gets the one-and-only <typeparamref name="TImplementation"/> instance.
        /// </summary>
        public static TImplementation Instance {
            get {
                if (s_Instance == null) {
                    s_Instance = new TImplementation();
                }
                return s_Instance;
            }
        }

        /// <summary>
        /// Gets localized strings for active culture of the package's language domain.
        /// </summary>
        protected static ILocalizedStrings LocalizedStrings {
            get {
                if (s_LocalizedStrings == null) {
                    s_LocalizedStrings = Instance.languageDomain;
                }
                return s_LocalizedStrings;
            }
        }


        /// <summary>
        /// Gets the name of the associated package.
        /// </summary>
        public static string PackageName {
            get { return Instance.GetPackageName(); }
        }

        /// <summary>
        /// Gets the root culture of the package. This is the culture that represents the
        /// non-translated text that is passed into the various text functions of this
        /// class. Typically this is "en-US".
        /// </summary>
        public static CultureInfo RootCulture {
            get { return Instance.GetRootCulture(); }
        }

        /// <summary>
        /// Gets the culture of the localization that is currently active.
        /// </summary>
        public static CultureInfo ActiveCulture {
            get { return Instance.GetActiveCulture(); }
        }


        /// <summary>
        /// Gets localized message text.
        /// </summary>
        /// <example>
        /// <code language="csharp"><![CDATA[
        /// string text = MyPackageLang.Text("Hello, world!");
        /// ]]></code>
        /// </example>
        /// <param name="message">Non-translated message text in root language.</param>
        /// <returns>
        /// The localized text or non-translated text if not localized.
        /// </returns>
        public static string Text(string message)
        {
            return LocalizedStrings.Text(message);
        }

        /// <summary>
        /// Gets localized message text with an explicit context to help disambiguate
        /// meaning for translators.
        /// </summary>
        /// <example>
        /// <code language="csharp"><![CDATA[
        /// string actionText = MyPackageLang.ParticularText("Action", "New");
        /// ]]></code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="message">Non-translated message text in root language.</param>
        /// <returns>
        /// The localized text or non-translated text if not localized.
        /// </returns>
        public static string ParticularText(string context, string message)
        {
            return LocalizedStrings.ParticularText(context, message);
        }

        /// <summary>
        /// Gets localized message text taking plural form into consideration using the
        /// given numeric value.
        /// </summary>
        /// <example>
        /// <code language="csharp"><![CDATA[
        /// int appleCount = 3;
        /// string text = MyPackageLang.PluralText(
        ///     "You ate an apple.", "You ate {0} apples.", appleCount
        /// );
        /// ]]></code>
        /// </example>
        /// <param name="singularMessage">Non-translated message text in root language
        /// for singular form of <paramref name="value"/>.</param>
        /// <param name="pluralMessage">Non-translated message text in root language
        /// for plural form of <paramref name="value"/>.</param>
        /// <param name="value">The numeric value.</param>
        /// <returns>
        /// The localized text or non-translated text if not localized.
        /// </returns>
        public static string PluralText(string singularMessage, string pluralMessage, int value)
        {
            return LocalizedStrings.PluralText(singularMessage, pluralMessage, value);
        }

        /// <summary>
        /// Gets localized message text taking plural form into consideration using the
        /// given numeric value with an explicit context to help disambiguate meaning for
        /// translators.
        /// </summary>
        /// <example>
        /// <code language="csharp"><![CDATA[
        /// int appleCount = 3;
        /// string text = MyPackageLang.ParticularPluralText(
        ///     "Fruit", "You ate an apple.", "You ate {0} apples.", appleCount
        /// );
        /// ]]></code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="singularMessage">Non-translated message text in root language
        /// for singular form of <paramref name="value"/>.</param>
        /// <param name="pluralMessage">Non-translated message text in root language
        /// for plural form of <paramref name="value"/>.</param>
        /// <param name="value">The numeric value.</param>
        /// <returns>
        /// The localized text or non-translated text if not localized.
        /// </returns>
        public static string ParticularPluralText(string context, string singularMessage, string pluralMessage, int value)
        {
            return LocalizedStrings.ParticularPluralText(context, singularMessage, pluralMessage, value);
        }


        /// <summary>
        /// Gets localized proper name. See the gettext manual, section Names.
        /// </summary>
        /// <remarks>
        /// <para>This method automatically annotates translated names with the original
        /// non-translated name when the translation doesn't already include the original
        /// non-translated name in parenthesis; for instance, "TRANSLATION (ORIGINAL)".</para>
        /// </remarks>
        /// <param name="name">Non-translated proper name.</param>
        /// <returns>
        /// The localized proper name or non-translated proper name if not localized.
        /// </returns>
        public static string ProperName(string name)
        {
            return LocalizedStrings.ProperName(name);
        }


        /// <summary>
        /// Formats action message text with ellipsis hint to highlight that the action
        /// will present the user with a different window.
        /// </summary>
        /// <remarks>
        /// <para>This method does not attempt to localize <paramref name="actionMessage"/>;
        /// use the other text methods of this interface to perform localization where
        /// required in addition to using this method.</para>
        /// </remarks>
        /// <example>
        /// <code language="csharp"><![CDATA[
        /// string actionText = MyPackageLang.ParticularText("Action", "New");
        /// string actionTextWithEllipsis = MyPackageLang.OpensWindow(actionText);
        /// ]]></code>
        /// </example>
        /// <param name="actionMessage">Action message text.</param>
        /// <returns>
        /// The formatted action message text.
        /// </returns>
        public static string OpensWindow(string actionMessage)
        {
            return LocalizedStrings.OpensWindow(actionMessage);
        }


        private readonly ILocalizedStringsRepository localizedStringsRepository;
        private readonly ILanguageDomain languageDomain;


        /// <summary>
        /// Initializes a new instance of the <see cref="PackageLanguage{TImplementation}"/> class.
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
            : base(packageName, rootCulture)
        {
            this.localizedStringsRepository = this.CreateLocalizedStringsRepository();
            this.languageDomain = CreateLanguageDomain(this.localizedStringsRepository);

            this.Load();
        }


        /// <inheritdoc/>
        public override CultureInfo GetActiveCulture()
        {
            return PackageLanguageManager.PreferredSpecificCulture;
        }


        /// <inheritdoc/>
        public override CultureInfo[] DiscoverAvailableCultures()
        {
            return this.localizedStringsRepository.DiscoverAvailableLocalizations();
        }


        /// <summary>
        /// Loads the language domain. This method should not be invoked directly by user
        /// code; instead invoke <see cref="Reload()"/>.
        /// </summary>
        protected virtual void Load()
        {
            this.languageDomain.Load(this.GetActiveCulture());
        }

        /// <inheritdoc/>
        public override void Reload()
        {
            this.Load();
        }


        /// <summary>
        /// Creates the <see cref="ILocalizedStringsRepository"/> that will be used to
        /// discover and access localized string sources.
        /// </summary>
        /// <returns>
        /// The <see cref="ILocalizedStringsRepository"/> instance.
        /// </returns>
        protected virtual ILocalizedStringsRepository CreateLocalizedStringsRepository()
        {
            var loader = this.CreateLocalizedStringsLoader();
            return new LocalizedStringsPathsRepository(this.GetLanguagesDirectoryPaths(), ".mo", loader);
        }

        /// <summary>
        /// Creates the <see cref="ILocalizedStringsLoader"/> that will be used for
        /// loading localized strings for the package's language domain.
        /// </summary>
        /// <returns>
        /// The <see cref="ILocalizedStringsLoader"/> instance.
        /// </returns>
        protected virtual ILocalizedStringsLoader CreateLocalizedStringsLoader()
        {
            return new LocalizedStringsLoaderMo();
        }

        /// <summary>
        /// Creates the <see cref="ILanguageDomain"/> for the package.
        /// </summary>
        /// <param name="localizedStringsRepository">The localized string repository.</param>
        /// <returns>
        /// The <see cref="ILanguageDomain"/> instance.
        /// </returns>
        protected virtual ILanguageDomain CreateLanguageDomain(ILocalizedStringsRepository localizedStringsRepository)
        {
            return new LanguageDomain(localizedStringsRepository);
        }

        /// <summary>
        /// Gets the absolute paths of the language file directories that should be used
        /// when searching for language files. When multiple language files are provided
        /// for the same culture they are all loaded and then flattened allowing latter
        /// paths to override specific strings if desired.
        /// </summary>
        /// <returns>
        /// An array of languages directory file system paths.
        /// </returns>
        protected virtual string[] GetLanguagesDirectoryPaths()
        {
            return new string[] {
                PackageUtility.ResolveAssetPathAbsolute(this.GetPackageName(), LanguagesDirectoryName),
                PackageUtility.ResolveDataPathAbsolute(this.GetPackageName(), LanguagesDirectoryName)
            };
        }
    }
}
