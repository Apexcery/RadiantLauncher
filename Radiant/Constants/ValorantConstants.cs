using System;
using System.Collections.Generic;

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

        public static readonly Dictionary<string, string> SeasonNameById = new(StringComparer.OrdinalIgnoreCase)
        {
            { "0df5adb9-4dcb-6899-1306-3e9860661dd3", "Closed Beta" },
            { "3f61c772-4560-cd3f-5d3f-a7ab5abda6b3", "Episode 1 Act 1" },
            { "0530b9c4-4980-f2ee-df5d-09864cd00542", "Episode 1 Act 2" },
            { "46ea6166-4573-1128-9cea-60a15640059b", "Episode 1 Act 3" },
            { "97b6e739-44cc-ffa7-49ad-398ba502ceb0", "Episode 2 Act 1" },
            { "ab57ef51-4e59-da91-cc8d-51a5a2b9b8ff", "Episode 2 Act 2" },
            { "52e9749a-429b-7060-99fe-4595426a0cf7", "Episode 2 Act 3" },
            { "2a27e5d2-4d30-c9e2-b15a-93b8909a442c", "Episode 3 Act 1" },
            { "4cb622e1-4244-6da3-7276-8daaf1c01be2", "Episode 3 Act 2" },
            { "a16955a5-4ad0-f761-5e9e-389df1c892fb", "Episode 3 Act 3" },
            { "573f53ac-41a5-3a7d-d9ce-d6a6298e5704", "Episode 4 Act 1" },
            { "d929bc38-4ab6-7da4-94f0-ee84f8ac141e", "Episode 4 Act 2" },
            { "3e47230a-463c-a301-eb7d-67bb60357d4f", "Episode 4 Act 3" }
        };

        public static readonly Dictionary<string, string> AgentNameById = new(StringComparer.OrdinalIgnoreCase)
        {
            { "5F8D3A7F-467B-97F3-062C-13ACF203C006", "Breach" },
            { "F94C3B30-42BE-E959-889C-5AA313DBA261", "Raze" },
            { "22697A3D-45BF-8DD7-4FEC-84A9E28C69D7", "Chamber" },
            { "601DBBE7-43CE-BE57-2A40-4ABD24953621", "KAY/O" },
            { "6F2A04CA-43E0-BE17-7F36-B3908627744D", "Skye" },
            { "117ED9E3-49F3-6512-3CCF-0CADA7E3823B", "Cypher" },
            { "DED3520F-4264-BFED-162D-B080E2ABCCF9", "Sova" },
            { "320B2A48-4D9B-A075-30F1-1F93A9B638FA", "Sova" },
            { "1E58DE9C-4950-5125-93E9-A0AEE9F98746", "Killjoy" },
            { "707EAB51-4836-F488-046A-CDA6BF494859", "Viper" },
            { "EB93336A-449B-9C1B-0A54-A891F7921D69", "Phoenix" },
            { "41FB69C1-4189-7B37-F117-BCAF1E96F1BF", "Astra" },
            { "9F0D8BA9-4140-B941-57D3-A7AD57C6B417", "Brimstone" },
            { "BB2A4828-46EB-8CD1-E765-15848195D751", "Neon" },
            { "7F94D92C-4234-0A36-9646-3A87EB8B5C89", "Yoru" },
            { "569FDD95-4D10-43AB-CA70-79BECC718B46", "Sage" },
            { "A3BFB853-43B2-7238-A4F1-AD90E9E46BCC", "Reyna" },
            { "8E253930-4C05-31DD-1B6C-968525494517", "Omen" },
            { "ADD6443A-41BD-E414-F6AD-E58D267F4E95", "Jett" }
        };

        public static readonly List<MapInfo> Maps = new()
        {
            new MapInfo
            {
                Name = "Ascent",
                Id = "7EAECC1B-4337-BBF6-6AB9-04B8F06B3319",
                AssetName = "Ascent",
                AssetPath = "/Game/Maps/Ascent/Ascent"
            },
            new MapInfo
            {
                Name = "Split",
                Id = "D960549E-485C-E861-8D71-AA9D1AED12A2",
                AssetName = "Bonsai",
                AssetPath = "/Game/Maps/Bonsai/Bonsai"
            },
            new MapInfo
            {
                Name = "Fracture",
                Id = "B529448B-4D60-346E-E89E-00A4C527A405",
                AssetName = "Canyon",
                AssetPath = "/Game/Maps/Canyon/Canyon"
            },
            new MapInfo
            {
                Name = "Bind",
                Id = "2C9D57EC-4431-9C5E-2939-8F9EF6DD5CBA",
                AssetName = "Duality",
                AssetPath = "/Game/Maps/Duality/Duality"
            },
            new MapInfo
            {
                Name = "Bind NPE",
                Id = "270D34BB-4614-3781-4712-A49759095F75",
                AssetName = "Duality_NPE",
                AssetPath = "/Game/Maps/Duality_NPE/Duality_NPE"
            },
            new MapInfo
            {
                Name = "Breeze",
                Id = "2FB9A4FD-47B8-4E7D-A969-74B4046EBD53",
                AssetName = "Foxtrot",
                AssetPath = "/Game/Maps/Foxtrot/Foxtrot"
            },
            new MapInfo
            {
                Name = "Icebox",
                Id = "E2AD5C54-4114-A870-9641-8EA21279579A",
                AssetName = "Port",
                AssetPath = "/Game/Maps/Port/Port"
            },
            new MapInfo
            {
                Name = "The Range",
                Id = "EE613EE9-28B7-4BEB-9666-08DB13BB2244",
                AssetName = "Range",
                AssetPath = "/Game/Maps/Poveglia/Range"
            },
            new MapInfo
            {
                Name = "Haven",
                Id = "2BEE0DC9-4FFE-519B-1CBD-7FBE763A6047",
                AssetName = "Triad",
                AssetPath = "/Game/Maps/Triad/Triad"
            }
        };

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

        public class MapInfo
        {
            public string Name { get; set; }
            public string Id { get; set; }
            public string AssetName { get; set; }
            public string AssetPath { get; set; }
        }

        public class GameModeInfo
        {
            public string Name { get; set; }
            public string Id { get; set; }
            public string AssetName { get; set; }
            public string AssetPath { get; set; }
        }
    }
}
