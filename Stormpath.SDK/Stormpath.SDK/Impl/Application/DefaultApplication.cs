﻿// <copyright file="DefaultApplication.cs" company="Stormpath, Inc.">
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

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Stormpath.SDK.Account;
using Stormpath.SDK.AccountStore;
using Stormpath.SDK.Application;
using Stormpath.SDK.Auth;
using Stormpath.SDK.Impl.Account;
using Stormpath.SDK.Impl.Auth;
using Stormpath.SDK.Impl.DataStore;
using Stormpath.SDK.Impl.Resource;
using Stormpath.SDK.Impl.Utility;
using Stormpath.SDK.Linq;
using Stormpath.SDK.Resource;
using Stormpath.SDK.Sync;

namespace Stormpath.SDK.Impl.Application
{
    internal sealed class DefaultApplication : AbstractExtendableInstanceResource, IApplication, IApplicationSync
    {
        private static readonly string AccountStoreMappingsPropertyName = "accountStoreMappings";
        private static readonly string AccountsPropertyName = "accounts";
        private static readonly string ApiKeysPropertyName = "apiKeys";
        private static readonly string AuthTokensPropertyName = "authTokens";
        private static readonly string DefaultAccountStoreMappingPropertyName = "defaultAccountStoreMapping";
        private static readonly string DefaultGroupStoreMappingPropertyName = "defaultGroupStoreMapping";
        private static readonly string DescriptionPropertyName = "description";
        private static readonly string GroupsPropertyName = "groups";
        private static readonly string LoginAttemptsPropertyName = "loginAttempts";
        private static readonly string NamePropertyName = "name";
        private static readonly string OAuthPolicyPropertyName = "oAuthPolicy";
        private static readonly string PasswordResetTokensPropertyName = "passwordResetTokens";
        private static readonly string StatusPropertyName = "status";
        private static readonly string TenantPropertyName = "tenant";
        private static readonly string VerificationEmailsPropertyName = "verificationEmails";

        public DefaultApplication(IInternalDataStore dataStore)
            : base(dataStore)
        {
        }

        public DefaultApplication(IInternalDataStore dataStore, IDictionary<string, object> properties)
            : base(dataStore, properties)
        {
        }

        private IApplication AsInterface => this;

        private IApplicationSync AsSyncInterface => this;

        internal LinkProperty AccountStoreMappings => this.GetLinkProperty(AccountStoreMappingsPropertyName);

        internal LinkProperty Accounts => this.GetLinkProperty(AccountsPropertyName);

        internal LinkProperty ApiKeys => this.GetLinkProperty(ApiKeysPropertyName);

        internal LinkProperty AuthTokens => this.GetLinkProperty(AuthTokensPropertyName);

        internal LinkProperty DefaultAccountStoreMapping => this.GetLinkProperty(DefaultAccountStoreMappingPropertyName);

        internal LinkProperty DefaultGroupStoreMapping => this.GetLinkProperty(DefaultGroupStoreMappingPropertyName);

        string IApplication.Description => this.GetProperty<string>(DescriptionPropertyName);

        internal LinkProperty Groups => this.GetLinkProperty(GroupsPropertyName);

        string IApplication.Name => this.GetProperty<string>(NamePropertyName);

        internal LinkProperty LoginAttempts => this.GetLinkProperty(LoginAttemptsPropertyName);

        internal LinkProperty OAuthPolicy => this.GetLinkProperty(OAuthPolicyPropertyName);

        internal LinkProperty PasswordResetToken => this.GetLinkProperty(PasswordResetTokensPropertyName);

        ApplicationStatus IApplication.Status => this.GetProperty<ApplicationStatus>(StatusPropertyName);

        internal LinkProperty Tenant => this.GetLinkProperty(TenantPropertyName);

        internal LinkProperty VerificationEmails => this.GetLinkProperty(VerificationEmailsPropertyName);

