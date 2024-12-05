using Jehree.ImmersiveDaylightCycle;
using Jehree.ImmersiveDaylightCycle.FikaNetworking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ImmersiveDaylightCycle.FikaNetworking
{
    // This class should contain zero types or references to wrapper classes
    internal class FikaInterface
    {
        public static bool IAmFikaClient()
        {
            if (!Plugin.FikaInstalled) return false;
            return FikaBridge.IAmFikaClient();
        }

        public static void OnHostGameStarted(Vector3 hostDateTime)
        {
            if (!Plugin.FikaInstalled) return;
            FikaBridge.OnHostGameStarted(hostDateTime);
        }

        public static void InitOnAwake()
        {
            if (!Plugin.FikaInstalled) return;
            // none atm
        }

        public static void InitOnEnable()
        {
            if (!Plugin.FikaInstalled) return;
            FikaBridge.InitOnEnable();
        }

        public static void InitOnDisable()
        {
            if (!Plugin.FikaInstalled) return;
            FikaBridge.InitOnDisable();
        }
    }
}
