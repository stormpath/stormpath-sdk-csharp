﻿// <copyright file="DefaultDataStore_tests.cs" company="Stormpath, Inc.">
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

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using Shouldly;
using Stormpath.SDK.Account;
using Stormpath.SDK.Application;
using Stormpath.SDK.Client;
using Stormpath.SDK.Error;
using Stormpath.SDK.Http;
using Stormpath.SDK.Impl.Account;
using Stormpath.SDK.Impl.DataStore;
using Stormpath.SDK.Impl.Linq;
using Stormpath.SDK.Impl.Resource;
using Stormpath.SDK.Impl.Utility;
using Stormpath.SDK.Linq;
using Stormpath.SDK.Logging;
using Stormpath.SDK.Tests.Common;
using Stormpath.SDK.Tests.Common.Fakes;
using Xunit;

namespace Stormpath.SDK.Tests
{
    public class DefaultDataStore_tests
    {
        [Fact]
        public async Task Single_item_data_is_deserialized_properly()
        {
            var dataStore = TestDataStore.Create(new StubRequestExecutor(FakeJson.Account).Object);

            var account = await dataStore.GetResourceAsync<IAccount>("/account", CancellationToken.None);

            // Verify against data from FakeJson.Account
            account.CreatedAt.ShouldBe(Iso8601.Parse("2015-07-21T23:50:49.078Z"));
            account.Email.ShouldBe("han.solo@corellia.core");
            account.FullName.ShouldBe("Han Solo");
            account.GivenName.ShouldBe("Han");
            account.Href.ShouldBe("https://api.stormpath.com/v1/accounts/foobarAccount");
            account.MiddleName.ShouldBeNull();
            account.ModifiedAt.ShouldBe(Iso8601.Parse("2015-07-21T23:50:49.078Z"));
            account.Status.ShouldBe(AccountStatus.Enabled);
            account.Surname.ShouldBe("Solo");
            account.Username.ShouldBe("han.solo@corellia.core");

            (account as DefaultAccount).AccessTokens.Href.ShouldBe("https://api.stormpath.com/v1/accounts/foobarAccount/accessTokens");
            (account as DefaultAccount).ApiKeys.Href.ShouldBe("https://api.stormpath.com/v1/accounts/foobarAccount/apiKeys");
            (account as DefaultAccount).Applications.Href.ShouldBe("https://api.stormpath.com/v1/accounts/foobarAccount/applications");
            (account as DefaultAccount).CustomData.Href.ShouldBe("https://api.stormpath.com/v1/accounts/foobarAccount/customData");
            (account as DefaultAccount).Directory.Href.ShouldBe("https://api.stormpath.com/v1/directories/foobarDirectory");
            (account as DefaultAccount).EmailVerificationToken.Href.ShouldBeNull();
            (account as DefaultAccount).GroupMemberships.Href.ShouldBe("https://api.stormpath.com/v1/accounts/foobarAccount/groupMemberships");
            (account as DefaultAccount).Groups.Href.ShouldBe("https://api.stormpath.com/v1/accounts/foobarAccount/groups");
            (account as DefaultAccount).ProviderData.Href.ShouldBe("https://api.stormpath.com/v1/accounts/foobarAccount/providerData");
            (account as DefaultAccount).RefreshTokens.Href.ShouldBe("https://api.stormpath.com/v1/accounts/foobarAccount/refreshTokens");
            (account as DefaultAccount).Tenant.Href.ShouldBe("https://api.stormpath.com/v1/tenants/foobarTenant");
        }

