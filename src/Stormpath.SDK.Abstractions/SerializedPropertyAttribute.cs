﻿using System;

namespace Stormpath.SDK
{
    /// <summary>
    /// Allows customization of serialized properties.
    /// </summary>
    /// <remarks>A thin replacement for DataMemberAttribute. This may be replaced in a future version.</remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SerializedPropertyAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
