using UnityEngine;

namespace Jehree.ImmersiveDaylightCycle.FikaNetworking {

    // This class must not cache any data. Do that in the wrapper classes if needed.
    public static class FikaBridge
    {
        public static bool IAmFikaClient()
        {
            return DaylightSync.IAmFikaClient();
        }

        public static void OnHostGameStarted(Vector3 hostDateTime)
        {
            DaylightSync.OnHostGameStarted(hostDateTime);
        }

        public static void InitOnAwake()
        {
            // none atm
        }

        public static void InitOnEnable()
        {
            DaylightSync.InitOnEnable();
        }

        public static void InitOnDisable()
        {
            DaylightSync.InitOnDisable();
        }
    }
}
