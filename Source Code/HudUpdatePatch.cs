using System;
using HarmonyLib;
using System.Net.Http;
using UnityEngine;
using static StopTime.ImpostorRole;
using Hazel;

namespace StopTime
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    public static class HudStartPatch
    {
        public static CooldownButton freezeButton;
        public static void Postfix(HudManager __instance)
        {
            #region add the freeze button
            freezeButton = new CooldownButton(delegate ()
            {
                FreezeButton.onClick(freezeButton);
            }, HarmonyMain.freezeCooldown.GetValue(), "StopTime.Assets.freeze.png", 100f, Vector2.zero, CooldownButton.Category.OnlyImpostor, __instance, HarmonyMain.freezeTimer.GetValue(), delegate ()
            {
                FreezeButton.onEnd();
            });

            freezeButton.killButtonManager.gameObject.SetActive(false);
            #endregion
        }

    }
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public static class HudUpdatePatch
    {
        public static void Postfix(HudManager __instance)
        {
            if (ShipStatus.Instance != null)
            {
                CooldownButton.HudUpdate();
            }
            else
            {
                HudStartPatch.freezeButton.killButtonManager.gameObject.SetActive(false);
            }
        }
    }
    public static class FreezeButton
    {
        public static void onClick(CooldownButton bttn)
        {
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.TimeEdit, Hazel.SendOption.None, -1);
            writer.Write(true);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            //PlayerControl.LocalPlayer.Collider.isTrigger = true;
        }
        public static void onEnd()
        {
             MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.TimeEdit, Hazel.SendOption.None, -1);
            writer.Write(false);
             AmongUsClient.Instance.FinishRpcImmediately(writer);
           // PlayerControl.LocalPlayer.Collider.isTrigger = false;
        }
    }
}
