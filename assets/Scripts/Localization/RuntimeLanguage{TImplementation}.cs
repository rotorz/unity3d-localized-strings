// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System;
using System.Globalization;

namespace Rotorz.Games.Localization
{
    /// <summary>
    /// Generic base class for localizing a custom runtime.
    /// </summary>
    /// <example>
    /// <para>Example of a custom runtime's language class:</para>
    /// <code language="csharp"><![CDATA[
    /// // MyProjectLang.cs
    /// using Rotorz.Games.Localization;
    /// using System.Globalization;
    ///
    /// public sealed class MyProjectLang : RuntimeLanguage<MyProjectLang>
    /// {
    ///     public MyProjectLang()
    ///         : base(CultureInfo.GetCultureInfo("en-US"))
    ///     {
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    /// <typeparam name="TImplementation">Type of the implementation.</typeparam>
    public abstract class RuntimeLanguage<TImplementation>
        where TImplementation : RuntimeLanguage<TImplementation>, new()
    {
        private static TImplementation s_Instance = null;


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
        /// Gets or sets localized strings for active culture of the package's language domain.
        /// </summary>
        public static ILocalizedStrings LocalizedStrings { get; private set; }


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
        /// string text = MyProjectLang.Text("Hello, world!");
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
        /// string actionText = MyProjectLang.ParticularText("Action", "New");
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
        /// string text = MyProjectLang.PluralText(
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
        /// string text = MyProjectLang.ParticularPluralText(
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
        /// string actionText = MyProjectLang.ParticularText("Action", "New");
        /// string actionTextWithEllipsis = MyProjectLang.OpensWindow(actionText);
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



        private readonly CultureInfo rootCulture;



        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeLanguage{TImplementation}"/> class.
        /// </summary>
        /// <param name="rootCulture">Culture of the non-translated message text in code.</param>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="rootCulture"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// If <paramref name="packageName"/> is an empty string.
        /// </exception>
        protected RuntimeLanguage(CultureInfo rootCulture)
        {
            ExceptionUtility.CheckArgumentNotNull(rootCulture, "rootCulture");

            this.rootCulture = rootCulture;
        }


        /// <summary>
        /// Sets up the language domain instance.
        /// </summary>
        /// <param name="domain">The language domain instance.</param>
        /// <exception cref="System.ArgumentNullException">
        /// If <paramref name="domain"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// If the runtime language has already been setup.
        /// </exception>
        public virtual void Setup(ILanguageDomain domain)
        {
            ExceptionUtility.CheckArgumentNotNull(domain, "domain");

            if (LocalizedStrings != null) {
                throw new InvalidOperationException("Runtime language has already been setup.");
            }

            LocalizedStrings = domain;
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
    }
}
