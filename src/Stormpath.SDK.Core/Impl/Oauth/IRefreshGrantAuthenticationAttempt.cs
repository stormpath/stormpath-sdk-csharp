﻿// <copyright file="IRefreshGrantAuthenticationAttempt.cs" company="Stormpath, Inc.">
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
using Stormpath.SDK.Resource;

namespace Stormpath.SDK.Impl.Oauth
{
    /// <summary>
    /// Represents the information required to build a Refresh Grant Authentication request.
    /// </summary>
    [Obsolete("Remove in 1.0")]
    internal interface IRefreshGrantAuthenticationAttempt : IResource
    {
        /// <summary>
        /// Gets the Refresh Token that will be used for the token exchange request.
        /// </summary>
        /// <value>The Refresh Token that will be used for the token exchange request.</value>
        string RefreshToken { get; }

        /// <summary>
        /// Sets the Refresh Token that will be used for the token exchange request.
        /// </summary>
        /// <param name="refreshToken">The string representation of the Refresh Token.</param>
        void SetRefreshToken(string refreshToken);
    }
}