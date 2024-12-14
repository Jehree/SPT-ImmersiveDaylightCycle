using SPT.Reflection.Patching;
using EFT;
using HarmonyLib;
using Jehree.ImmersiveDaylightCycle.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SPT.Reflection.Utils;
using Jehree.ImmersiveDaylightCycle.Fika;
using ImmersiveDaylightCycle.Common;
using SPT.Common.Utils;
using Sirenix.Serialization;
using Newtonsoft.Json;

namespace Jehree.ImmersiveDaylightCycle.Patches {

    internal class OnGameStartedPatch : ModulePatch 
    {

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(GameWorld), nameof(GameWorld.OnGameStarted));
        }

        [PatchPostfix]
        static void Postfix()
        {
            if (!Settings.modEnabled.Value) return;

            if (FikaInterface.IAmHost())
            {
                Utils.ServerRoute(Utils.HostRaidStartedURL, FikaInterface.GetRaidId());
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
                targetClass.GetMethods().Any(method => method.Name == "ReceiveInsurancePrices")
            );

            return AccessTools.Method(_targetClassType.GetTypeInfo(), "LocalRaidEnded");
        }

        [PatchPostfix]
        static void Postfix(LocalRaidSettings settings, GClass1924 results, GClass1301[] lostInsuredItems, Dictionary<string, GClass1301[]> transferItems)
        {
            if (!Settings.modEnabled.Value) return;

            IDCClientExitInfo exitInfo = new IDCClientExitInfo
            {
                RaidId = FikaInterface.GetRaidId(),
                ProfileId = FikaInterface.GetProfileId(),
                ExitStatus = results.result,
                IsHost = FikaInterface.IAmHost(),
                IsDedicatedClient = Plugin.IAmDedicatedClient,
                SecondsInRaid = results.playTime,
            };

            Utils.ServerRoute(Utils.ClientLeftRaidURL, JsonConvert.SerializeObject(exitInfo));
        }
    }
}
