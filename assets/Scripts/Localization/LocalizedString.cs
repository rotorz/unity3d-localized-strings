// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using System;
using UnityEngine;

namespace Rotorz.Games.Localization
{
    /// <summary>
    /// A localized string which has a distinctive pattern when serialized that can be
    /// used to automatically detect strings in serialized assets.
    /// </summary>
    [Serializable]
    public sealed class LocalizedString
    {
        [SerializeField]
        private XGetText xgettext;


        /// <summary>
        /// Gets or sets the context of the string.
        /// </summary>
        /// <remarks>
        /// <para>An empty string indicates that no context is specified for the string.</para>
        /// </remarks>
        public string Context {
            get { return this.xgettext.context; }
            set { this.xgettext.context = value ?? ""; }
        }

        /// <summary>
        /// Gets or sets the value of the string in the base language.
        /// </summary>
        public string Value {
            get { return this.xgettext.value; }
            set { this.xgettext.value = value; }
        }



        [Serializable]
        private struct XGetText
        {
            public string context;
            public string value;
        }
    }
}
