﻿// <copyright file="DefaultIdSiteAsyncCallbackHandler_exception_tests.cs" company="Stormpath, Inc.">
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
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using Shouldly;
using Stormpath.SDK.Api;
using Stormpath.SDK.Cache;
using Stormpath.SDK.Http;
using Stormpath.SDK.IdSite;
using Stormpath.SDK.Impl.DataStore;
using Stormpath.SDK.Impl.Http;
using Stormpath.SDK.Impl.IdSite;
using Stormpath.SDK.Tests.Common;
using Xunit;

namespace Stormpath.SDK.Tests.IdSite
{
    public class DefaultIdSiteAsyncCallbackHandler_exception_tests : IDisposable
    {
        private IInternalDataStore dataStore;

        public void Dispose()
        {
            this.dataStore.Dispose();
        }

        [Theory]
        [MemberData(nameof(ExceptionTests.TestCases), MemberType = typeof(ExceptionTests))]
        public async Task Handle_error(string id_, string jwtResponse, Type expectedExceptionType, int expectedCode, int expectedStatus, string expectedMessage, string expectedDeveloperMessage)
        {
            var testApiKey = ClientApiKeys.Builder()
                .SetId("2EV70AHRTYF0JOA7OEFO3SM29")
                .SetSecret("goPUHQMkS4dlKwl5wtbNd91I+UrRehCsEDJrIrMruK8")
                .Build();
            var fakeRequestExecutor = Substitute.For<IRequestExecutor>();
            fakeRequestExecutor.ApiKey.Returns(testApiKey);

            this.dataStore = TestDataStore.Create(fakeRequestExecutor, CacheProviders.Create().InMemoryCache().Build());

            var request = new DefaultHttpRequest(HttpMethod.Get, new CanonicalUri($"https://foo.bar?{IdSiteClaims.JwtResponse}={jwtResponse}"));

            IIdSiteAsyncCallbackHandler callbackHandler = new DefaultIdSiteAsyncCallbackHandler(this.dataStore, request);

            try
            {
                var accountResult = await callbackHandler.GetAccountResultAsync(CancellationToken.None);

                throw new Exception("Should not reach here. Proper exception was not thrown.");
            }
            catch (IdSiteRuntimeException e) when (expectedExceptionType.IsAssignableFrom(e.GetType()))
            {
                e.Code.ShouldBe(expectedCode);
                e.HttpStatus.ShouldBe(expectedStatus);
                e.Message.ShouldBe(expectedMessage);
                e.DeveloperMessage.ShouldBe(expectedDeveloperMessage);
            }
            catch (Exception e) when (expectedExceptionType.IsAssignableFrom(e.GetType()))
            {
                e.Message.ShouldStartWith(expectedMessage);
            }
        }
    }
}
