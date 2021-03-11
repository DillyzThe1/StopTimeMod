using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;
using Essentials.Options;
using System;
using BepInEx.Configuration;
using System.Linq;
using System.Net;

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
        public ConfigEntry<string> Name { get; set; }
        public ConfigEntry<string> Ip { get; set; }
        public ConfigEntry<ushort> Port { get; set; }
        public override void Load()
        {
            Harmony.PatchAll();
            Name = Config.Bind("Server", "Name", "CheepYT - EU");
            Ip = Config.Bind("Server", "Ipv4 or Hostname", "207.180.234.175");
            Port = Config.Bind("Server", "Port", (ushort)22023);
            var defReg = AOBNFCIHAJL.DefaultRegions.ToList();
            if (Uri.CheckHostName(Ip.Value).ToString() == "Dns")
            {
                foreach (IPAddress adress in Dns.GetHostAddresses(Ip.Value))
                {
                    if (adress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        Ip.Value = adress.ToString();
                        break;
                    }
                }
            }
            defReg.Insert(0, new OIBMKGDLGOG(Name.Value, Ip.Value, new[] { new PLFDMKKDEMI($"{Name.Value}-Master-1", Ip.Value, Port.Value) }));
            AOBNFCIHAJL.DefaultRegions = defReg.ToArray();
        }
    }
}