        IApplication IApplication.SetDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));

            this.SetProperty(DescriptionPropertyName, description);
            return this;
        }

        IApplication IApplication.SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            this.SetProperty(NamePropertyName, name);
            return this;
        }

        IApplication IApplication.SetStatus(ApplicationStatus status)
        {
            this.SetProperty(StatusPropertyName, status);
            return this;
        }

        Task<IAuthenticationResult> IApplication.AuthenticateAccountAsync(IAuthenticationRequest request, CancellationToken cancellationToken)
        {
            var dispatcher = new AuthenticationRequestDispatcher();

            return dispatcher.AuthenticateAsync(this.GetInternalDataStore(), this, request, cancellationToken);
        }

        IAuthenticationResult IApplicationSync.AuthenticateAccount(IAuthenticationRequest request)
        {
            var dispatcher = new AuthenticationRequestDispatcher();

            return dispatcher.Authenticate(this.GetInternalDataStore(), this, request);
        }

        Task<IAuthenticationResult> IApplication.AuthenticateAccountAsync(string username, string password, CancellationToken cancellationToken)
        {
            var request = new UsernamePasswordRequest(username, password) as IAuthenticationRequest;

            return this.AsInterface.AuthenticateAccountAsync(request, cancellationToken);
        }

        IAuthenticationResult IApplicationSync.AuthenticateAccount(string username, string password)
        {
            var request = new UsernamePasswordRequest(username, password) as IAuthenticationRequest;

            return this.AsSyncInterface.AuthenticateAccount(request);
        }

        async Task<bool> IApplication.TryAuthenticateAccountAsync(string username, string password, CancellationToken cancellationToken)
        {
            try
            {
                var loginResult = await this.AsInterface.AuthenticateAccountAsync(username, password, cancellationToken).ConfigureAwait(false);
                return true;
            }
            catch
            {
                return false;
            }
        }

        bool IApplicationSync.TryAuthenticateAccount(string username, string password)
        {
            try
            {
                var loginResult = this.AsSyncInterface.AuthenticateAccount(username, password);
                return true;
            }
            catch
            {
                return false;
            }
        }

        Task<IAccount> IAccountCreationActions.CreateAccountAsync(IAccount account, Action<AccountCreationOptionsBuilder> creationOptionsAction, CancellationToken cancellationToken)
        {
            var builder = new AccountCreationOptionsBuilder();
            creationOptionsAction(builder);
            var options = builder.Build();

            return this.GetInternalDataStore().CreateAsync(this.Accounts.Href, account, options, cancellationToken);
        }

        IAccount IAccountCreationActionsSync.CreateAccount(IAccount account, Action<AccountCreationOptionsBuilder> creationOptionsAction)
        {
            var builder = new AccountCreationOptionsBuilder();
            creationOptionsAction(builder);
            var options = builder.Build();

            return this.GetInternalDataStoreSync().Create(this.Accounts.Href, account, options);
        }

        Task<IAccount> IAccountCreationActions.CreateAccountAsync(IAccount account, IAccountCreationOptions creationOptions, CancellationToken cancellationToken)
             => this.GetInternalDataStore().CreateAsync(this.Accounts.Href, account, creationOptions, cancellationToken);

        IAccount IAccountCreationActionsSync.CreateAccount(IAccount account, IAccountCreationOptions creationOptions)
             => this.GetInternalDataStoreSync().Create(this.Accounts.Href, account, creationOptions);

        Task<IAccount> IAccountCreationActions.CreateAccountAsync(IAccount account, CancellationToken cancellationToken)
             => this.GetInternalDataStore().CreateAsync(this.Accounts.Href, account, cancellationToken);

        IAccount IAccountCreationActionsSync.CreateAccount(IAccount account)
             => this.GetInternalDataStoreSync().Create(this.Accounts.Href, account);

        Task<IAccount> IAccountCreationActions.CreateAccountAsync(string givenName, string surname, string email, string password, object customData, CancellationToken cancellationToken)
        {
            var account = this.GetInternalDataStore().Instantiate<IAccount>();
            account.SetGivenName(givenName);
            account.SetSurname(surname);
            account.SetEmail(email);
            account.SetPassword(password);
            account.CustomData.Put(customData);

            return this.AsInterface.CreateAccountAsync(account, cancellationToken: cancellationToken);
        }

        Task<IAccount> IAccountCreationActions.CreateAccountAsync(string givenName, string surname, string email, string password, CancellationToken cancellationToken)
            => this.AsInterface.CreateAccountAsync(givenName, surname, email, password, customData: null, cancellationToken: cancellationToken);

        IAccount IAccountCreationActionsSync.CreateAccount(string givenName, string surname, string email, string password, object customData)
        {
            var account = this.GetInternalDataStore().Instantiate<IAccount>();
            account.SetGivenName(givenName);
            account.SetSurname(surname);
            account.SetEmail(email);
            account.SetPassword(password);
            account.CustomData.Put(customData);

            return this.AsSyncInterface.CreateAccount(account);
        }

        Task<bool> IDeletable.DeleteAsync(CancellationToken cancellationToken)
            => this.GetInternalDataStore().DeleteAsync(this, cancellationToken);

        bool IDeletableSync.Delete()
             => this.GetInternalDataStoreSync().Delete(this);

        Task<IApplication> ISaveable<IApplication>.SaveAsync(CancellationToken cancellationToken)
             => this.SaveAsync<IApplication>(cancellationToken);

        IApplication ISaveableSync<IApplication>.Save()
            => this.GetInternalDataStoreSync().Save<IApplication>(this);

        Task<IPasswordResetToken> IApplication.SendPasswordResetEmailAsync(string email, CancellationToken cancellationToken)
        {
            var token = this.GetInternalDataStore().Instantiate<IPasswordResetToken>() as DefaultPasswordResetToken;
            token.SetEmail(email);

            return this.GetInternalDataStore().CreateAsync(this.PasswordResetToken.Href, (IPasswordResetToken)token, cancellationToken);
        }

        IPasswordResetToken IApplicationSync.SendPasswordResetEmail(string email)
        {
            var token = this.GetInternalDataStore().Instantiate<IPasswordResetToken>() as DefaultPasswordResetToken;
            token.SetEmail(email);

            return this.GetInternalDataStoreSync().Create(this.PasswordResetToken.Href, (IPasswordResetToken)token);
        }

        async Task<IAccount> IApplication.VerifyPasswordResetTokenAsync(string token, CancellationToken cancellationToken)
        {
            string href = $"{this.PasswordResetToken.Href}/{token}";

            var validTokenResponse = await this.GetInternalDataStore().GetResourceAsync<IPasswordResetToken>(href, cancellationToken).ConfigureAwait(false);
            return await validTokenResponse.GetAccountAsync(cancellationToken).ConfigureAwait(false);
        }

        IAccount IApplicationSync.VerifyPasswordResetToken(string token)
        {
            string href = $"{this.PasswordResetToken.Href}/{token}";

            var validTokenResponse = this.GetInternalDataStore().GetResource<IPasswordResetToken>(href);
            return validTokenResponse.GetAccount();
        }

        async Task<IAccount> IApplication.ResetPasswordAsync(string token, string newPassword, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));
            if (string.IsNullOrEmpty(newPassword))
                throw new ArgumentNullException(nameof(newPassword));

            var href = $"{this.PasswordResetToken.Href}/{token}";
            var passwordResetToken = this.GetInternalDataStore().Instantiate<IPasswordResetToken>() as DefaultPasswordResetToken;
            passwordResetToken.SetPassword(newPassword);

            var responseToken = await this.GetInternalDataStore().CreateAsync(href, (IPasswordResetToken)passwordResetToken, cancellationToken).ConfigureAwait(false);
            return await responseToken.GetAccountAsync(cancellationToken).ConfigureAwait(false);
        }

        IAccount IApplicationSync.ResetPassword(string token, string newPassword)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));
            if (string.IsNullOrEmpty(newPassword))
                throw new ArgumentNullException(nameof(newPassword));

            var href = $"{this.PasswordResetToken.Href}/{token}";
            var passwordResetToken = this.GetInternalDataStore().Instantiate<IPasswordResetToken>() as DefaultPasswordResetToken;
            passwordResetToken.SetPassword(newPassword);

            var responseToken = this.GetInternalDataStoreSync().Create(href, (IPasswordResetToken)passwordResetToken);
            return responseToken.GetAccount();
        }

        IAsyncQueryable<IAccount> IApplication.GetAccounts()
             => new CollectionResourceQueryable<IAccount>(this.Accounts.Href, this.GetInternalDataStore());

        IAsyncQueryable<IAccountStoreMapping> IApplication.GetAccountStoreMappings()
             => new CollectionResourceQueryable<IAccountStoreMapping>(this.AccountStoreMappings.Href, this.GetInternalDataStore());

        async Task<IAccountStore> IApplication.GetDefaultAccountStoreAsync(CancellationToken cancellationToken)
        {
            if (this.DefaultAccountStoreMapping.Href == null)
                return null;

            var accountStoreMapping = await this.GetInternalDataStore().GetResourceAsync<IAccountStoreMapping>(this.DefaultAccountStoreMapping.Href, cancellationToken).ConfigureAwait(false);
            if (accountStoreMapping == null)
                return null;

            return await accountStoreMapping.GetAccountStoreAsync().ConfigureAwait(false);
        }

        IAccountStore IApplicationSync.GetDefaultAccountStore()
        {
            if (this.DefaultAccountStoreMapping.Href == null)
                return null;

            var accountStoreMapping = this.GetInternalDataStore().GetResource<IAccountStoreMapping>(this.DefaultAccountStoreMapping.Href);
            if (accountStoreMapping == null)
                return null;

            return accountStoreMapping.GetAccountStore();
        }
    }
}
