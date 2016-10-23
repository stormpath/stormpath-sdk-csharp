﻿// <copyright file="Expansion_retrieval_options_tests.cs" company="Stormpath, Inc.">
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

using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using Shouldly;
using Stormpath.SDK.Account;
using Stormpath.SDK.AccountStore;
using Stormpath.SDK.Application;
using Stormpath.SDK.Group;
using Stormpath.SDK.Http;
using Stormpath.SDK.Impl.DataStore;
using Stormpath.SDK.Tenant;
using Stormpath.SDK.Tests.Common;
using Stormpath.SDK.Tests.Common.Fakes;
using Xunit;

namespace Stormpath.SDK.Tests
{
    public class Expansion_retrieval_options_tests
    {
        private static async Task GeneratedArgumentsWere(IInternalDataStore dataStore, string expectedQueryString)
        {
            await dataStore.RequestExecutor.Received(1).ExecuteAsync(
                Arg.Is<IHttpRequest>(x => x.CanonicalUri.QueryString.ToString() == expectedQueryString),
                Arg.Any<CancellationToken>());
        }

        private IInternalDataStore BuildDataStore(string resourceResponse)
        {
            return TestDataStore.Create(new StubRequestExecutor(resourceResponse).Object);
        }

        [Fact]
        public async Task Inline_syntax()
        {
            var dataStore = this.BuildDataStore(FakeJson.Account);

            var account = await dataStore.GetResourceAsync<IAccount>(
                "/foobarAccount",
                o => o.Expand(x => x.GetDirectory()).Expand(x => x.GetGroupMemberships()));

            account.Email.ShouldBe("han.solo@corellia.core");
            account.FullName.ShouldBe("Han Solo");

            await GeneratedArgumentsWere(dataStore, "expand=directory,groupMemberships");
        }

        [Fact]
        public async Task Expanded_syntax()
        {
            var dataStore = this.BuildDataStore(FakeJson.Account);

            var account = await dataStore.GetResourceAsync<IAccount>(
                "/foobarAccount",
                opt =>
                {
                    opt.Expand(x => x.GetDirectory());
                    opt.Expand(x => x.GetGroupMemberships(10, 10));
                });

            await GeneratedArgumentsWere(dataStore, "expand=directory,groupMemberships(offset:10,limit:10)");
        }

        [Fact]
        public async Task With_custom_data()
        {
            var dataStore = this.BuildDataStore(FakeJson.Account);

            var account = await dataStore.GetResourceAsync<IAccount>("/foobarAccount", o => o.Expand(x => x.GetCustomData()));

            await GeneratedArgumentsWere(dataStore, "expand=customData");
        }

        [Fact]
        public async Task With_directory()
        {
            var dataStore = this.BuildDataStore(FakeJson.Account);

            var account = await dataStore.GetResourceAsync<IAccount>("/foobarAccount", o => o.Expand(x => x.GetDirectory()));

            await GeneratedArgumentsWere(dataStore, "expand=directory");
        }

        [Fact]
        public async Task With_group_memberships()
        {
            var dataStore = this.BuildDataStore(FakeJson.Account);

            var account = await dataStore.GetResourceAsync<IAccount>("/foobarAccount", o => o.Expand(x => x.GetGroupMemberships()));

            await GeneratedArgumentsWere(dataStore, "expand=groupMemberships");
        }

        [Fact]
        public async Task With_group_memberships_overloaded()
        {
            var dataStore = this.BuildDataStore(FakeJson.Account);

            var account = await dataStore.GetResourceAsync<IAccount>("/foobarAccount", o => o.Expand(x => x.GetGroupMemberships(10, 10)));

            await GeneratedArgumentsWere(dataStore, "expand=groupMemberships(offset:10,limit:10)");
        }

        [Fact]
        public async Task With_groups()
        {
            var dataStore = this.BuildDataStore(FakeJson.Account);

            var account = await dataStore.GetResourceAsync<IAccount>("/foobarAccount", o => o.Expand(x => x.GetGroups()));

            await GeneratedArgumentsWere(dataStore, "expand=groups");
        }

        [Fact]
        public async Task With_groups_overloaded()
        {
            var dataStore = this.BuildDataStore(FakeJson.Account);

            var account = await dataStore.GetResourceAsync<IAccount>("/foobarAccount", o => o.Expand(x => x.GetGroups(10, 10)));

            await GeneratedArgumentsWere(dataStore, "expand=groups(offset:10,limit:10)");
        }

        [Fact]
        public async Task With_tenant()
        {
            var dataStore = this.BuildDataStore(FakeJson.Account);

            var account = await dataStore.GetResourceAsync<IAccount>("/foobarAccount", o => o.Expand(x => x.GetTenant()));

            await GeneratedArgumentsWere(dataStore, "expand=tenant");
        }

        [Fact]
        public async Task With_account_store()
        {
            var dataStore = this.BuildDataStore(FakeJson.AccountStoreMapping);

            var account = await dataStore.GetResourceAsync<IAccountStoreMapping>("/foobarASM", o => o.Expand(x => x.GetAccountStore()));

            await GeneratedArgumentsWere(dataStore, "expand=accountStore");
        }