        [Fact]
        public async Task Collection_resource_data_is_deserialized_properly()
        {
            var dataStore = TestDataStore.Create(new StubRequestExecutor(FakeJson.AccountList).Object);

            IAsyncQueryable<IAccount> accounts = new CollectionResourceQueryable<IAccount>("/accounts", dataStore);
            await accounts.MoveNextAsync();

            // Verify against data from FakeJson.AccountList
            (accounts as CollectionResourceQueryable<IAccount>).Size.ShouldBe(6);
            (accounts as CollectionResourceQueryable<IAccount>).Offset.ShouldBe(0);
            (accounts as CollectionResourceQueryable<IAccount>).Limit.ShouldBe(25);
            accounts.CurrentPage.Count().ShouldBe(6);

            var account = accounts.CurrentPage.First();
            account.CreatedAt.ShouldBe(Iso8601.Parse("2015-07-21T23:50:49.078Z"));
            account.Email.ShouldBe("han.solo@corellia.core");
            account.FullName.ShouldBe("Han Solo");
            account.GivenName.ShouldBe("Han");
            account.Href.ShouldBe("https://api.stormpath.com/v1/accounts/account1");
            account.MiddleName.ShouldBeNull();
            account.ModifiedAt.ShouldBe(Iso8601.Parse("2015-07-21T23:50:49.078Z"));
            account.Status.ShouldBe(AccountStatus.Enabled);
            account.Surname.ShouldBe("Solo");
            account.Username.ShouldBe("han.solo@corellia.core");

            (account as DefaultAccount).AccessTokens.Href.ShouldBe("https://api.stormpath.com/v1/accounts/account1/accessTokens");
            (account as DefaultAccount).ApiKeys.Href.ShouldBe("https://api.stormpath.com/v1/accounts/account1/apiKeys");
            (account as DefaultAccount).Applications.Href.ShouldBe("https://api.stormpath.com/v1/accounts/account1/applications");
            (account as DefaultAccount).CustomData.Href.ShouldBe("https://api.stormpath.com/v1/accounts/account1/customData");
            (account as DefaultAccount).Directory.Href.ShouldBe("https://api.stormpath.com/v1/directories/directory1");
            (account as DefaultAccount).EmailVerificationToken.Href.ShouldBeNull();
            (account as DefaultAccount).GroupMemberships.Href.ShouldBe("https://api.stormpath.com/v1/accounts/account1/groupMemberships");
            (account as DefaultAccount).Groups.Href.ShouldBe("https://api.stormpath.com/v1/accounts/account1/groups");
            (account as DefaultAccount).ProviderData.Href.ShouldBe("https://api.stormpath.com/v1/accounts/account1/providerData");
            (account as DefaultAccount).RefreshTokens.Href.ShouldBe("https://api.stormpath.com/v1/accounts/account1/refreshTokens");
            (account as DefaultAccount).Tenant.Href.ShouldBe("https://api.stormpath.com/v1/tenants/foobarTenant");
        }

