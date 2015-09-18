﻿// <copyright file="IAsynchronousHttpClient.cs" company="Stormpath, Inc.">
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

using System.Threading;
using System.Threading.Tasks;

namespace Stormpath.SDK.Http
{
    /// <summary>
    /// An HTTP client that can execute asynchronous requests.
    /// </summary>
    public interface IAsynchronousHttpClient : IHttpClient
    {
        /// <summary>
        /// Executes an HTTP request asynchronously.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Task whose result is the HTTP response.</returns>
        Task<IHttpResponse> ExecuteAsync(IHttpRequest request, CancellationToken cancellationToken = default(CancellationToken));
    }
}
