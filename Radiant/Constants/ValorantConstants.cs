using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Radiant.Extensions;
using Radiant.Models;
using Radiant.Models.Valorant;

namespace Radiant.Constants
{
    public static class ValorantConstants
    {
        public static readonly Dictionary<int, string> RankNameByTier = new()
        {
            { 0, "Unranked" },
            { 3, "Iron 1" },
            { 4, "Iron 2" },
            { 5, "Iron 3" },
            { 6, "Bronze 1" },
            { 7, "Bronze 2" },
            { 8, "Bronze 3" },
            { 9, "Silver 1" },
            { 10, "Silver 2" },
            { 11, "Silver 3" },
            { 12, "Gold 1" },
            { 13, "Gold 2" },
            { 14, "Gold 3" },
            { 15, "Platinum 1" },
            { 16, "Platinum 2" },
            { 17, "Platinum 3" },
            { 18, "Diamond 1" },
            { 19, "Diamond 2" },
            { 20, "Diamond 3" },
            { 21, "Immortal 1" },
            { 22, "Immortal 2" },
            { 23, "Immortal 3" },
            { 24, "Radiant" }
        };

        public static readonly Dictionary<string, Agents.Agent> AgentById = new(StringComparer.InvariantCultureIgnoreCase);

        public static readonly Dictionary<string, Seasons.Season> SeasonById = new(StringComparer.InvariantCultureIgnoreCase);

        public static readonly Dictionary<string, Maps.Map> MapById = new(StringComparer.InvariantCultureIgnoreCase);
        public static readonly Dictionary<string, Maps.Map> MapByUrl = new(StringComparer.InvariantCultureIgnoreCase);
        public static readonly List<Maps.Map> Maps = new();

        public static readonly Dictionary<string, Bundles.Bundle> BundleById = new(StringComparer.InvariantCultureIgnoreCase);
        
        public static readonly Dictionary<string, Currencies.Currency> CurrencyById = new(StringComparer.InvariantCultureIgnoreCase);
        public static readonly Dictionary<string, Currencies.Currency> CurrencyByName = new(StringComparer.InvariantCultureIgnoreCase);

        public static readonly List<Skins.Skin> Skins = new();

        private static DateTime? _lastInit;

        public static async Task Init()
        {
            if (_lastInit.HasValue && _lastInit.Value.AddMinutes(5) > DateTime.UtcNow) // If initialized in the last 5 minutes, skip initialization
                return;

            _lastInit = DateTime.UtcNow;

            var cancellationTokenSource = new CancellationTokenSource();
            var httpClient = new HttpClient();

            var agents = await GetValorantData<Agents>(httpClient, ApiURIs.URIs["AgentUri"], cancellationTokenSource.Token);
            foreach (var agent in agents.AgentData)
            {
                var agentId = agent.Id;
                if (AgentById.ContainsKey(agentId))
                    AgentById[agentId] = agent;
                else
                    AgentById.Add(agentId, agent);
            }

            var seasons = await GetValorantData<Seasons>(httpClient, ApiURIs.URIs["SeasonUri"], cancellationTokenSource.Token);
            foreach (var season in seasons.SeasonData)
            {
                var seasonId = season.Id;
                if (SeasonById.ContainsKey(seasonId))
                    SeasonById[seasonId] = season;
                else
                    SeasonById.Add(seasonId, season);
            }

            var maps = await GetValorantData<Maps>(httpClient, ApiURIs.URIs["MapUri"], cancellationTokenSource.Token);
            foreach (var map in maps.MapData)
            {
                var mapId = map.Id;
                var mapUrl = map.MapUrl;
                
                if (MapById.ContainsKey(mapId))
                    MapById[mapId] = map;
                else
                    MapById.Add(mapId, map);
                
                if (MapByUrl.ContainsKey(mapUrl))
                    MapByUrl[mapUrl] = map;
                else
                    MapByUrl.Add(mapUrl, map);

                if (!Maps.Contains(map))
                    Maps.Add(map);
            }

            var bundles = await GetValorantData<Bundles>(httpClient, ApiURIs.URIs["BundleUri"], cancellationTokenSource.Token);
            foreach (var bundle in bundles.BundleData)
            {
                var bundleId = bundle.Id;
                if (BundleById.ContainsKey(bundleId))
                    BundleById[bundleId] = bundle;
                else
                    BundleById.Add(bundleId, bundle);
            }
            
            var currencies = await GetValorantData<Currencies>(httpClient, ApiURIs.URIs["CurrencyUri"], cancellationTokenSource.Token);
            foreach (var currency in currencies.CurrencyData)
            {
                var currencyId = currency.Id;
                var currencyName = currency.DisplayName;
                
                if (CurrencyById.ContainsKey(currencyId))
                    CurrencyById[currencyId] = currency;
                else
                    CurrencyById.Add(currencyId, currency);

                if (CurrencyByName.ContainsKey(currencyName))
                    CurrencyByName[currencyName] = currency;
                else
                    CurrencyByName.Add(currencyName, currency);
            }

            var skins = await GetValorantData<Skins>(httpClient, ApiURIs.URIs["SkinUri"], cancellationTokenSource.Token);
            foreach (var skin in skins.SkinData)
            {
                if (!Skins.Contains(skin))
                    Skins.Add(skin);
            }
        }

