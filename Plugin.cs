using BepInEx;
using BepInEx.Logging;
using Jehree.ImmersiveDaylightCycle.FikaNetworking;
using Jehree.ImmersiveDaylightCycle.Helpers;
using Jehree.ImmersiveDaylightCycle.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jehree.ImmersiveDaylightCycle
{
    [BepInPlugin("Jehree.ImmersiveDaylightCycle", "Jehree.ImmersiveDaylightCycle", "1.0.0")]
#if SP
    [BepInIncompatibility("com.fika.core")]
#endif
#if FIKA_COMPAT
    [BepInDependency("com.fika.core")]
#endif
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource LogSource { get; private set; }
        public static NetMiddleman NetMiddleman { get; private set; }

        private void Awake()
        {
            LogSource = Logger;
            Settings.Init(Config);

            NetMiddleman = new NetMiddleman();
            NetMiddleman.InitOnAwake();

            new TimeUIPanelPatch().Enable();
            new OnGameStartedPatch().Enable();
            new OfflineRaidEndedPatch().Enable();
            new TimeUIUpdatePatch().Enable();
            new LocationConditionsPanelPatch().Enable();
        }

        private void OnEnable()
        {
            NetMiddleman.InitOnEnable();
        }

        private void OnDisable()
        {
            NetMiddleman.InitOnDisable();
        }
    }
}
