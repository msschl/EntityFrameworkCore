// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;

namespace Microsoft.Data.Entity.ValueGeneration
{
    public class TemporaryIntegerValueGenerator<TValue> : SimpleTemporaryValueGenerator<TValue>
    {
        private long _current;

        public override TValue Next()
        {
            var generatedValue = Interlocked.Decrement(ref _current);

            if (typeof(TValue) == typeof(uint))
            {
                return (TValue)(object)unchecked((uint)generatedValue);
            }

            if (typeof(TValue) == typeof(ulong))
            {
                return (TValue)(object)unchecked((ulong)generatedValue);
            }

            if (typeof(TValue) == typeof(ushort))
            {
                return (TValue)(object)unchecked((ushort)generatedValue);
            }

            if (typeof(TValue) == typeof(byte))
            {
                return (TValue)(object)unchecked((byte)generatedValue);
            }

            return (TValue)Convert.ChangeType(generatedValue, typeof(TValue));
        }
    }
}
