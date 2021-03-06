﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pomelo.AspNetCore.Extensions.BlobStorage;

namespace Pomelo.AspNetCore.Extensions.BlobStorage
{
    public class SignedUserDownloadAuthorizationProvider<TKey> : IBlobAccessAuthorizationProvider<TKey>
        where TKey : IEquatable<TKey>
    {
        protected HttpContext httpContext { get; set; }

        public SignedUserDownloadAuthorizationProvider(IHttpContextAccessor accessor)
        {
            httpContext = accessor.HttpContext;
        }

        public bool IsAbleToDownload(TKey BlobId)
        {
            return httpContext.User.Identities.Count() > 0;
        }
    }
}

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SignedUserAccessAuthorizationProviderServiceCollectionExtensions
    {
        public static IBlobStorageBuilder AddSignedUserDownloadAuthorization<TKey>(this IBlobStorageBuilder self)
            where TKey : IEquatable<TKey>
        {
            self.Services.AddSingleton<IBlobAccessAuthorizationProvider<TKey>, SignedUserDownloadAuthorizationProvider<TKey>>();
            return self;
        }

        public static IBlobStorageBuilder AddSignedUserDownloadAuthorization(this IBlobStorageBuilder self)
        {
            self.Services.AddSingleton<IBlobAccessAuthorizationProvider<Guid>, SignedUserDownloadAuthorizationProvider<Guid>>();
            return self;
        }
    }
}