        [Fact]
        public async Task Default_headers_are_applied_to_all_requests()
        {
            var dataStore = TestDataStore.Create(new StubRequestExecutor(FakeJson.Account).Object);

            var account = await dataStore.GetResourceAsync<IAccount>("/account", CancellationToken.None);

            // Verify the default headers
            await dataStore.RequestExecutor.Received().ExecuteAsync(
                Arg.Is<IHttpRequest>(request =>
                    request.Headers.Accept == "application/json"),
                Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Trace_log_is_sent_to_logger()
        {
            var fakeLog = new List<LogEntry>();
            var stubLogger = Substitute.For<ILogger>();
            stubLogger.When(x => x.Log(Arg.Any<LogEntry>())).Do(call =>
            {
                fakeLog.Add(call.Arg<LogEntry>());
            });

            var dataStore = TestDataStore.Create(new StubRequestExecutor(FakeJson.Account).Object, logger: stubLogger);

            var account = await dataStore.GetResourceAsync<IAccount>("account", CancellationToken.None);
            await account.DeleteAsync();

            fakeLog.Count.ShouldBeGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task Saving_resource_posts_changed_values_only()
        {
            string savedHref = null;
            string savedJson = null;

            var dataStore = TestDataStore.Create(new StubRequestExecutor(FakeJson.Account).Object);
            dataStore.RequestExecutor
                .When(x => x.ExecuteAsync(Arg.Any<IHttpRequest>(), Arg.Any<CancellationToken>()))
                .Do(call =>
                {
                    savedHref = call.Arg<IHttpRequest>().CanonicalUri.ToString();
                    savedJson = call.Arg<IHttpRequest>().Body;
                });

            var account = await dataStore.GetResourceAsync<IAccount>("/account", CancellationToken.None);
            account.Href.ShouldBe("https://api.stormpath.com/v1/accounts/foobarAccount");

            account.SetMiddleName("Test");
            account.SetUsername("newusername");

            await account.SaveAsync();

            savedHref.ShouldBe("https://api.stormpath.com/v1/accounts/foobarAccount");

            var savedMap = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(savedJson);
            savedMap.Count.ShouldBe(2);
            savedMap["middleName"].ShouldBe("Test");
            savedMap["username"].ShouldBe("newusername");
        }

        [Fact]
        public async Task When_saving_returns_empty_response_without_status_202_throws_error()
        {
            // Expected behavior: Saving should always return the updated data unless we get back HTTP 202 (Accepted for Processing).
            var dataStore = TestDataStore.Create(new StubRequestExecutor(FakeJson.Account).Object) as IInternalAsyncDataStore;
            dataStore.RequestExecutor
                .ExecuteAsync(Arg.Any<IHttpRequest>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<IHttpResponse>(new FakeHttpResponse(200, "OK", null, null, null, transportError: false)));

            var account = dataStore.Instantiate<IAccount>();
            account.SetMiddleName("Test");
            account.SetUsername("newusername");

            bool erroredAsExpected = false;
            try
            {
                await dataStore.CreateAsync("http://api.foo.bar/accounts", account, CancellationToken.None);
            }
            catch (ResourceException rex)
            {
                rex.DeveloperMessage.ShouldBe("Unable to obtain resource data from the API server.");
                erroredAsExpected = true;
            }

            erroredAsExpected.ShouldBeTrue();
        }

        [Fact]
        public async Task When_saving_returns_empty_response_with_status_202()
        {
            // Expected behavior: Saving should always return the updated data unless we get back HTTP 202 (Accepted for Processing).
            var dataStore = TestDataStore.Create(new StubRequestExecutor(FakeJson.Account).Object) as IInternalAsyncDataStore;
            dataStore.RequestExecutor
                .ExecuteAsync(Arg.Any<IHttpRequest>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<IHttpResponse>(new FakeHttpResponse(202, "Accepted", null, null, null, transportError: false)));

            var account = dataStore.Instantiate<IAccount>();
            account.SetMiddleName("Test");
            account.SetUsername("newusername");

            var result = await dataStore.CreateAsync("http://api.foo.bar/accounts", account, CancellationToken.None);
            bool isEmpty = (result as AbstractResource).GetPropertyNames().Single() == "href";
            isEmpty.ShouldBeTrue();

            result.Href.ShouldBeNullOrEmpty();
        }

        [Fact]
        public async Task Cancellation_token_is_passed_down_to_low_level_operations()
        {
            var dataStore = TestDataStore.Create(new StubRequestExecutor(FakeJson.Account).Object) as IInternalAsyncDataStore;
            dataStore.RequestExecutor
                .ExecuteAsync(Arg.Any<IHttpRequest>(), Arg.Any<CancellationToken>())
                .Returns(async callInfo =>
                {
                    // Will pause for 1 second, unless CancellationToken has been passed through to us
                    await Task.Delay(1000, callInfo.Arg<CancellationToken>());
                    return new FakeHttpResponse(204, "No Content", new HttpHeaders(), null, null, transportError: false) as IHttpResponse;
                });

            var fakeAccount = dataStore.InstantiateWithHref<IAccount>("http://api.foo.bar/accounts/1");

            var alreadyCanceledSource = new CancellationTokenSource();
            alreadyCanceledSource.Cancel();

            var stopwatch = Stopwatch.StartNew();
            var deleted = false;
            try
            {
                await dataStore.DeleteAsync(fakeAccount, alreadyCanceledSource.Token);
            }
            catch (TaskCanceledException)
            {
                deleted = true;
            }

            stopwatch.Stop();
            stopwatch.ElapsedMilliseconds.ShouldBeLessThan(1000);
            deleted.ShouldBeTrue();
        }

        [Fact]
        public void Instantiated_resources_contain_reference_to_client()
        {
            var fakeClient = Substitute.For<IClient>();
            var dataStore = TestDataStore.Create(client: fakeClient);

            var account = dataStore.Instantiate<IAccount>();

            account.Client.ShouldBe(fakeClient);
        }

        [Fact]
        public async Task Supports_list_property()
        {
            var dataStore = TestDataStore.Create(new StubRequestExecutor(FakeJson.Application).Object);

            var application = await dataStore.GetResourceAsync<IApplication>("/application", CancellationToken.None);

            // Verify against data from FakeJson.Application
            application.Name.ShouldBe("Lightsabers Galore");

            // AuthorizedCallbackUris should be a populated list
            application.AuthorizedCallbackUris.Count.ShouldBe(2);
            application.AuthorizedCallbackUris.ShouldContain("https://foo.bar/1");
            application.AuthorizedCallbackUris.ShouldContain("https://foo.bar/2");
        }

        /// <summary>
        /// Regression test for https://github.com/stormpath/stormpath-sdk-dotnet/issues/212
        /// </summary>
        /// <returns>A Task that represents the asynchronous test.</returns>
        [Fact]
        public async Task Supports_email_verification_status_unknown()
        {
            var dataStore = TestDataStore.Create(new StubRequestExecutor(FakeJson.Account).Object);

            var account = await dataStore.GetResourceAsync<IAccount>("/account", CancellationToken.None);

            // Verify against data from FakeJson.Application
            account.EmailVerificationStatus.ShouldBe(EmailVerificationStatus.Unknown);
        }

    }
}
