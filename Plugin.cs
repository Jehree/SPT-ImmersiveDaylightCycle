using BepInEx;
using BepInEx.Logging;

#if FIKA_COMPAT
    using Jehree.ImmersiveDaylightCycle.FikaNetworking;
#endif

using Jehree.ImmersiveDaylightCycle.Helpers;
using Jehree.ImmersiveDaylightCycle.Patch;
using Jehree.ImmersiveDaylightCycle.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jehree.ImmersiveDaylightCycle
{
    //save a static out of raid time

    //at start of raid, set raid to this time and set accel multi to the one in config

    //at raid end, save the new raid to our out of raid time

    //visually set UI in menu to reflect time. Hide 2nd time option

    //repeat!

    [BepInPlugin("Jehree.ImmersiveDaylightCycle", "Jehree.ImmersiveDaylightCycle", "0.0.1")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource LogSource;
#if FIKA_COMPAT
        public static DaylightSync DaylightSync { get; private set; };
#endif
        private void Awake()
        {
            LogSource = Logger;
            Settings.Init(Config);

#if FIKA_COMPAT
        DaylightSync = new DaylightSync();
#endif


            new TimeUIPanelPatch().Enable();
            new OnGameStartedPatch().Enable();
            new OfflineRaidEndedPatch().Enable();
            new TimeUIUpdatePatch().Enable();
            new LocationConditionsPanelPatch().Enable();
        }

#if FIKA_COMPAT
        private void OnEnable()
        {
            DaylightSync.InitOnEnable();
        }

        private void OnDisable(bool disposing)
        {
            DaylightSync.InitOnDisable();
        }
#endif
    }
}
