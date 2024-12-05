using SPT.Reflection.Patching;
using Comfort.Common;
using EFT;
using HarmonyLib;
using Jehree.ImmersiveDaylightCycle.Helpers;
using JsonType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SPT.Reflection.Utils;
using ImmersiveDaylightCycle;
using ImmersiveDaylightCycle.Fika;



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

            if (FikaInterface.IAmFikaClient())
            {
                FikaInterface.OnClientGameStarted();
            }
            else
            {
                DateTime dateTime = Settings.GetSavedGameTime();
                Utils.SetRaidTime();
            }
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

            bool playerDied = results.result == ExitStatus.Killed;
            bool playerDisconnected = results.result == ExitStatus.Left;
            bool resetNeeded = playerDied && Settings.timeResetsOnDeath.Value || playerDisconnected && Settings.timeResetsOnDisconnect.Value;

            DateTime newGameTime = resetNeeded
                ? Settings.GetResetGameTime()
                : Settings.GetSavedGameTime().AddSeconds(results.playTime * Settings.daylightCycleRate.Value + Settings.raidExitTimeJump.Value * 3600);

            Settings.SaveGameTime(newGameTime.Hour, newGameTime.Minute, newGameTime.Second);
#if DEBUG
            Plugin.LogSource.LogError("total secs: " + raidSeconds * Settings.daylightCycleRate.Value);
            Plugin.LogSource.LogError("total minutes: " + (raidSeconds * Settings.daylightCycleRate.Value) / 60);
            Plugin.LogSource.LogError("total hours: " + (raidSeconds * Settings.daylightCycleRate.Value) / 3600);

            Plugin.LogSource.LogError("new hour: " + newGameTime.Hour);
            Plugin.LogSource.LogError("new minute: " + newGameTime.Minute);
            Plugin.LogSource.LogError("new second: " + newGameTime.Second);

            Plugin.LogSource.LogError("exit status: " + exitStatus.ToString());
            Plugin.LogSource.LogError("exit name: " + exitName);
#endif
        }
    }
}
