﻿// <copyright file="Sync_unsupported_methods_tests.cs" company="Stormpath, Inc.">
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
using System.Linq;
using NSubstitute;
using Shouldly;
using Stormpath.SDK.Account;
using Stormpath.SDK.Sync;
using Stormpath.SDK.Tests.Helpers;
using Xunit;

namespace Stormpath.SDK.Tests.Impl.Linq
{
    public class Sync_unsupported_methods_tests : Linq_tests
    {
        [Fact]
        public void Aggregate_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                this.Harness.Queryable.Synchronously().Aggregate((x, y) => x);
            });
        }

        [Fact]
        public void All_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                this.Harness.Queryable.Synchronously().All(x => x.Email == "foo");
            });
        }

        [Fact]
        public void Average_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                this.Harness.Queryable.Synchronously().Average(x => 1.0);
            });
        }

        [Fact]
        public void Cast_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable.Synchronously().Cast<Tenant.ITenant>();
                query.GeneratedSynchronousArgumentsWere(this.Href, "<not evaluated>");
            });
        }

        [Fact]
        public void Concat_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable.Synchronously().Concat(Enumerable.Empty<IAccount>());
                query.GeneratedSynchronousArgumentsWere(this.Href, "<not evaluated>");
            });
        }

        [Fact]
        public void Contains_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                this.Harness.Queryable.Synchronously().Contains(Substitute.For<IAccount>());
            });
        }

        [Fact]
        public void Distinct_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable.Synchronously().Distinct();
                query.GeneratedSynchronousArgumentsWere(this.Href, "<not evaluated>");
            });
        }

        [Fact]
        public void Except_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable.Synchronously().Except(Enumerable.Empty<IAccount>());
                query.GeneratedSynchronousArgumentsWere(this.Href, "<not evaluated>");
            });
        }

        [Fact]
        public void GroupBy_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable.Synchronously().GroupBy(x => x.Email);
                query.GeneratedSynchronousArgumentsWere(this.Href, "<not evaluated>");
            });
        }

        [Fact]
        public void GroupJoin_clause_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable.Synchronously().GroupJoin(
                    Enumerable.Empty<IAccount>(),
                    outer => outer.Email,
                    inner => inner.Username,
                    (outer, results) => new { outer.CreatedAt, results });
                query.GeneratedSynchronousArgumentsWere(this.Href, "<not evaluated>");
            });
        }

        [Fact]
        public void Intersect_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable.Synchronously().Intersect(Enumerable.Empty<IAccount>());
                query.GeneratedSynchronousArgumentsWere(this.Href, "<not evaluated>");
            });
        }

        [Fact]
        public void Join_clause_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable.Synchronously().Join(
                    Enumerable.Empty<IAccount>(),
                    outer => outer.Email,
                    inner => inner.Username,
                    (outer, inner) => outer.CreatedAt);
                query.GeneratedSynchronousArgumentsWere(this.Href, "<not evaluated>");
            });
        }

        [Fact]
        public void Last_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                this.Harness.Queryable.Synchronously().Last();
            });

            Should.Throw<NotSupportedException>(() =>
            {
                this.Harness.Queryable.Synchronously().LastOrDefault();
            });
        }

        [Fact]
        public void Max_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                this.Harness.Queryable.Synchronously().Max();
            });
        }

        [Fact]
        public void Min_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                this.Harness.Queryable.Synchronously().Min();
            });
        }

        [Fact]
        public void OfType_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable.Synchronously().OfType<IAccount>();
                query.GeneratedSynchronousArgumentsWere(this.Href, "<not evaluated>");
            });
        }

        [Fact]
        public void OrderBy_throws_for_complex_overloads()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                this.Harness.Queryable
                    .Synchronously()
                    .OrderBy(x => x.GivenName, StringComparer.OrdinalIgnoreCase).ToList();
            });
        }

        [Fact]
        public void ThenBy_throws_for_complex_overloads()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                this.Harness.Queryable
                    .Synchronously()
                    .OrderBy(x => x.GivenName)
                    .ThenBy(x => x.Surname, StringComparer.OrdinalIgnoreCase)
                    .ToList();
            });
        }

        [Fact]
        public void Reverse_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable.Synchronously().Reverse();
                query.GeneratedSynchronousArgumentsWere(this.Href, "<not evaluated>");
            });
        }

        [Fact]
        public void Select_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable
                    .Synchronously()
                    .Select(x => x.Email);
                query.GeneratedSynchronousArgumentsWere(this.Href, "<not evaluated>");
            });
        }

        [Fact]
        public void SelectMany_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable.Synchronously().SelectMany(x => x.Email);
                query.GeneratedSynchronousArgumentsWere(this.Href, "<not evaluated>");
            });
        }

        [Fact]
        public void SequenceEqual_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                this.Harness.Queryable.Synchronously().SequenceEqual(Enumerable.Empty<IAccount>());
            });
        }

        [Fact]
        public void SkipWhile_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable.Synchronously().SkipWhile(x => x.Email == "foobar");
                query.GeneratedSynchronousArgumentsWere(this.Href, "<not evaluated>");
            });
        }

        [Fact]
        public void Sum_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                this.Harness.Queryable.Synchronously().Sum(x => 1);
            });
        }

        [Fact]
        public void TakeWhile_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable.Synchronously().TakeWhile(x => x.Email == "foobar");
                query.GeneratedSynchronousArgumentsWere(this.Href, "<not evaluated>");
            });
        }

        [Fact]
        public void Union_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable.Synchronously().Union(Enumerable.Empty<IAccount>());
                query.GeneratedSynchronousArgumentsWere(this.Href, "<not evaluated>");
            });
        }

        [Fact]
        public void Zip_is_unsupported()
        {
            Should.Throw<NotSupportedException>(() =>
            {
                var query = this.Harness.Queryable.Synchronously().Zip(
                    Enumerable.Empty<IAccount>(),
                    (first, second) => first.Email == second.Email);
                query.GeneratedSynchronousArgumentsWere(this.Href, "<not evaluated>");
            });
        }
    }
}
