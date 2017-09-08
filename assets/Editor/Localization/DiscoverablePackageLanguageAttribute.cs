// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System;

namespace Rotorz.Games.Localization
{
    /// <summary>
    /// Allows <see cref="PackageLanguage"/> implementations to be automatically
    /// discovered by the <see cref="PackageLanguageManager"/>.
    /// </summary>
    /// <seealso cref="PackageLanguage{TImplementation}"/>
    /// <seealso cref="PackageLanguage"/>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class DiscoverablePackageLanguageAttribute : Attribute
    {
    }
}
