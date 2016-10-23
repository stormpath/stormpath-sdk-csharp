﻿// <copyright file="DefaultPasswordGrantAuthenticator.cs" company="Stormpath, Inc.">
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
using Stormpath.SDK.Application;
using Stormpath.SDK.Impl.DataStore;
using Stormpath.SDK.Oauth;

namespace Stormpath.SDK.Impl.Oauth
{
    [Obsolete("Remove in 1.0")]
    internal sealed class DefaultPasswordGrantAuthenticator :
        AbstractGrantAuthenticator<IPasswordGrantRequest>,
        IPasswordGrantAuthenticator,
        IPasswordGrantAuthenticatorSync
    {
        public DefaultPasswordGrantAuthenticator(IApplication application, IInternalDataStore internalDataStore)
            : base(application, internalDataStore)
        {
        }

        async Task<IOauthGrantAuthenticationResult> IOauthAuthenticator<IPasswordGrantRequest, IOauthGrantAuthenticationResult>
            .AuthenticateAsync(IPasswordGrantRequest authenticationRequest, CancellationToken cancellationToken)
        {
            this.ThrowIfInvalid(authenticationRequest);

            var createGrantAttempt = this.BuildGrantAttempt(authenticationRequest);
            var headers = this.GetHeaderWithMediaType();

            return await this.InternalAsyncDataStore.CreateAsync<IPasswordGrantAuthenticationAttempt, IGrantAuthenticationToken>(
                $"{this.application.Href}{OauthTokenPath}",
                createGrantAttempt,
                headers,
                cancellationToken).ConfigureAwait(false);
        }

        IOauthGrantAuthenticationResult IOauthAuthenticatorSync<IPasswordGrantRequest, IOauthGrantAuthenticationResult>
            .Authenticate(IPasswordGrantRequest authenticationRequest)
        {
            this.ThrowIfInvalid(authenticationRequest);

            var createGrantAttempt = this.BuildGrantAttempt(authenticationRequest);
            var headers = this.GetHeaderWithMediaType();

            return this.InternalSyncDataStore.Create<IPasswordGrantAuthenticationAttempt, IGrantAuthenticationToken>(
                $"{this.application.Href}{OauthTokenPath}",
                createGrantAttempt,
                headers);
        }

        private IPasswordGrantAuthenticationAttempt BuildGrantAttempt(IPasswordGrantRequest authenticationRequest)
        {
            var passwordGrantAttempt = this.internalDataStore.Instantiate<IPasswordGrantAuthenticationAttempt>();
            passwordGrantAttempt.SetLogin(authenticationRequest.Login);
            passwordGrantAttempt.SetPassword(authenticationRequest.Password);

            if (!string.IsNullOrEmpty(authenticationRequest.AccountStoreHref))
            {
                passwordGrantAttempt.SetAccountStore(authenticationRequest.AccountStoreHref);
            }

            if (!string.IsNullOrEmpty(authenticationRequest.OrganizationNameKey))
            {
                passwordGrantAttempt.SetOrganizationNameKey(authenticationRequest.OrganizationNameKey);
            }

            return passwordGrantAttempt;
        }
    }
}
