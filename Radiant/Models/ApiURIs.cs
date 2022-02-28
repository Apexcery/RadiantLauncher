using System;
using System.Collections.Generic;

namespace ValorantLauncher.Models
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
                "ClientVersionUri",
                new Uri("https://valorant-api.com/v1/version")
            },
            {
                "SkinUri",
                new Uri("https://assist.rumblemike.com/Skins")
            },
            {
                "OfferUri",
                new Uri("https://assist.rumblemike.com/Offers")
            },
            {
                "BundleUri",
                new Uri("https://assist.rumblemike.com/Bundle")
            }
        };
    }
}
