﻿// <copyright file="DefaultAccountResult_tests.cs" company="Stormpath, Inc.">
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

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using Shouldly;
using Stormpath.SDK.Http;
using Stormpath.SDK.IdSite;
using Stormpath.SDK.Impl.Resource;
using Stormpath.SDK.Tests.Common;
using Stormpath.SDK.Tests.Common.Fakes;
using Xunit;

namespace Stormpath.SDK.Tests
{
    public class DefaultAccountResult_tests
    {
        [Fact]
        public async Task When_getting_account()
        {
            var dataStore = TestDataStore.Create(new StubRequestExecutor(FakeJson.Account).Object);

            var accountResult = dataStore.InstantiateWithData<IAccountResult>(
                new Dictionary<string, object>()
                {
                    ["account"] = new LinkProperty("https://foo.bar/account1")
                });

            var account = await accountResult.GetAccountAsync();
            account.FullName.ShouldBe("Han Solo");

            await dataStore.RequestExecutor.Received().ExecuteAsync(
                Arg.Is<IHttpRequest>(x => x.CanonicalUri.ToString() == "https://foo.bar/account1"),
                Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task When_getting_account_and_no_account_present_throws()
        {
            var dataStore = TestDataStore.Create(new StubRequestExecutor(FakeJson.Account).Object);

            var accountResult = dataStore.InstantiateWithData<IAccountResult>(
                new Dictionary<string, object>()
                {
                    ["state"] = "foobar"
                });

            await Should.ThrowAsync<Exception>(async () =>
            {
                var account = await accountResult.GetAccountAsync();
            });

            await dataStore.RequestExecutor.DidNotReceive().ExecuteAsync(
                Arg.Any<IHttpRequest>(),
                Arg.Any<CancellationToken>());
        }
    }
}
