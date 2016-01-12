﻿// <copyright file="DefaultSynchronousFilter.cs" company="Stormpath, Inc.">
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
using Stormpath.SDK.Logging;

namespace Stormpath.SDK.Impl.DataStore.Filters
{
    internal sealed class DefaultSynchronousFilter : ISynchronousFilter
    {
        private readonly Func<IResourceDataRequest, ISynchronousFilterChain, ILogger, IResourceDataResult> filterFunc;

        public DefaultSynchronousFilter(Func<IResourceDataRequest, ISynchronousFilterChain, ILogger, IResourceDataResult> filterFunc)
        {
            this.filterFunc = filterFunc;
        }

        IResourceDataResult ISynchronousFilter.Filter(IResourceDataRequest request, ISynchronousFilterChain chain, ILogger logger)
        {
            return this.filterFunc(request, chain, logger);
        }
    }
}
