using Jehree.ImmersiveDaylightCycle;
using Jehree.ImmersiveDaylightCycle.Fika;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ImmersiveDaylightCycle.Fika
{
    // This class should contain zero types or references to wrapper classes
    internal class FikaInterface
    {
        public static bool IAmFikaClient()
        {
            if (!Plugin.FikaInstalled) return false;
            return FikaBridge.IAmFikaClient();
        }

        public static bool IAmFikaServer()
        {
            if (!Plugin.FikaInstalled) return false;
            return FikaBridge.IAmFikaServer();
        }

        public static void OnClientGameStarted()
        {
            if (!Plugin.FikaInstalled) return;
            FikaBridge.OnClientGameStarted();
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
