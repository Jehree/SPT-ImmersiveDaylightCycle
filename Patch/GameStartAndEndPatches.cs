using Aki.Reflection.Patching;
using Comfort.Common;
using EFT;
using Jehree.ImmersiveDaylightCycle.FikaNetworking;
using HarmonyLib;
using Jehree.ImmersiveDaylightCycle.Helpers;
using JsonType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Aki.Reflection.Utils;

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

            // clients will set their time via the DaylightSync packet, so they don't need to do so here
            // this will always return false in Release (non Fika) build
            if (DaylightSync.IAmFikaClient()) return; 

            DateTime dateTime = Settings.GetCurrentGameTime();

            // this function is hollowed and does nothing in Release (non Fika) build
            Plugin.DaylightSync.OnHostGameStarted(new UnityEngine.Vector3(dateTime.Hour, dateTime.Minute, dateTime.Second));

            Utils.SetRaidTime();
#if DEBUG
            Plugin.LogSource.LogError("OnGameStarted ran!");
#endif
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
                targetClass.GetMethods().Any(method => method.Name == "OfflineRaidEnded") &&
                targetClass.GetMethods().Any(method => method.Name == "ReceiveInsurancePrices")//
            );

            return AccessTools.Method(_targetClassType.GetTypeInfo(), "OfflineRaidEnded");
        }

        [PatchPostfix]
        static void Postfix(ExitStatus exitStatus, string exitName, double raidSeconds)
        {
            if (!Settings.modEnabled.Value) return;

            DateTime newGameTime;

            if ((exitStatus.ToString() == "Left" || exitStatus.ToString() == "Killed") && Settings.timeResetsOnDeath.Value) {
                newGameTime = Settings.GetCurrentGameTime(true);
            } else {
                DateTime oldGameTime = Settings.GetCurrentGameTime();
                newGameTime = oldGameTime.AddSeconds((raidSeconds * Settings.daylightCycleRate.Value) + (Settings.raidExitTimeJump.Value * 3600));
            }

            Settings.SetCurrentGameTime(newGameTime.Hour, newGameTime.Minute, newGameTime.Second);
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
