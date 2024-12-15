using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using EFT.UI;
using ImmersiveDaylightCycle.Common;
using Jehree.ImmersiveDaylightCycle.Helpers;
using Jehree.ImmersiveDaylightCycle.Patches;


namespace Jehree.ImmersiveDaylightCycle
{
    [BepInPlugin("Jehree.ImmersiveDaylightCycle", "Jehree.ImmersiveDaylightCycle", "2.1.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource LogSource { get; private set; }
        public static bool FikaInstalled { get; private set; }
        public static bool IAmDedicatedClient { get; private set; }

        private void Awake()
        {
            FikaInstalled = Chainloader.PluginInfos.ContainsKey("com.fika.core");
            IAmDedicatedClient = Chainloader.PluginInfos.ContainsKey("com.fika.dedicated");

            LogSource = Logger;
            Settings.Init(Config);

            if (!IAmDedicatedClient)
            {
                new TimeUIPanelPatch().Enable();
                new LocationConditionsPanelPatch().Enable();
                new TimeUIUpdatePatch().Enable();
            }
            new OfflineRaidEndedPatch().Enable();
            new OnGameStartedPatch().Enable();

            ConsoleScreen.Processor.RegisterCommandGroup<CommandGroup>();
        }
    }
}
