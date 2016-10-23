﻿// <copyright file="IPasswordGrantAuthenticationAttempt.cs" company="Stormpath, Inc.">
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
using Stormpath.SDK.AccountStore;
using Stormpath.SDK.Resource;

namespace Stormpath.SDK.Impl.Oauth
{
    /// <summary>
    /// Represents the information required to build a Password Grant Authentication request.
    /// </summary>
    [Obsolete("Remove in 1.0")]
    internal interface IPasswordGrantAuthenticationAttempt : IResource
    {
        /// <summary>
        /// Gets the <see cref="IAccountStore">Account Store</see> <c>href</c> that will used for the token exchange request.
        /// </summary>
        /// <value>The <see cref="IAccountStore">Account Store</see> <c>href</c> that will used for the token exchange request.</value>
        string AccountStoreHref { get; }

        /// <summary>
        /// Gets the <see cref="Stormpath.SDK.Organization.IOrganization">Organization</see> <c>nameKey</c> that will used for the token exchange request.
        /// </summary>
        /// <value>The <c>nameKey</c> that will used for the token exchange request.</value>
        string OrganizationNameKey { get; }

        /// <summary>
        /// Gets the username to be used in the token exchange request.
        /// </summary>
        /// <value>The username to be used in the token exchange request.</value>
        string Login { get; }

        /// <summary>
        /// Gets the password to be used in the token exchange request.
        /// </summary>
        /// <value>The password to be used in the token exchange request.</value>
        string Password { get; }

        /// <summary>
        /// Sets the <see cref="IAccountStore">Account Store</see> that will be used for the token exchange request.
        /// </summary>
        /// <param name="accountStore">The Account Store.</param>
        void SetAccountStore(IAccountStore accountStore);

        /// <summary>
        /// Sets the <see cref="Stormpath.SDK.Organization.IOrganization">Organization</see> <c>nameKey</c> that will be used for the token exchange request.
        /// </summary>
        /// <param name="nameKey">The <c>nameKey</c>.</param>
        void SetOrganizationNameKey(string nameKey);

        /// <summary>
        /// Sets the <see cref="IAccountStore">Account Store</see> <c>href</c> that will be used for the token exchange request.
        /// </summary>
        /// <param name="accountStoreHref">The Account Store <c>href</c>.</param>
        void SetAccountStore(string accountStoreHref);

        /// <summary>
        /// Sets the username to be used in the token exchange request.
        /// </summary>
        /// <param name="login">The username.</param>
        void SetLogin(string login);

        /// <summary>
        /// Sets the password to be used in the token exchange request.
        /// </summary>
        /// <param name="password">The password.</param>
        void SetPassword(string password);
    }
}