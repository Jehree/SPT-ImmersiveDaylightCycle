using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using ImmersiveDaylightCycle.FikaNetworking;
using Jehree.ImmersiveDaylightCycle.FikaNetworking;
using Jehree.ImmersiveDaylightCycle.Helpers;
using Jehree.ImmersiveDaylightCycle.Patches;


namespace Jehree.ImmersiveDaylightCycle
{
    [BepInPlugin("Jehree.ImmersiveDaylightCycle", "Jehree.ImmersiveDaylightCycle", "2.1.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource LogSource { get; private set; }
        public static bool FikaInstalled { get; private set; }

        private void Awake()
        {
            FikaInstalled = Chainloader.PluginInfos.ContainsKey("com.fika.core");

            LogSource = Logger;
            Settings.Init(Config);

            LogSource.LogError(FikaInstalled);

            FikaInterface.InitOnAwake();

            new TimeUIPanelPatch().Enable();
            new OnGameStartedPatch().Enable();
            new OfflineRaidEndedPatch().Enable();
            new TimeUIUpdatePatch().Enable();
            new LocationConditionsPanelPatch().Enable();
        }

        private void OnEnable()
        {
            FikaInterface.InitOnEnable();
        }

        private void OnDisable()
        {
            FikaInterface.InitOnDisable();
        }
    }
}
