using Aki.Reflection.Patching;
using Comfort.Common;
using EFT;

#if FIKA_COMPAT
    using Fika.Core.Networking;
    using Jehree.ImmersiveDaylightCycle.FikaNetworking;
#endif

using HarmonyLib;
using Jehree.ImmersiveDaylightCycle.Helpers;
using JsonType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
#if FIKA_COMPAT
            if (Singleton<FikaClient>.Instantiated) return; //clients will set their time via the DaylightSync packet, so they don't need to do so here

            DateTime dateTime = Settings.GetCurrentGameTime();
            Plugin.DaylightSync.OnHostGameStarted(new UnityEngine.Vector3(dateTime.Hour, dateTime.Minute, dateTime.Second));
#endif
            Utils.SetRaidTime();
#if DEBUG
            Plugin.LogSource.LogError("OnGameStarted ran!");
#endif
        }
    }

    internal class OfflineRaidEndedPatch : ModulePatch
    {

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(Class263), nameof(Class263.OfflineRaidEnded));
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
