using System;

namespace Martijn.Extensions.JSON.Compat
{
    internal enum NullHandling
    {
        /// <summary>Do not write the property when it is null, and do not assign JSON nulls when reading.</summary>
        Ignore,

        /// <summary>Always write the property, even when the serializer skips nulls.</summary>
        Include,
    }

    /// <summary>
    /// Replacement for <c>[JsonProperty(NullValueHandling = ...)]</c>. System.Text.Json has no
    /// equivalent that also works on value types and also affects reading, so the compat layer
    /// interprets this attribute itself.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    internal sealed class NullValueHandlingAttribute : Attribute
    {
        public NullValueHandlingAttribute(NullHandling handling)
        {
            Handling = handling;
        }

        public NullHandling Handling { get; }
    }
}
