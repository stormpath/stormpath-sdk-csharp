﻿// <copyright file="PasswordResetToken_tests.cs" company="Stormpath, Inc.">
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

using Shouldly;
using Stormpath.SDK.Account;
using Stormpath.SDK.Tests.Common;
using Xunit;

namespace Stormpath.SDK.Tests
{
    public class PasswordResetToken_tests
    {
        [Fact]
        public void GetValue_returns_token_value()
        {
            var dataStore = TestDataStore.Create();

            var href = "https://api.foobar.com/v1/applications/WpM9nyZ2TbaEzfbRvLk9KA/passwordResetTokens/my-token-value-here";

            var passwordResetToken = dataStore.InstantiateWithHref<IPasswordResetToken>(href);

            passwordResetToken.GetValue().ShouldBe("my-token-value-here");
        }
    }
}