        private static async Task<T> GetValorantData<T>(HttpClient client, Uri uri, CancellationToken cancellationToken)
        {
            var response = await client.GetAsync(uri, cancellationToken);
            if (!response.IsSuccessStatusCode)
                return default;

            var data = await response.Content.ReadAsJsonAsync<T>(cancellationToken);
            return data;
        }
        
        public static readonly List<GameModeInfo> GameModes = new()
        {
            new GameModeInfo
            {
                Name = "Standard",
                Id = "96BD3920-4F36-D026-2B28-C683EB0BCAC5",
                AssetName = "BombGameMode",
                AssetPath = "/Game/GameModes/Bomb/BombGameMode.BombGameMode_C"
            },
            new GameModeInfo
            {
                Name = "Deathmatch",
                Id = "A8790EC5-4237-F2F0-E93B-08A8E89865B2",
                AssetName = "DeathmatchGameMode",
                AssetPath = "/Game/GameModes/Deathmatch/DeathmatchGameMode.DeathmatchGameMode_C"
            },
            new GameModeInfo
            {
                Name = "Escalation",
                Id = "A4ED6518-4741-6DCB-35BD-F884AECDC859",
                AssetName = "GunGameTeamsGameMode",
                AssetPath = "/Game/GameModes/GunGame/GunGameTeamsGameMode.GunGameTeamsGameMode_C"
            },
            new GameModeInfo
            {
                Name = "TDM Mobile Dev",
                Id = "BC752E29-426A-8F70-817F-6CB826A3F54A",
                AssetName = "TDM_Prototype_Gamemode",
                AssetPath = "/Game/GameModes/MikeProto/TDM_Prototype_Gamemode.TDM_Prototype_Gamemode_C"
            },
            new GameModeInfo
            {
                Name = "Onboarding",
                Id = "D2B4E425-4CAB-8D95-EB26-BB9B444551DC",
                AssetName = "NPEGameMode",
                AssetPath = "/Game/GameModes/NewPlayerExperience/NPEGameMode.NPEGameMode_C"
            },
            new GameModeInfo
            {
                Name = "Bots NPE",
                Id = "2E4C781B-4B7F-E30D-36CA-2CAA6D0D7279",
                AssetName = "NPE_BotGameMode",
                AssetPath = "/Game/GameModes/NewPlayerExperience/NPE_BotMode/NPE_BotGameMode.NPE_BotGameMode_C"
            },
            new GameModeInfo
            {
                Name = "Replication",
                Id = "4744698A-4513-DC96-9C22-A9AA437E4A58",
                AssetName = "OneForAll_GameMode",
                AssetPath = "/Game/GameModes/OneForAll/OneForAll_GameMode.OneForAll_GameMode_C"
            },
            new GameModeInfo
            {
                Name = "Spike Rush",
                Id = "E921D1E6-416B-C31F-1291-74930C330B7B",
                AssetName = "QuickBombGameMode",
                AssetPath = "/Game/GameModes/QuickBomb/QuickBombGameMode.QuickBombGameMode_C"
            },
            new GameModeInfo
            {
                Name = "PRACTICE",
                Id = "E2DC3878-4FE5-D132-28F8-3D8C259EFCC6",
                AssetName = "ShootingRangeGameMode",
                AssetPath = "/Game/GameModes/ShootingRange/ShootingRangeGameMode.ShootingRangeGameMode_C"
            },
            new GameModeInfo
            {
                Name = "Snowball Fight",
                Id = "57038D6D-49B1-3A74-C5EF-3395D9F23A97",
                AssetName = "SnowballFightGameMode",
                AssetPath = "/Game/GameModes/SnowballFight/SnowballFightGameMode.SnowballFightGameMode_C"
            },
            new GameModeInfo
            {
                Name = "SwiftPlay",
                Id = "5D0F264B-4EBE-CC63-C147-809E1374484B",
                AssetName = "SwiftPlay_GameMode",
                AssetPath = "/Game/GameModes/SwiftPlay/SwiftPlay_GameMode.SwiftPlay_GameMode_C"
            },
            new GameModeInfo
            {
                Name = "SwiftPlay Carryover",
                Id = "8403E16F-41DE-666E-6330-43A2A24140C5",
                AssetName = "SwiftPlayCarryover_GameMode",
                AssetPath = "/Game/GameModes/SwiftPlay/CarryOver/SwiftPlayCarryover_GameMode.SwiftPlayCarryover_GameMode_C"
            },
            new GameModeInfo
            {
                Name = "SwiftPlay Pips",
                Id = "10E4B36F-4F4A-DBEB-375F-D7AB71E6B75C",
                AssetName = "SwiftPlayPips_GameMode",
                AssetPath = "/Game/GameModes/SwiftPlay/Pips/SwiftPlayPips_GameMode.SwiftPlayPips_GameMode_C"
            }
        };
        
        public class GameModeInfo
        {
            public string Name { get; set; }
            public string Id { get; set; }
            public string AssetName { get; set; }
            public string AssetPath { get; set; }
        }
    }
}
