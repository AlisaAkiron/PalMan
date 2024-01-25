using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PalMan.Agent.Migrations
{
    /// <inheritdoc />
    public partial class Initialize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContainerEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Time = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContainerEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PalWorldSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Difficulty = table.Column<string>(type: "TEXT", nullable: false),
                    DayTimeSpeedRate = table.Column<float>(type: "REAL", nullable: false),
                    NightTimeSpeedRate = table.Column<float>(type: "REAL", nullable: false),
                    ExpRate = table.Column<float>(type: "REAL", nullable: false),
                    PalCaptureRate = table.Column<float>(type: "REAL", nullable: false),
                    PalSpawnNumRate = table.Column<float>(type: "REAL", nullable: false),
                    PalDamageRateAttack = table.Column<float>(type: "REAL", nullable: false),
                    PalDamageRateDefense = table.Column<float>(type: "REAL", nullable: false),
                    PlayerDamageRateAttack = table.Column<float>(type: "REAL", nullable: false),
                    PlayerDamageRateDefense = table.Column<float>(type: "REAL", nullable: false),
                    PlayerStomachDecreaceRate = table.Column<float>(type: "REAL", nullable: false),
                    PlayerAutoHPRegeneRate = table.Column<float>(type: "REAL", nullable: false),
                    PlayerAutoHpRegeneRateInSleep = table.Column<float>(type: "REAL", nullable: false),
                    PalStomachDecreaceRate = table.Column<float>(type: "REAL", nullable: false),
                    PalStaminaDecreaceRate = table.Column<float>(type: "REAL", nullable: false),
                    PalAutoHPRegeneRate = table.Column<float>(type: "REAL", nullable: false),
                    PalAutoHpRegeneRateInSleep = table.Column<float>(type: "REAL", nullable: false),
                    BuildObjectDamageRate = table.Column<float>(type: "REAL", nullable: false),
                    BuildObjectDeteriorationDamageRate = table.Column<float>(type: "REAL", nullable: false),
                    CollectionDropRate = table.Column<float>(type: "REAL", nullable: false),
                    CollectionObjectHpRate = table.Column<float>(type: "REAL", nullable: false),
                    CollectionObjectRespawnSpeedRate = table.Column<float>(type: "REAL", nullable: false),
                    EnemyDropItemRate = table.Column<float>(type: "REAL", nullable: false),
                    DeathPenalty = table.Column<string>(type: "TEXT", nullable: false),
                    bEnablePlayerToPlayerDamage = table.Column<bool>(type: "INTEGER", nullable: false),
                    bEnableFriendlyFire = table.Column<bool>(type: "INTEGER", nullable: false),
                    bEnableInvaderEnemy = table.Column<bool>(type: "INTEGER", nullable: false),
                    bActiveUNKO = table.Column<bool>(type: "INTEGER", nullable: false),
                    bEnableAimAssistPad = table.Column<bool>(type: "INTEGER", nullable: false),
                    bEnableAimAssistKeyboard = table.Column<bool>(type: "INTEGER", nullable: false),
                    DropItemMaxNum = table.Column<int>(type: "INTEGER", nullable: false),
                    DropItemMaxNum_UNKO = table.Column<int>(type: "INTEGER", nullable: false),
                    BaseCampMaxNum = table.Column<int>(type: "INTEGER", nullable: false),
                    BaseCampWorkerMaxNum = table.Column<int>(type: "INTEGER", nullable: false),
                    DropItemAliveMaxHours = table.Column<float>(type: "REAL", nullable: false),
                    bAutoResetGuildNoOnlinePlayers = table.Column<bool>(type: "INTEGER", nullable: false),
                    AutoResetGuildTimeNoOnlinePlayers = table.Column<float>(type: "REAL", nullable: false),
                    GuildPlayerMaxNum = table.Column<int>(type: "INTEGER", nullable: false),
                    PalEggDefaultHatchingTime = table.Column<float>(type: "REAL", nullable: false),
                    WorkSpeedRate = table.Column<float>(type: "REAL", nullable: false),
                    bIsMultiplay = table.Column<bool>(type: "INTEGER", nullable: false),
                    bIsPvP = table.Column<bool>(type: "INTEGER", nullable: false),
                    bCanPickupOtherGuildDeathPenaltyDrop = table.Column<bool>(type: "INTEGER", nullable: false),
                    bEnableNonLoginPenalty = table.Column<bool>(type: "INTEGER", nullable: false),
                    bEnableFastTravel = table.Column<bool>(type: "INTEGER", nullable: false),
                    bIsStartLocationSelectByMap = table.Column<bool>(type: "INTEGER", nullable: false),
                    bExistPlayerAfterLogout = table.Column<bool>(type: "INTEGER", nullable: false),
                    bEnableDefenseOtherGuildPlayer = table.Column<bool>(type: "INTEGER", nullable: false),
                    CoopPlayerMaxNum = table.Column<int>(type: "INTEGER", nullable: false),
                    ServerPlayerMaxNum = table.Column<int>(type: "INTEGER", nullable: false),
                    ServerName = table.Column<string>(type: "TEXT", nullable: false),
                    ServerDescription = table.Column<string>(type: "TEXT", nullable: false),
                    AdminPassword = table.Column<string>(type: "TEXT", nullable: false),
                    ServerPassword = table.Column<string>(type: "TEXT", nullable: false),
                    PublicPort = table.Column<int>(type: "INTEGER", nullable: false),
                    PublicIP = table.Column<string>(type: "TEXT", nullable: false),
                    RCONEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    RCONPort = table.Column<int>(type: "INTEGER", nullable: false),
                    Region = table.Column<string>(type: "TEXT", nullable: false),
                    bUseAuth = table.Column<bool>(type: "INTEGER", nullable: false),
                    BanListURL = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PalWorldSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    TokenValue = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PalWorldServers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ContainerId = table.Column<string>(type: "TEXT", nullable: false),
                    Identifier = table.Column<string>(type: "TEXT", nullable: false),
                    Installed = table.Column<bool>(type: "INTEGER", nullable: false),
                    SettingsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PalWorldServers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PalWorldServers_PalWorldSettings_SettingsId",
                        column: x => x.SettingsId,
                        principalTable: "PalWorldSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PalWorldServers_SettingsId",
                table: "PalWorldServers",
                column: "SettingsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContainerEvents");

            migrationBuilder.DropTable(
                name: "PalWorldServers");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "PalWorldSettings");
        }
    }
}
