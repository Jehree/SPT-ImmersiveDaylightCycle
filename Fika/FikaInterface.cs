namespace Jehree.ImmersiveDaylightCycle.Fika
{
    internal class FikaInterface
    {
        public static bool IAmFikaClient()
        {
            if (!Plugin.FikaInstalled) return false;
            return DaylightSyncWrapper.IAmFikaServer();
        }

        public static bool IAmFikaServer()
        {
            if (!Plugin.FikaInstalled) return false;
            return DaylightSyncWrapper.IAmFikaServer();
        }

        public static void OnClientGameStarted()
        {
            if (!Plugin.FikaInstalled) return;
            DaylightSyncWrapper.OnClientGameStarted();
        }

        public static void InitOnEnable()
        {
            if (!Plugin.FikaInstalled) return;
            DaylightSyncWrapper.InitOnEnable();
        }

        public static void InitOnDisable()
        {
            if (!Plugin.FikaInstalled) return;
            DaylightSyncWrapper.InitOnDisable();
        }
    }
}
