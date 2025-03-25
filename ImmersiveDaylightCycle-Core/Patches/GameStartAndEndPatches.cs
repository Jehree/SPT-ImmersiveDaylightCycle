﻿using Comfort.Common;
using EFT;
using HarmonyLib;
using ImmersiveDaylightCycle.Common;
using ImmersiveDaylightCycle.Fika;
using Jehree.ImmersiveDaylightCycle.Helpers;
using SPT.Reflection.Patching;
using SPT.Reflection.Utils;
using System;
using System.Linq;
using System.Reflection;

namespace Jehree.ImmersiveDaylightCycle.Patches
{

    internal class OnGameStartedPatch : ModulePatch
    {

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(GameWorld), nameof(GameWorld.OnGameStarted));
        }

        [PatchPostfix]
        static void Postfix()
        {
            if (!Settings.ModEnabled.Value) return;

            if (FikaBridge.IAmHost())
            {
                Utils.ServerRoute(Utils.HostRaidStartedURL, FikaBridge.GetRaidId());
            }
            Utils.SetRaidTime();
        }
    }

    internal class OfflineRaidEndedPatch : ModulePatch
    {
        private static Type _targetClassType;

        protected override MethodBase GetTargetMethod()
        {
            _targetClassType = PatchConstants.EftTypes.Single(targetClass =>
                !targetClass.IsInterface &&
                !targetClass.IsNested &&
                targetClass.GetMethods().Any(method => method.Name == "LocalRaidEnded") &&
                targetClass.GetMethods().Any(method => method.Name == "LocalRaidStarted")
            );

            return AccessTools.Method(_targetClassType.GetTypeInfo(), "LocalRaidEnded");
        }

        [PatchPostfix]
        static void Postfix(LocalRaidSettings settings, GClass1959 results)
        {
            if (!Settings.ModEnabled.Value) return;

            IDCClientExitInfo exitInfo = new IDCClientExitInfo
            {
                RaidId = FikaBridge.GetRaidId(),
                ProfileId = Singleton<GameWorld>.Instance.MainPlayer.ProfileId,
                ExitStatus = results.result,
                IsHost = FikaBridge.IAmHost(),
                IsDedicatedClient = Plugin.IAmDedicatedClient,
                SecondsInRaid = results.playTime,
            };

            Utils.ServerRoute(Utils.ClientLeftRaidURL, exitInfo);
        }
    }
}
