using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable InconsistentNaming

namespace PalMan.Shared.Models.PalWorld;

[SuppressMessage("Performance", "CA1805:Do not initialize unnecessarily")]
[SuppressMessage("Style", "IDE1006:Naming Styles")]
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class ServerArguments
{
    public ServerArguments()
    {
    }

    protected ServerArguments(ServerArguments arguments)
    {
        var properties = typeof(ServerArguments).GetProperties();
        foreach (var property in properties)
        {
            var value = property.GetValue(arguments);
            property.SetValue(this, value);
        }
    }

    [Column("Difficulty")]
    [Description("Difficulty")]
    public string Difficulty { get; set; } = "None";

    [Column("DayTimeSpeedRate")]
    [Description("Day time speed")]
    public float DayTimeSpeedRate { get; set; } = 1.000000f;

    [Column("NightTimeSpeedRate")]
    [Description("Night time speed")]
    public float NightTimeSpeedRate { get; set; } = 1.000000f;

    [Column("ExpRate")]
    [Description("EXP rate")]
    public float ExpRate { get; set; } = 1.000000f;

    [Column("PalCaptureRate")]
    [Description("Pal capture rate")]
    public float PalCaptureRate { get; set; } = 1.000000f;

    [Column("PalSpawnNumRate")]
    [Description("Pal appearance rate")]
    public float PalSpawnNumRate { get; set; } = 1.000000f;

    [Column("PalDamageRateAttack")]
    [Description("Damage from pals multipiler")]
    public float PalDamageRateAttack { get; set; } = 1.000000f;

    [Column("PalDamageRateDefense")]
    [Description("Damage to pals multipiler")]
    public float PalDamageRateDefense { get; set; } = 1.000000f;

    [Column("PlayerDamageRateAttack")]
    [Description("Damage from player multipiler")]
    public float PlayerDamageRateAttack { get; set; } = 1.000000f;

    [Column("PlayerDamageRateDefense")]
    [Description("Damage to player multipiler")]
    public float PlayerDamageRateDefense { get; set; } = 1.000000f;

    [Column("PlayerStomachDecreaseRate")]
    [Description("Player hunger depletion rate")]
    public float PlayerStomachDecreaceRate { get; set; } = 1.000000f;

    [Column("PlayerStaminaDecreaseRate")]
    [Description("Player stamina depletion rate")]
    public float PlayerStaminaDecreaceRate { get; set; } = 1.000000f;

    [Column("PlayerAutoHPRegenerationRate")]
    [Description("Player auto HP regeneration rate")]
    public float PlayerAutoHPRegeneRate { get; set; } = 1.000000f;

    [Column("PlayerAutoHPRegenerationRateInSleep")]
    [Description("Player auto HP regeneration rate in sleep")]
    public float PlayerAutoHpRegeneRateInSleep { get; set; } = 1.000000f;

    [Column("PalStomachDecreaseRate")]
    [Description("Pal hunger depletion rate")]
    public float PalStomachDecreaceRate { get; set; } = 1.000000f;

    [Column("PalStaminaDecreaseRate")]
    [Description("Pal stamina reduction rate")]
    public float PalStaminaDecreaceRate { get; set; } = 1.000000f;

    [Column("PalAutoHPRegenerationRate")]
    [Description("Pal auto HP regeneration rate")]
    public float PalAutoHPRegeneRate { get; set; } = 1.000000f;

    [Column("PalAutoHPRegenerationRateInSleep")]
    [Description("Pal sleep health regeneration rate (in Palbox)")]
    public float PalAutoHpRegeneRateInSleep { get; set; } = 1.000000f;

    [Column("BuildObjectDamageRate")]
    [Description("Damage to structure multipiler")]
    public float BuildObjectDamageRate { get; set; } = 1.000000f;

    [Column("BuildObjectDeteriorationDamageRate")]
    [Description("Structure determination rate")]
    public float BuildObjectDeteriorationDamageRate { get; set; } = 1.000000f;

    [Column("CollectionDropRate")]
    [Description("Getherable items multipiler")]
    public float CollectionDropRate { get; set; } = 1.000000f;

    [Column("CollectionObjectHPRate")]
    [Description("Getherable objects HP multipiler")]
    public float CollectionObjectHpRate { get; set; } = 1.000000f;

    [Column("CollectionObjectRespawnSpeedRate")]
    [Description("Getherable objects respawn interval")]
    public float CollectionObjectRespawnSpeedRate { get; set; } = 1.000000f;

    [Column("EnemyDropItemRate")]
    [Description("Dropped Items Multipiler")]
    public float EnemyDropItemRate { get; set; } = 1.000000f;

    [Column("DeathPenalty")]
    [Description("None: No lost; Item: Lost item without equipment; ItemAndEquipment: Lost item and equipment; All: Lost All item, equipment, pal(in inventory)")]
    public string DeathPenalty { get; set; } = "All";

    [Column("GuildPlayerMaxNum")]
    [Description("Max player of Guild")]
    public int GuildPlayerMaxNum { get; set; } = 20;

    [Column("PalEggDefaultHatchingTime")]
    [Description("Time(h) to incubate massive egg")]
    public float PalEggDefaultHatchingTime { get; set; } = 72.000000f;

    [Column("ServerPlayerMaxNum")]
    [Description("Maximum number of people who can join the server")]
    public int ServerPlayerMaxNum { get; set; } = 32;

    [Column("ServerName")]
    [Description("Server name")]
    public string ServerName { get; set; } = "Default Palworld Server";

    [Column("ServerDescription")]
    [Description("Server description")]
    public string ServerDescription { get; set; } = "";

    [Column("AdminPassword")]
    [Description("Admin password")]
    public string AdminPassword { get; set; } = "";

    [Column("ServerPassword")]
    [Description("Server password")]
    public string ServerPassword { get; set; } = "";

    [Column("PublicPort")]
    [Description("Public port number")]
    public int PublicPort { get; set; } = 8211;

    [Column("PublicIP")]
    [Description("Public IP")]
    public string PublicIP { get; set; } = "";

    [Column("RCONEnabled")]
    [Description("Enable RCON")]
    public bool RCONEnabled { get; set; } = true;

    [Column("RCONPort")]
    [Description("Port number for RCON")]
    public int RCONPort { get; set; } = 25575;

    [Column("EnablePlayerToPlayerDamage")]
    [Description("[N] Enable player-to-player damage")]
    public bool bEnablePlayerToPlayerDamage { get; set; } = false;

    [Column("EnableFriendlyFire")]
    [Description("[N] Enable friendly fire")]
    public bool bEnableFriendlyFire { get; set; } = false;

    [Column("EnableInvaderEnemy")]
    [Description("[N] Enable invader enemy")]
    public bool bEnableInvaderEnemy { get; set; } = true;

    [Column("ActiveUNKO")]
    [Description("[N] Active UNKO (Nocturne Unidentified Knock-off)")]
    public bool bActiveUNKO { get; set; } = false;

    [Column("EnableAimAssistPad")]
    [Description("[N] Enable aim assist for joysticks")]
    public bool bEnableAimAssistPad { get; set; } = true;

    [Column("EnableAimAssistKeyboard")]
    [Description("[N] Enable aim assist for keyboard")]
    public bool bEnableAimAssistKeyboard { get; set; } = false;

    [Column("DropItemMaxNum")]
    [Description("[N] Maximum number of dropped items")]
    public int DropItemMaxNum { get; set; } = 3000;

    [Column("DropItemMaxNum_UNKO")]
    [Description("[N] Maximum number of UNKO (Nocturne Unidentified Knock-off) dropped objects")]
    public int DropItemMaxNum_UNKO { get; set; } = 100;

    [Column("BaseCampMaxNum")]
    [Description("[N] Maximum number of base camps can be built")]
    public int BaseCampMaxNum { get; set; } = 128;

    [Column("BaseCampWorkerMaxNum")]
    [Description("[N] Maximum number of Pals in one camp")]
    public int BaseCampWorkerMaxNum { get; set; } = 15;

    [Column("DropItemAliveMaxHours")]
    [Description("[N] Maximum number of hours that dropped items will remain")]
    public float DropItemAliveMaxHours { get; set; } = 1.000000f;

    [Column("AutoResetGuildNoOnlinePlayers")]
    [Description("[N] Automatically resets guilds without online players")]
    public bool bAutoResetGuildNoOnlinePlayers { get; set; } = false;

    [Column("AutoResetGuildTimeNoOnlinePlayers")]
    [Description("[N] Time(h) to trigger the reset of guilds without online players")]
    public float AutoResetGuildTimeNoOnlinePlayers { get; set; } = 72.000000f;

    [Column("WorkSpeedRate")]
    [Description("[N] Work speed multiplier")]
    public float WorkSpeedRate { get; set; } = 1.000000f;

    [Column("IsMultiplayer")]
    [Description("[N] Enable multiplayer")]
    public bool bIsMultiplay { get; set; } = false;

    [Column("IsPvP")]
    [Description("[N] Enable PvP")]
    public bool bIsPvP { get; set; } = false;

    [Column("CanPickupOtherGuildDeathPenaltyDrop")]
    [Description("[N] Can pick up items dropped by the death penalty of other guilds' player")]
    public bool bCanPickupOtherGuildDeathPenaltyDrop { get; set; } = false;

    [Column("EnableNonLoginPenalty")]
    [Description("[N] Enable penalty for not logging in")]
    public bool bEnableNonLoginPenalty { get; set; } = true;

    [Column("EnableFastTravel")]
    [Description("[N] Enable fast travel")]
    public bool bEnableFastTravel { get; set; } = true;

    [Column("IsStartLocationSelectByMap")]
    [Description("[N] Enable the selection for starting location")]
    public bool bIsStartLocationSelectByMap { get; set; } = true;

    [Column("ExistPlayerAfterLogout")]
    [Description("[N] Enable player to stay in the world after logout")]
    public bool bExistPlayerAfterLogout { get; set; } = false;

    [Column("EnableDefenseOtherGuildPlayer")]
    [Description("[N] Enable defense of other guilds' player")]
    public bool bEnableDefenseOtherGuildPlayer { get; set; } = false;

    [Column("CoopPlayerMaxNum")]
    [Description("[N] Maximum number of players in cooperative mode")]
    public int CoopPlayerMaxNum { get; set; } = 4;

    [Column("Region")]
    [Description("[N] Server region")]
    public string Region { get; set; } = "";

    [Column("UseAuth")]
    [Description("[N] Enable server authentication")]
    public bool bUseAuth { get; set; } = true;

    [Column("BanListURL")]
    [Description("[N] Server ban list URL")]
    public string BanListURL { get; set; } = "https://api.palworldgame.com/api/banlist.txt";
}
