using Comfort.Common;
using EFT;

namespace Jehree.ImmersiveDaylightCycle.Fika
{
    internal class FikaInterface
    {
        public static bool IAmHost()
        {
            if (!Plugin.FikaInstalled) return true;
            return DaylightSyncWrapper.IAmHost();
        }
        public static string GetRaidId()
        {
            if (!Plugin.FikaInstalled) return Singleton<GameWorld>.Instance.MainPlayer.ProfileId;
            return DaylightSyncWrapper.GetRaidId();
        }
        public static string GetProfileId()
        {
            if (!Plugin.FikaInstalled) return Singleton<GameWorld>.Instance.MainPlayer.ProfileId;
            return DaylightSyncWrapper.GetProfileId();
        }
    }
}
