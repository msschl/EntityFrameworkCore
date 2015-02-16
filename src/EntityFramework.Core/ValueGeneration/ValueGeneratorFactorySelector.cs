// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Internal;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Utilities;

namespace Microsoft.Data.Entity.ValueGeneration
{
    public abstract class ValueGeneratorFactorySelector
    {
        private readonly SimpleValueGeneratorFactory<GuidValueGenerator> _guidFactory;
        private readonly TemporaryIntegerValueGeneratorFactory _integerFactory;
        private readonly SimpleValueGeneratorFactory<TemporaryStringValueGenerator> _stringFactory;
        private readonly SimpleValueGeneratorFactory<TemporaryBinaryValueGenerator> _binaryFactory;

        protected ValueGeneratorFactorySelector(
            [NotNull] SimpleValueGeneratorFactory<GuidValueGenerator> guidFactory,
            [NotNull] TemporaryIntegerValueGeneratorFactory integerFactory,
            [NotNull] SimpleValueGeneratorFactory<TemporaryStringValueGenerator> stringFactory,
            [NotNull] SimpleValueGeneratorFactory<TemporaryBinaryValueGenerator> binaryFactory)
        {
            Check.NotNull(guidFactory, nameof(guidFactory));
            Check.NotNull(integerFactory, nameof(integerFactory));
            Check.NotNull(stringFactory, nameof(stringFactory));
            Check.NotNull(binaryFactory, nameof(binaryFactory));

            _guidFactory = guidFactory;
            _integerFactory = integerFactory;
            _stringFactory = stringFactory;
            _binaryFactory = binaryFactory;
        }

        public virtual ValueGeneratorFactory Select([NotNull] IProperty property)
        {
            Check.NotNull(property, nameof(property));

            var propertyType = property.PropertyType;

            if (propertyType == typeof(Guid))
            {
                return _guidFactory;
            }

            if (propertyType.UnwrapNullableType().IsInteger())
            {
                return _integerFactory;
            }

            if (propertyType == typeof(string))
            {
                return _stringFactory;
            }

            if (propertyType == typeof(byte[]))
            {
                return _binaryFactory;
            }

            throw new NotSupportedException(
                Strings.NoValueGenerator(property.Name, property.EntityType.SimpleName, propertyType.Name));
        }
    }
}