        [Fact]
        public async Task With_application()
        {
            var dataStore = this.BuildDataStore(FakeJson.AccountStoreMapping);

            var account = await dataStore.GetResourceAsync<IAccountStoreMapping>("/foobarASM", o => o.Expand(x => x.GetApplication()));

            await GeneratedArgumentsWere(dataStore, "expand=application");
        }

        [Fact]
        public async Task With_accounts()
        {
            var dataStore = this.BuildDataStore(FakeJson.Application);

            var account = await dataStore.GetResourceAsync<IApplication>("/foobarApplication", o => o.Expand(x => x.GetAccounts()));

            await GeneratedArgumentsWere(dataStore, "expand=accounts");
        }

        [Fact]
        public async Task With_accounts_overloaded()
        {
            var dataStore = this.BuildDataStore(FakeJson.Application);

            var account = await dataStore.GetResourceAsync<IApplication>("/foobarApplication", o => o.Expand(x => x.GetAccounts(10, 10)));

            await GeneratedArgumentsWere(dataStore, "expand=accounts(offset:10,limit:10)");
        }

        [Fact]
        public async Task With_account_store_mappings()
        {
            var dataStore = this.BuildDataStore(FakeJson.Directory);

            var account = await dataStore.GetResourceAsync<IApplication>("/foobarApplication", o => o.Expand(x => x.GetAccountStoreMappings()));

            await GeneratedArgumentsWere(dataStore, "expand=accountStoreMappings");
        }

        [Fact]
        public async Task With_account_store_mappings_overloaded()
        {
            var dataStore = this.BuildDataStore(FakeJson.Account);

            var account = await dataStore.GetResourceAsync<IApplication>("/foobarApplication", o => o.Expand(x => x.GetAccountStoreMappings(10, 10)));

            await GeneratedArgumentsWere(dataStore, "expand=accountStoreMappings(offset:10,limit:10)");
        }

        [Fact]
        public async Task With_account_memberships()
        {
            var dataStore = this.BuildDataStore(FakeJson.Group);

            var account = await dataStore.GetResourceAsync<IGroup>("/group1", o => o.Expand(x => x.GetAccountMemberships()));

            await GeneratedArgumentsWere(dataStore, "expand=accountMemberships");
        }

        [Fact]
        public async Task With_account_memberships_overloaded()
        {
            var dataStore = this.BuildDataStore(FakeJson.Group);

            var account = await dataStore.GetResourceAsync<IGroup>("/group1", o => o.Expand(x => x.GetAccountMemberships(10, 10)));

            await GeneratedArgumentsWere(dataStore, "expand=accountMemberships(offset:10,limit:10)");
        }

        [Fact]
        public async Task With_applications()
        {
            var dataStore = this.BuildDataStore(FakeJson.Tenant);

            var account = await dataStore.GetResourceAsync<ITenant>("/tenants/foo-bar", o => o.Expand(x => x.GetApplications()));

            await GeneratedArgumentsWere(dataStore, "expand=applications");
        }

        [Fact]
        public async Task With_applications_overloaded()
        {
            var dataStore = this.BuildDataStore(FakeJson.Tenant);

            var account = await dataStore.GetResourceAsync<ITenant>("/tenants/foo-bar", o => o.Expand(x => x.GetApplications(10, 10)));

            await GeneratedArgumentsWere(dataStore, "expand=applications(offset:10,limit:10)");
        }

        [Fact]
        public async Task With_directories()
        {
            var dataStore = this.BuildDataStore(FakeJson.Tenant);

            var account = await dataStore.GetResourceAsync<ITenant>("/tenants/foo-bar", o => o.Expand(x => x.GetDirectories()));

            await GeneratedArgumentsWere(dataStore, "expand=directories");
        }

        [Fact]
        public async Task With_directories_overloaded()
        {
            var dataStore = this.BuildDataStore(FakeJson.Tenant);

            var account = await dataStore.GetResourceAsync<ITenant>("/tenants/foo-bar", o => o.Expand(x => x.GetDirectories(10, 10)));

            await GeneratedArgumentsWere(dataStore, "expand=directories(offset:10,limit:10)");
        }

        [Fact]
        public async Task With_account()
        {
            var dataStore = this.BuildDataStore(FakeJson.GroupMembership);

            var account = await dataStore.GetResourceAsync<IGroupMembership>("/foobarGM", o => o.Expand(x => x.GetAccount()));

            await GeneratedArgumentsWere(dataStore, "expand=account");
        }

        [Fact]
        public async Task With_group()
        {
            var dataStore = this.BuildDataStore(FakeJson.GroupMembership);

            var account = await dataStore.GetResourceAsync<IGroupMembership>("/foobarGM", o => o.Expand(x => x.GetGroup()));

            await GeneratedArgumentsWere(dataStore, "expand=group");
        }
    }
}
