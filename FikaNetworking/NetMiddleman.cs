using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Jehree.ImmersiveDaylightCycle.FikaNetworking {

    // funcs that need to always be available but hollowed in SPT SP build:
        // OnHostGameStarted
        // InitOnEnable
        // InitOnDisable
        // IAmFikaClient

    public class NetMiddleman
    {
#if FIKA_COMPAT
        public static DaylightSync DaylightSync { get; private set; }
#endif

        public bool IAmFikaClient()
        {
#if FIKA_COMPAT
            return DaylightSync.IAmFikaClient();
#endif
#if SP
            return false;
#endif
        }

        public void OnHostGameStarted(Vector3 hostDateTime)
        {
#if FIKA_COMPAT
            DaylightSync.OnHostGameStarted(hostDateTime);
#endif
            //if SP do nothing
        }

        public void InitOnAwake()
        {
#if FIKA_COMPAT
            DaylightSync = new DaylightSync();
#endif
            //if SP do nothing
        }

        public void InitOnEnable()
        {
#if FIKA_COMPAT
            DaylightSync.InitOnEnable();
#endif
            //if SP do nothing
        }

        public void InitOnDisable()
        {
#if FIKA_COMPAT
            DaylightSync.InitOnDisable();
#endif
            //if SP do nothing
        }
    }
}
