﻿// <copyright file="CreationOptions_tests.cs" company="Stormpath, Inc.">
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
using Stormpath.SDK.Account;
using Stormpath.SDK.Application;
using Stormpath.SDK.Directory;
using Stormpath.SDK.Group;
using Stormpath.SDK.Http;
using Stormpath.SDK.Impl.DataStore;
using Stormpath.SDK.Impl.Http;
using Stormpath.SDK.Resource;
using Stormpath.SDK.Tests.Common;
using Stormpath.SDK.Tests.Common.Fakes;
using Xunit;

namespace Stormpath.SDK.Tests
{
    public class CreationOptions_tests
    {
        private static void VerifyRequestContents(IRequestExecutor executor, string queryString)
        {
            executor.Received()
                .ExecuteAsync(
                    Arg.Is<IHttpRequest>(request =>
                        request.CanonicalUri.ToString().EndsWith(queryString)),
                    Arg.Any<CancellationToken>());
        }

        public class Application_options : IDisposable
        {
            private readonly IInternalAsyncDataStore dataStore;

            public Application_options()
            {
                this.dataStore = TestDataStore.Create(new StubRequestExecutor(FakeJson.Application).Object) as IInternalAsyncDataStore;
            }

            private async Task VerifyThat(ICreationOptions options, string resultsInQueryString)
            {
                var newApplication = this.dataStore.Instantiate<IApplication>();
                await this.dataStore.CreateAsync("/application", newApplication, options, CancellationToken.None);

                VerifyRequestContents(this.dataStore.RequestExecutor, resultsInQueryString);
            }

            [Fact]
            public async Task Create_without_directory()
            {
                var options = new ApplicationCreationOptionsBuilder()
                {
                    CreateDirectory = false
                }.Build();

                await this.VerifyThat(options, resultsInQueryString: string.Empty);
            }

            [Fact]
            public async Task Create_application_request_with_default_directory()
            {
                var options = new ApplicationCreationOptionsBuilder()
                {
                    CreateDirectory = true
                }.Build();

                await this.VerifyThat(options, resultsInQueryString: "?createDirectory=true");
            }

            [Fact]
            public async Task Create_application_request_with_named_directory()
            {
                var options = new ApplicationCreationOptionsBuilder()
                {
                    CreateDirectory = true,
                    DirectoryName = "Foobar Directory"
                }.Build();

                await this.VerifyThat(options, resultsInQueryString: "?createDirectory=Foobar+Directory");
            }

            [Fact]
            public async Task Create_with_response_options()
            {
                var optionsBuilder = new ApplicationCreationOptionsBuilder();
                optionsBuilder.ResponseOptions.Expand(x => x.GetCustomData());
                var options = optionsBuilder.Build();

                await this.VerifyThat(options, resultsInQueryString: "?expand=customData");
            }

            [Fact]
            public async Task Create_with_all_options()
            {
                var optionsBuilder = new ApplicationCreationOptionsBuilder();
                optionsBuilder.CreateDirectory = true;
                optionsBuilder.DirectoryName = "Foobar Directory";
                optionsBuilder.ResponseOptions.Expand(x => x.GetAccounts(0, 10));
                var options = optionsBuilder.Build();

                await this.VerifyThat(options, resultsInQueryString: "?createDirectory=Foobar+Directory&expand=accounts(offset:0,limit:10)");
            }

            public void Dispose()
            {
                this.dataStore.Dispose();
            }
        }

        public class Account_options : IDisposable
        {
            private readonly IInternalAsyncDataStore dataStore;

            public Account_options()
            {
                this.dataStore = TestDataStore.Create(new StubRequestExecutor(FakeJson.Account).Object) as IInternalAsyncDataStore;
            }

            private async Task VerifyThat(ICreationOptions options, string resultsInQueryString)
            {
                var newAccount = this.dataStore.Instantiate<IAccount>();
                await this.dataStore.CreateAsync("/account", newAccount, options, CancellationToken.None);

                VerifyRequestContents(this.dataStore.RequestExecutor, resultsInQueryString);
            }

            [Fact]
            public async Task Create_without_workflow_option()
            {
                var options = new AccountCreationOptionsBuilder()
                {
                }.Build();

                await this.VerifyThat(options, resultsInQueryString: string.Empty);
            }

            [Fact]
            public async Task Create_with_workflow_override_enabled()
            {
                var options = new AccountCreationOptionsBuilder()
                {
                    RegistrationWorkflowEnabled = true
                }.Build();

                await this.VerifyThat(options, resultsInQueryString: "?registrationWorkflowEnabled=true");
            }

