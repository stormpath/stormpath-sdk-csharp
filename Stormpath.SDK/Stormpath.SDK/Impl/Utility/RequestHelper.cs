﻿// <copyright file="RequestHelper.cs" company="Stormpath, Inc.">
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

using System.Net;

namespace Stormpath.SDK.Impl.Utility
{
    internal static class RequestHelper
    {
        public static string UrlEncode(string value, bool isPath = false, bool canonicalize = false)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var encoded = WebUtility.UrlEncode(value);

            if (canonicalize)
            {
                encoded = encoded
                    .Replace("%2B", "+")
                    .Replace("+", "%20")
                    .Replace("*", "%2A")
                    .Replace("%7E", "~");

                if (isPath)
                    encoded = encoded.Replace("%2F", "/");
            }

            return encoded;
        }
    }
}
