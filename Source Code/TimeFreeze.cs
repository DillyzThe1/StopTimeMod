using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using Reactor.Unstrip;
using static StopTime.ImpostorRole;
using Essentials.CustomOptions;

namespace StopTime
{
    public class DeadPlayer
    {
        public byte KillerId { get; set; }
        public byte PlayerId { get; set; }
        public DateTime KillTime { get; set; }
        public DeathReason DeathReason { get; set; }
    }
    public static class Extensions
    {
        public static bool isPlayerRole(this PlayerControl plr, string RoleName)
        {
            if (plr.getModdedControl() != null)
            {
                return plr.getModdedControl().Role == RoleName;
            }
            else
            {
                return false;
            }
        }
        public static bool isPlayerImmortal(this PlayerControl plr)
        {
            if (plr.getModdedControl() != null)
            {
                return plr.getModdedControl().Immortal;
            }
            else
            {
                return false;
            }
        }
        public static ModPlayerControl getModdedControl(this PlayerControl plr)
        {
            return Main.Logic.AllModPlayerControl.Find(x => x.PlayerControl == plr);
        }
    }
    [HarmonyPatch]
    public class ImpostorRole
    {
        public static class Main
        {
            public static Assets Assets = new Assets();
            public static ModdedPalette Palette = new ModdedPalette();
            public static ModdedConfig Config = new ModdedConfig();
            public static ModdedLogic Logic = new ModdedLogic();
        }
        public class ModdedLogic
        {
            public ModPlayerControl getRolePlayer(string roleName)
            {
                return Main.Logic.AllModPlayerControl.Find(x => x.Role == roleName);
            }
            public ModPlayerControl getImmortalPlayer()
            {
                return Main.Logic.AllModPlayerControl.Find(x => x.Immortal);
            }
            public bool anyPlayerImmortal()
            {
                return Main.Logic.AllModPlayerControl.FindAll(x => x.Immortal).Count > 0;
            }
            public List<ModPlayerControl> AllModPlayerControl = new List<ModPlayerControl>();
            public bool sabotageActive { get; set; }
        }

        public class Assets
        {
            public AssetBundle bundle;
            public AssetBundle bundle2;
            public Sprite freezeIcon;
        }
        public class ModdedPalette
        {
            public Color senseiColor = new Color(0.51f, 0.27f, 0.79f, 1f);
        }
        public class ModPlayerControl
        {
            public PlayerControl PlayerControl { get; set; }
            public string Role { get; set; }
            public DateTime? LastAbilityTime { get; set; }
            public bool UsedAbility { get; set; }
            public bool Immortal { get; set; }
        }
        public static GameObject rend;
        public static List<DeadPlayer> killedPlayers = new List<DeadPlayer>();
        public static PlayerControl currentTarget = null;
        public static PlayerControl localPlayer = null;
        public static List<PlayerControl> localPlayers = new List<PlayerControl>();
        public static System.Random rng = new System.Random();
        public static KillButtonManager KillButton;
        public static int KBTarget;
        public static double DistLocalClosest;
        public static string versionString = "1.1.0";
        public class ModdedConfig
        {
            public float freezeTimer { get; set; }
            public float freezeCooldown { get; set; }
            public void SetConfigSettings()
            {
                this.freezeCooldown = HarmonyMain.freezeCooldown.GetValue();
                this.freezeTimer = HarmonyMain.freezeTimer.GetValue();
            }
        }
        [HarmonyPatch(typeof(VersionShower), "Start")]
        public static class VersionStartPatch
        {
            static void Postfix(VersionShower __instance)
            {
                var obj = UnityEngine.GameObject.Find("ReactorVersion");
                if (obj != null) UnityEngine.GameObject.Destroy(obj);
                __instance.text.Text += "\nStop Time mod [F6FF00FF]" + versionString + "[] by [3AA3D9FF]DillyzThe1[].";
            }
        }
        [HarmonyPatch(typeof(PingTracker), "Update")]
        public static class PingPatch
        {
            public static void Postfix(PingTracker __instance)
            {
                __instance.text.Text += "\nStop Time mod V" + versionString + "\n[3AA3D9]github.com/DillyzThe1[]";
            }
        }
        [HarmonyPatch(typeof(StatsManager), nameof(StatsManager.AmBanned), MethodType.Getter)]
        public static class AmBannedPatch
        {
            public static void Postfix(out bool __result)
            {
                __result = false;
            }
        }
    }
}
