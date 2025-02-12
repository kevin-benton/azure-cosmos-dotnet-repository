﻿// Copyright (c) IEvangelist. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Linq;
using Microsoft.Azure.CosmosRepository.Attributes;
using Microsoft.Azure.CosmosRepository.Builders;
using Microsoft.Azure.CosmosRepository.Options;
using Microsoft.Extensions.Options;

namespace Microsoft.Azure.CosmosRepository.Providers
{
    /// <inheritdoc cref="Microsoft.Azure.CosmosRepository.Providers.ICosmosPartitionKeyPathProvider" />
    class DefaultCosmosPartitionKeyPathProvider :
        ICosmosPartitionKeyPathProvider
    {
        private readonly IOptions<RepositoryOptions> _options;

        public DefaultCosmosPartitionKeyPathProvider(IOptions<RepositoryOptions> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc />
        public string GetPartitionKeyPath<TItem>() where TItem : IItem
        {
            Type itemType = typeof(TItem);
            Type attributeType = typeof(PartitionKeyPathAttribute);

            ContainerOptionsBuilder optionsBuilder = _options.Value.GetContainerOptions<TItem>();

            if (optionsBuilder is { } && string.IsNullOrWhiteSpace(optionsBuilder.PartitionKey) is false)
            {
                return optionsBuilder.PartitionKey;
            }

            return Attribute.GetCustomAttribute(
                itemType, attributeType) is PartitionKeyPathAttribute partitionKeyPathAttribute
                ? partitionKeyPathAttribute.Path
                : "/id";
        }
    }
}
