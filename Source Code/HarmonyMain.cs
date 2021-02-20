using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;
using Essentials.CustomOptions;

namespace StopTime
{
    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    public class HarmonyMain : BasePlugin
    {
        public const string Id = "gg.reactor.stoptime";
        public Harmony Harmony { get; } = new Harmony(Id);
        public static CustomNumberOption freezeCooldown = CustomOption.AddNumber("Freeze Button Cooldown", 10f, 15f, 60f, 2.5f);
        public static CustomNumberOption freezeTimer = CustomOption.AddNumber("Freeze Time Active", 20f, 15f, 45f, 2.5f);
        public override void Load()
        {
            CustomOption.ShamelessPlug = false;
            Harmony.PatchAll();
        }
    }
}