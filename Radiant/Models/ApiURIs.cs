using System;
using System.Collections.Generic;

namespace Radiant.Models
{
    public static class ApiURIs
    {
        public static readonly Dictionary<string, Uri> URIs = new()
        {
            {
                "AuthUri",
                new Uri("https://auth.riotgames.com/api/v1/authorization")
            },
            {
                "EntitlementUri",
                new Uri("https://entitlements.auth.riotgames.com/api/token/v1")
            },
            {
                "UserInfoUri",
                new Uri("https://auth.riotgames.com/userinfo")
            },
            {
                "UserRegionUri",
                new Uri("https://riot-geo.pas.si.riotgames.com/pas/v1/product/valorant")
            },
            {
                "SkinUri",
                new Uri("https://valorant-api.com/v1/weapons/skins")
            },
            {
                "AgentUri",
                new Uri("https://valorant-api.com/v1/agents?isPlayableCharacter=true")
            },
            {
                "SeasonUri",
                new Uri("https://valorant-api.com/v1/seasons")
            },
            {
                "MapUri",
                new Uri("https://valorant-api.com/v1/maps")
            },
            {
                "BundleUri",
                new Uri("https://valorant-api.com/v1/bundles")
            },
            {
                "CurrencyUri",
                new Uri("https://valorant-api.com/v1/currencies")
            },
            {
                "ClientVersionUri",
                new Uri("https://valorant-api.com/v1/version")
            }
        };
    }
}