            [Fact]
            public async Task Create_with_workflow_override_disabled()
            {
                var options = new AccountCreationOptionsBuilder()
                {
                    RegistrationWorkflowEnabled = false
                }.Build();

                await this.VerifyThat(options, resultsInQueryString: "?registrationWorkflowEnabled=false");
            }

            [Fact]
            public async Task Create_with_response_options()
            {
                var optionsBuilder = new AccountCreationOptionsBuilder();
                optionsBuilder.ResponseOptions.Expand(x => x.GetCustomData());
                var options = optionsBuilder.Build();

                await this.VerifyThat(options, resultsInQueryString: "?expand=customData");
            }

            [Fact]
            public async Task Create_with_password_format()
            {
                var options = new AccountCreationOptionsBuilder()
                {
                    PasswordFormat = PasswordFormat.MCF
                }.Build();

                await this.VerifyThat(options, resultsInQueryString: "?passwordFormat=mcf");
            }

            [Fact]
            public async Task Create_with_all_options()
            {
                var optionsBuilder = new AccountCreationOptionsBuilder();
                optionsBuilder.RegistrationWorkflowEnabled = true;
                optionsBuilder.PasswordFormat = PasswordFormat.MCF;
                optionsBuilder.ResponseOptions.Expand(x => x.GetGroups(0, 10));
                var options = optionsBuilder.Build();

                await this.VerifyThat(options, resultsInQueryString: "?expand=groups(offset:0,limit:10)&passwordFormat=mcf&registrationWorkflowEnabled=true");
            }

            public void Dispose()
            {
                this.dataStore.Dispose();
            }
        }

        public class Directory_options : IDisposable
        {
            private readonly IInternalAsyncDataStore dataStore;

            public Directory_options()
            {
                this.dataStore = TestDataStore.Create(new StubRequestExecutor(FakeJson.Directory).Object) as IInternalAsyncDataStore;
            }

            private async Task VerifyThat(ICreationOptions options, string resultsInQueryString)
            {
                var newDirectory = this.dataStore.Instantiate<IDirectory>();
                await this.dataStore.CreateAsync("/directories", newDirectory, options, CancellationToken.None);

                VerifyRequestContents(this.dataStore.RequestExecutor, resultsInQueryString);
            }

            [Fact]
            public async Task Create_with_response_options()
            {
                var optionsBuilder = new DirectoryCreationOptionsBuilder();
                optionsBuilder.ResponseOptions.Expand(x => x.GetCustomData());
                var options = optionsBuilder.Build();

                await this.VerifyThat(options, resultsInQueryString: "?expand=customData");
            }

            [Fact]
            public async Task Create_with_all_options()
            {
                var optionsBuilder = new DirectoryCreationOptionsBuilder();
                optionsBuilder.ResponseOptions.Expand(x => x.GetGroups(0, 10));
                var options = optionsBuilder.Build();

                await this.VerifyThat(options, resultsInQueryString: "?expand=groups(offset:0,limit:10)");
            }

            public void Dispose()
            {
                this.dataStore.Dispose();
            }
        }

        public class Group_options : IDisposable
        {
            private readonly IInternalAsyncDataStore dataStore;

            public Group_options()
            {
                this.dataStore = TestDataStore.Create(new StubRequestExecutor(FakeJson.Group).Object) as IInternalAsyncDataStore;
            }

            private async Task VerifyThat(ICreationOptions options, string resultsInQueryString)
            {
                var newGroup = this.dataStore.Instantiate<IDirectory>();
                await this.dataStore.CreateAsync("/groups", newGroup, options, CancellationToken.None);

                VerifyRequestContents(this.dataStore.RequestExecutor, resultsInQueryString);
            }

            [Fact]
            public async Task Create_with_response_options()
            {
                var optionsBuilder = new GroupCreationOptionsBuilder();
                optionsBuilder.ResponseOptions.Expand(x => x.GetCustomData());
                var options = optionsBuilder.Build();

                await this.VerifyThat(options, resultsInQueryString: "?expand=customData");
            }

            [Fact]
            public async Task Create_with_all_options()
            {
                var optionsBuilder = new GroupCreationOptionsBuilder();
                optionsBuilder.ResponseOptions.Expand(x => x.GetAccounts(0, 10));
                var options = optionsBuilder.Build();

                await this.VerifyThat(options, resultsInQueryString: "?expand=accounts(offset:0,limit:10)");
            }

            public void Dispose()
            {
                this.dataStore.Dispose();
            }
        }
    }
}
