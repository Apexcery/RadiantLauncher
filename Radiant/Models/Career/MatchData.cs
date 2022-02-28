using System.Collections.Generic;
using Newtonsoft.Json;

namespace Radiant.Models.Career
{
    public class MatchData
    {
        [JsonProperty("matchInfo")]
        public MatchInfo MatchInfo { get; set; }

        [JsonProperty("players")]
        public List<Player> Players { get; set; }

        [JsonProperty("bots")]
        public List<object> Bots { get; set; }

        [JsonProperty("coaches")]
        public List<object> Coaches { get; set; }

        [JsonProperty("teams")]
        public List<Team> Teams { get; set; }

        [JsonProperty("roundResults")]
        public List<RoundResult> RoundResults { get; set; }

        [JsonProperty("kills")]
        public List<MatchKill> Kills { get; set; }
    }
    public class MatchInfo
    {
        [JsonProperty("matchId")]
        public string MatchId { get; set; }

        [JsonProperty("mapId")]
        public string MapId { get; set; }

        [JsonProperty("gamePodId")]
        public string GamePodId { get; set; }

        [JsonProperty("gameLoopZone")]
        public string GameLoopZone { get; set; }

        [JsonProperty("gameServerAddress")]
        public string GameServerAddress { get; set; }

        [JsonProperty("gameVersion")]
        public string GameVersion { get; set; }

        [JsonProperty("gameLengthMillis")]
        public int GameLengthMillis { get; set; }

        [JsonProperty("gameStartMillis")]
        public long GameStartMillis { get; set; }

        [JsonProperty("provisioningFlowID")]
        public string ProvisioningFlowID { get; set; }

        [JsonProperty("isCompleted")]
        public bool IsCompleted { get; set; }

        [JsonProperty("customGameName")]
        public string CustomGameName { get; set; }

        [JsonProperty("forcePostProcessing")]
        public bool ForcePostProcessing { get; set; }

        [JsonProperty("queueID")]
        public string QueueID { get; set; }

        [JsonProperty("gameMode")]
        public string GameMode { get; set; }

        [JsonProperty("isRanked")]
        public bool IsRanked { get; set; }

        [JsonProperty("isMatchSampled")]
        public bool IsMatchSampled { get; set; }

        [JsonProperty("seasonId")]
        public string SeasonId { get; set; }

        [JsonProperty("completionState")]
        public string CompletionState { get; set; }

        [JsonProperty("platformType")]
        public string PlatformType { get; set; }
    }

    public class Player
    {
        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("gameName")]
        public string GameName { get; set; }

        [JsonProperty("tagLine")]
        public string TagLine { get; set; }

        [JsonProperty("platformInfo")]
        public PlatformInfo PlatformInfo { get; set; }

        [JsonProperty("teamId")]
        public string TeamId { get; set; }

        [JsonProperty("partyId")]
        public string PartyId { get; set; }

        [JsonProperty("characterId")]
        public string CharacterId { get; set; }

        [JsonProperty("stats")]
        public Stats Stats { get; set; }

        [JsonProperty("roundDamage")]
        public List<RoundDamage> RoundDamage { get; set; }

        [JsonProperty("competitiveTier")]
        public int CompetitiveTier { get; set; }

        [JsonProperty("playerCard")]
        public string PlayerCard { get; set; }

        [JsonProperty("playerTitle")]
        public string PlayerTitle { get; set; }

        [JsonProperty("preferredLevelBorder")]
        public string PreferredLevelBorder { get; set; }

        [JsonProperty("accountLevel")]
        public int AccountLevel { get; set; }

        [JsonProperty("sessionPlaytimeMinutes")]
        public int SessionPlaytimeMinutes { get; set; }

        [JsonProperty("xpModifications")]
        public List<XpModification> XpModifications { get; set; }

        [JsonProperty("behaviorFactors")]
        public BehaviorFactors BehaviorFactors { get; set; }

