﻿// Copyright (c) IEvangelist. All rights reserved.
// Licensed under the MIT License.

using System;
using Newtonsoft.Json;

namespace Microsoft.Azure.CosmosRepository
{
    /// <summary>
    /// A base helper class that implements IItemWithEtag
    /// <remarks>
    /// This base class will implement all supported interfaces giving the complete information available from this SDK.
    /// </remarks>
    /// </summary>
    /// <example>
    /// Here is an example subclass item, which adds several properties:
    /// <code language="c#">
    /// <![CDATA[
    /// public class SubItem : FullItem
    /// {
    ///     public DateTimeOffset Date { get; set; }
    ///     public string Name { get; set; }
    ///     public IEnumerable<Child> Children { get; set; }
    ///     public IEnumerable<string> Tags { get; set; }
    /// }
    ///
    /// public class Child
    /// {
    ///     public string Name { get; set; }
    ///     public DateTime BirthDate { get; set; }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public abstract class FullItem : Item, IItemWithEtag, IItemWithTimeToLive, IItemWithTimeStamps
    {
        /// <inheritdoc />
        [JsonProperty("_etag")]
        public string Etag { get; private set; }

        /// <inheritdoc />
        public TimeSpan? TimeToLive
        {
            get => _timeToLive.HasValue ? TimeSpan.FromSeconds(_timeToLive.Value) : null;
            set
            {
                if (value?.TotalSeconds != null) _timeToLive = (int)value?.TotalSeconds;
            }
        }

        [JsonProperty("ttl", NullValueHandling = NullValueHandling.Ignore)]
        private int? _timeToLive;

        /// <inheritdoc />
        [JsonIgnore]
        public DateTime LastUpdatedTimeUtc => DateTimeOffset.FromUnixTimeSeconds(LastUpdatedTimeRaw).DateTime;

        /// <inheritdoc />
        [JsonProperty("_ts")]
        public long LastUpdatedTimeRaw
        {
            get;
            private set;
        }

        /// <inheritdoc />
        [JsonProperty]
        public DateTime? CreatedTimeUtc { get; set; }
    }
}