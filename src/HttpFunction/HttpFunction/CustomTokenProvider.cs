﻿using Microsoft.Azure.ServiceBus.Primitives;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HttpFunction
{
    public class CustomTokenProvider : TokenProvider
    {
        private readonly string _managedIdentityTenantId;

        public CustomTokenProvider(string managedIdentityTenantId = null)
        {
            if (string.IsNullOrEmpty(managedIdentityTenantId))
            {
                // Ensure tenant id is null if none given
                _managedIdentityTenantId = null;
            }
            else
            {
                _managedIdentityTenantId = managedIdentityTenantId;
            }
        }

        public override async Task<SecurityToken> GetTokenAsync(string appliesTo, TimeSpan timeout)
        {
            string accessToken = await GetAccessToken("https://servicebus.azure.net/");
            return new JsonSecurityToken(accessToken, appliesTo);
        }

        private async Task<string> GetAccessToken(string resource)
        {
            var authProvider = new AzureServiceTokenProvider();
            return await authProvider.GetAccessTokenAsync(resource, _managedIdentityTenantId);
        }
    }
}