        [JsonProperty("newPlayerExperienceDetails")]
        public NewPlayerExperienceDetails NewPlayerExperienceDetails { get; set; }
    }

    public class PlatformInfo
    {
        [JsonProperty("platformType")]
        public string PlatformType { get; set; }

        [JsonProperty("platformOS")]
        public string PlatformOS { get; set; }

        [JsonProperty("platformOSVersion")]
        public string PlatformOSVersion { get; set; }

        [JsonProperty("platformChipset")]
        public string PlatformChipset { get; set; }
    }

    public class Stats
    {
        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("roundsPlayed")]
        public int RoundsPlayed { get; set; }

        [JsonProperty("kills")]
        public double Kills { get; set; }

        [JsonProperty("deaths")]
        public double Deaths { get; set; }

        [JsonProperty("assists")]
        public double Assists { get; set; }

        [JsonProperty("playtimeMillis")]
        public int PlaytimeMillis { get; set; }

        [JsonProperty("abilityCasts")]
        public AbilityCasts AbilityCasts { get; set; }
    }

    public class AbilityCasts
    {
        [JsonProperty("grenadeCasts")]
        public int GrenadeCasts { get; set; }

        [JsonProperty("ability1Casts")]
        public int Ability1Casts { get; set; }

        [JsonProperty("ability2Casts")]
        public int Ability2Casts { get; set; }

        [JsonProperty("ultimateCasts")]
        public int UltimateCasts { get; set; }
    }

    public class RoundDamage
    {
        [JsonProperty("round")]
        public int Round { get; set; }

        [JsonProperty("receiver")]
        public string Receiver { get; set; }

        [JsonProperty("damage")]
        public int Damage { get; set; }
    }

    public class XpModification
    {
        [JsonProperty("Value")]
        public double Value { get; set; }

        [JsonProperty("ID")]
        public string ID { get; set; }
    }

    public class BehaviorFactors
    {
        [JsonProperty("afkRounds")]
        public double AfkRounds { get; set; }

        [JsonProperty("friendlyFireIncoming")]
        public double FriendlyFireIncoming { get; set; }

        [JsonProperty("friendlyFireOutgoing")]
        public double FriendlyFireOutgoing { get; set; }

        [JsonProperty("stayedInSpawnRounds")]
        public double StayedInSpawnRounds { get; set; }
    }

    public class NewPlayerExperienceDetails
    {
        [JsonProperty("basicMovement")]
        public BasicMovement BasicMovement { get; set; }

        [JsonProperty("basicGunSkill")]
        public BasicGunSkill BasicGunSkill { get; set; }

        [JsonProperty("adaptiveBots")]
        public AdaptiveBots AdaptiveBots { get; set; }

        [JsonProperty("ability")]
        public Ability Ability { get; set; }

        [JsonProperty("bombPlant")]
        public BombPlant BombPlant { get; set; }

        [JsonProperty("defendBombSite")]
        public DefendBombSite DefendBombSite { get; set; }

        [JsonProperty("settingStatus")]
        public SettingStatus SettingStatus { get; set; }
    }

    public class BasicMovement
    {
        [JsonProperty("idleTimeMillis")]
        public int IdleTimeMillis { get; set; }

        [JsonProperty("objectiveCompleteTimeMillis")]
        public int ObjectiveCompleteTimeMillis { get; set; }
    }

    public class BasicGunSkill
    {
        [JsonProperty("idleTimeMillis")]
        public int IdleTimeMillis { get; set; }

        [JsonProperty("objectiveCompleteTimeMillis")]
        public int ObjectiveCompleteTimeMillis { get; set; }
    }

    public class AdaptiveBots
    {
        [JsonProperty("idleTimeMillis")]
        public int IdleTimeMillis { get; set; }

        [JsonProperty("objectiveCompleteTimeMillis")]
        public int ObjectiveCompleteTimeMillis { get; set; }

        [JsonProperty("adaptiveBotAverageDurationMillisAllAttempts")]
        public int AdaptiveBotAverageDurationMillisAllAttempts { get; set; }

        [JsonProperty("adaptiveBotAverageDurationMillisFirstAttempt")]
        public int AdaptiveBotAverageDurationMillisFirstAttempt { get; set; }

        [JsonProperty("killDetailsFirstAttempt")]
        public object KillDetailsFirstAttempt { get; set; }
    }

    public class Ability
    {
        [JsonProperty("idleTimeMillis")]
        public int IdleTimeMillis { get; set; }

        [JsonProperty("objectiveCompleteTimeMillis")]
        public int ObjectiveCompleteTimeMillis { get; set; }
    }

    public class BombPlant
    {
        [JsonProperty("idleTimeMillis")]
        public int IdleTimeMillis { get; set; }

        [JsonProperty("objectiveCompleteTimeMillis")]
        public int ObjectiveCompleteTimeMillis { get; set; }
    }

    public class DefendBombSite
    {
        [JsonProperty("idleTimeMillis")]
        public int IdleTimeMillis { get; set; }

        [JsonProperty("objectiveCompleteTimeMillis")]
        public int ObjectiveCompleteTimeMillis { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }

    public class SettingStatus
    {
        [JsonProperty("isMouseSensitivityDefault")]
        public bool IsMouseSensitivityDefault { get; set; }

        [JsonProperty("isCrosshairDefault")]
        public bool IsCrosshairDefault { get; set; }
    }

    public class Team
    {
        [JsonProperty("teamId")]
        public string TeamId { get; set; }

        [JsonProperty("won")]
        public bool Won { get; set; }

        [JsonProperty("roundsPlayed")]
        public int RoundsPlayed { get; set; }

        [JsonProperty("roundsWon")]
        public int RoundsWon { get; set; }

        [JsonProperty("numPoints")]
        public int NumPoints { get; set; }
    }

    public class RoundResult
    {
        [JsonProperty("roundNum")]
        public int RoundNum { get; set; }

        [JsonProperty("roundResult")]
        public string RoundResultAsString { get; set; }

        [JsonProperty("roundCeremony")]
        public string RoundCeremony { get; set; }

        [JsonProperty("winningTeam")]
        public string WinningTeam { get; set; }

        [JsonProperty("plantRoundTime")]
        public long PlantRoundTime { get; set; }

        [JsonProperty("plantPlayerLocations")]
        public object PlantPlayerLocations { get; set; }

        [JsonProperty("plantLocation")]
        public PlantLocation PlantLocation { get; set; }

        [JsonProperty("plantSite")]
        public string PlantSite { get; set; }

        [JsonProperty("defuseRoundTime")]
        public long DefuseRoundTime { get; set; }

        [JsonProperty("defusePlayerLocations")]
        public object DefusePlayerLocations { get; set; }

        [JsonProperty("defuseLocation")]
        public DefuseLocation DefuseLocation { get; set; }

        [JsonProperty("playerStats")]
        public List<PlayerStat> PlayerStats { get; set; }

        [JsonProperty("roundResultCode")]
        public string RoundResultCode { get; set; }

        [JsonProperty("playerEconomies")]
        public List<PlayerEconomy> PlayerEconomies { get; set; }

        [JsonProperty("playerScores")]
        public List<PlayerScore> PlayerScores { get; set; }
    }

    public class PlantLocation
    {
        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }
    }

    public class DefuseLocation
    {
        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }
    }

    public class PlayerStat
    {
        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("kills")]
        public List<Kill> Kills { get; set; }

        [JsonProperty("damage")]
        public List<Damage> Damage { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("economy")]
        public Economy Economy { get; set; }

        [JsonProperty("ability")]
        public Ability1 Ability { get; set; }

        [JsonProperty("wasAfk")]
        public bool WasAfk { get; set; }

        [JsonProperty("wasPenalized")]
        public bool WasPenalized { get; set; }

        [JsonProperty("stayedInSpawn")]
        public bool StayedInSpawn { get; set; }
    }

    public class Kill
    {
        [JsonProperty("gameTime")]
        public long GameTime { get; set; }

        [JsonProperty("roundTime")]
        public long RoundTime { get; set; }

        [JsonProperty("killer")]
        public string Killer { get; set; }

        [JsonProperty("victim")]
        public string Victim { get; set; }

        [JsonProperty("victimLocation")]
        public VictimLocation VictimLocation { get; set; }

        [JsonProperty("assistants")]
        public List<string> Assistants { get; set; }

        [JsonProperty("playerLocations")]
        public List<PlayerLocation> PlayerLocations { get; set; }

        [JsonProperty("finishingDamage")]
        public FinishingDamage FinishingDamage { get; set; }
    }

    public class VictimLocation
    {
        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }
    }

    public class PlayerLocation
    {
        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("viewRadians")]
        public double ViewRadians { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }
    }

    public class Location
    {
        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }
    }

    public class FinishingDamage
    {
        [JsonProperty("damageType")]
        public string DamageType { get; set; }

        [JsonProperty("damageItem")]
        public string DamageItem { get; set; }

        [JsonProperty("isSecondaryFireMode")]
        public bool IsSecondaryFireMode { get; set; }
    }

    public class Damage
    {
        [JsonProperty("receiver")]
        public string Receiver { get; set; }

        [JsonProperty("damage")]
        public double DamageValue { get; set; }

        [JsonProperty("legshots")]
        public int Legshots { get; set; }

        [JsonProperty("bodyshots")]
        public int Bodyshots { get; set; }

        [JsonProperty("headshots")]
        public int Headshots { get; set; }
    }

    public class Economy
    {
        [JsonProperty("loadoutValue")]
        public double LoadoutValue { get; set; }

        [JsonProperty("weapon")]
        public string Weapon { get; set; }

        [JsonProperty("armor")]
        public string Armor { get; set; }

        [JsonProperty("remaining")]
        public double Remaining { get; set; }

        [JsonProperty("spent")]
        public double Spent { get; set; }
    }

    public class Ability1
    {
        [JsonProperty("grenadeEffects")]
        public object GrenadeEffects { get; set; }

        [JsonProperty("ability1Effects")]
        public object Ability1Effects { get; set; }

        [JsonProperty("ability2Effects")]
        public object Ability2Effects { get; set; }

        [JsonProperty("ultimateEffects")]
        public object UltimateEffects { get; set; }
    }

    public class PlayerEconomy : Economy
    {
        [JsonProperty("subject")]
        public string Subject { get; set; }
    }

    public class PlayerScore
    {
        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }
    }

    public class MatchKill : Kill
    {
        [JsonProperty("round")]
        public int Round { get; set; }
    }
}
