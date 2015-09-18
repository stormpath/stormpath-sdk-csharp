﻿// <copyright file="LinqAssertExtensions.cs" company="Stormpath, Inc.">
//      Copyright (c) 2015 Stormpath, Inc.
// </copyright>
// <remarks>
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </remarks>

using System.Threading;
using NSubstitute;
using Shouldly;
using Stormpath.SDK.Impl.DataStore;
using Stormpath.SDK.Impl.Resource;
using Stormpath.SDK.Linq;
using Stormpath.SDK.Resource;
using Stormpath.SDK.Tests.Fakes;

namespace Stormpath.SDK.Tests.Helpers
{
    public static class LinqAssertExtensions
    {
        public static void GeneratedArgumentsWere<T>(this IAsyncQueryable<T> queryable, string href, string arguments)
        {
            var resourceQueryable = queryable as CollectionResourceQueryable<T>;
            if (resourceQueryable == null)
                Assertly.Fail("This queryable is not a CollectionResourceQueryable.");

            resourceQueryable.CurrentHref.ShouldBe($"{href}?{arguments}");
        }

        // The same thing as above, but for testing whether a FakeDataStore received a particular URL call
        internal static void WasCalledWithArguments<T>(this IInternalDataStore ds, string href, string arguments)
            where T : class, IResource
        {
            var asFake = ds as FakeDataStore<T>;
            if (asFake != null)
            {
                if (string.IsNullOrEmpty(arguments))
                    asFake.GetCalls().ShouldContain($"{href}");
                else
                    asFake.GetCalls().ShouldContain($"{href}?{arguments}");
                return;
            }
            else
            {
                // Maybe it's an NSubstitute mock
                if (string.IsNullOrEmpty(arguments))
                    ds.Received().GetCollectionAsync<T>($"{href}", CancellationToken.None);
                else
                    ds.Received().GetCollectionAsync<T>($"{href}?{arguments}", CancellationToken.None);
                return;
            }
        }
    }
}
