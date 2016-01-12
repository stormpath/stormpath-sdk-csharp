﻿// <copyright file="CollectionResourceQueryable_iteration_tests.cs" company="Stormpath, Inc.">
// Copyright (c) 2016 Stormpath, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Stormpath.SDK.Account;
using Stormpath.SDK.Linq;
using Stormpath.SDK.Tests.Common.Fakes;
using Xunit;

namespace Stormpath.SDK.Tests.Impl.Linq
{
    public class CollectionResourceQueryable_iteration_tests : Linq_test<IAccount>
    {
        [Fact]
        public async Task First_iteration_does_not_modify_pagination_arguments()
        {
            this.InitializeClientWithCollection(TestAccounts.RebelAlliance);

            var query = this.Queryable
                .Take(5)
                .Skip(10);

            await (query as IAsyncQueryable<IAccount>).MoveNextAsync();
            this.ShouldBeCalledWithArguments("limit=5", "offset=10");
        }

        [Fact]
        public async Task First_iteration_does_not_add_pagination_arguments()
        {
            this.InitializeClientWithCollection(TestAccounts.RebelAlliance);

            var query = this.Queryable;

            await query.MoveNextAsync();
            this.FakeHttpClient.Calls.Single().CanonicalUri.ToString().ShouldNotContain("?");
        }

        [Fact]
        public async Task Subsequent_iterations_add_pagination_arguments_if_none_exist()
        {
            this.InitializeClientWithCollection(Enumerable.Repeat(TestAccounts.AdmiralAckbar, 50));

            var query = this.Queryable;

            await query.MoveNextAsync();
            var firstPageCount = query.CurrentPage.Count();
            await query.MoveNextAsync();
            this.FakeHttpClient.Calls.Last().CanonicalUri.ToString().ShouldContain($"offset={firstPageCount}");
        }
    }
}
