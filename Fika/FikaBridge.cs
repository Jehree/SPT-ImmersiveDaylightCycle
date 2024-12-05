using ImmersiveDaylightCycle.Fika;
using UnityEngine;

namespace Jehree.ImmersiveDaylightCycle.Fika {

    // This class must not cache any data. Do that in the wrapper classes if needed.
    public static class FikaBridge
    {
        public static bool IAmFikaClient()
        {
            return DaylightSyncWrapper.IAmFikaClient();
        }

        public static bool IAmFikaServer()
        {
            return DaylightSyncWrapper.IAmFikaServer();
        }

        public static void OnClientGameStarted()
        {
            DaylightSyncWrapper.OnClientGameStarted();
        }

        public static void InitOnEnable()
        {
            DaylightSyncWrapper.InitOnEnable();
        }

        public static void InitOnDisable()
        {
            DaylightSyncWrapper.InitOnDisable();
        }
    }
}
