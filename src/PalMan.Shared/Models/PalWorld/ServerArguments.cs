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

    public ServerArguments(ServerArguments arguments)
    {
        var properties = typeof(ServerArguments).GetProperties();
        foreach (var property in properties)
        {
            var value = property.GetValue(arguments);
            property.SetValue(this, value);
        }
    }

    public string Difficulty { get; set; } = "None";

    public float DayTimeSpeedRate { get; set; } = 1.000000f;

    public float NightTimeSpeedRate { get; set; } = 1.000000f;

    public float ExpRate { get; set; } = 1.000000f;

    public float PalCaptureRate { get; set; } = 1.000000f;

    public float PalSpawnNumRate { get; set; } = 1.000000f;

    public float PalDamageRateAttack { get; set; } = 1.000000f;

    public float PalDamageRateDefense { get; set; } = 1.000000f;

    public float PlayerDamageRateAttack { get; set; } = 1.000000f;

    public float PlayerDamageRateDefense { get; set; } = 1.000000f;

    public float PlayerStomachDecreaceRate { get; set; } = 1.000000f;

    public float PlayerAutoHPRegeneRate { get; set; } = 1.000000f;

    public float PlayerAutoHpRegeneRateInSleep { get; set; } = 1.000000f;

    public float PalStomachDecreaceRate { get; set; } = 1.000000f;

    public float PalStaminaDecreaceRate { get; set; } = 1.000000f;

    public float PalAutoHPRegeneRate { get; set; } = 1.000000f;

    public float PalAutoHpRegeneRateInSleep { get; set; } = 1.000000f;

    public float BuildObjectDamageRate { get; set; } = 1.000000f;

    public float BuildObjectDeteriorationDamageRate { get; set; } = 1.000000f;

    public float CollectionDropRate { get; set; } = 1.000000f;

    public float CollectionObjectHpRate { get; set; } = 1.000000f;

    public float CollectionObjectRespawnSpeedRate { get; set; } = 1.000000f;

    public float EnemyDropItemRate { get; set; } = 1.000000f;

    public string DeathPenalty { get; set; } = "All";

    public bool bEnablePlayerToPlayerDamage { get; set; } = false;

    public bool bEnableFriendlyFire { get; set; } = false;

    public bool bEnableInvaderEnemy { get; set; } = true;

    public bool bActiveUNKO { get; set; } = false;

    public bool bEnableAimAssistPad { get; set; } = true;

    public bool bEnableAimAssistKeyboard { get; set; } = false;

    public int DropItemMaxNum { get; set; } = 3000;

    public int DropItemMaxNum_UNKO { get; set; } = 100;

    public int BaseCampMaxNum { get; set; } = 128;

    public int BaseCampWorkerMaxNum { get; set; } = 15;

    public float DropItemAliveMaxHours { get; set; } = 1.000000f;

    public bool bAutoResetGuildNoOnlinePlayers { get; set; } = false;

    public float AutoResetGuildTimeNoOnlinePlayers { get; set; } = 72.000000f;

    public int GuildPlayerMaxNum { get; set; } = 20;

    public float PalEggDefaultHatchingTime { get; set; } = 72.000000f;

    public float WorkSpeedRate { get; set; } = 1.000000f;

    public bool bIsMultiplay { get; set; } = false;

    public bool bIsPvP { get; set; } = false;

    public bool bCanPickupOtherGuildDeathPenaltyDrop { get; set; } = false;

    public bool bEnableNonLoginPenalty { get; set; } = true;

    public bool bEnableFastTravel { get; set; } = true;

    public bool bIsStartLocationSelectByMap { get; set; } = true;

    public bool bExistPlayerAfterLogout { get; set; } = false;

    public bool bEnableDefenseOtherGuildPlayer { get; set; } = false;

    public int CoopPlayerMaxNum { get; set; } = 4;

    public int ServerPlayerMaxNum { get; set; } = 32;

    public string ServerName { get; set; } = "Default Palworld Server";

    public string ServerDescription { get; set; } = "";

    public string AdminPassword { get; set; } = "";

    public string ServerPassword { get; set; } = "";

    public int PublicPort { get; set; } = 8211;

    public string PublicIP { get; set; } = "";

    public bool RCONEnabled { get; set; } = true;

    public int RCONPort { get; set; } = 25575;

    public string Region { get; set; } = "";

    public bool bUseAuth { get; set; } = true;

    public string BanListURL { get; set; } = "https://api.palworldgame.com/api/banlist.txt";
}
