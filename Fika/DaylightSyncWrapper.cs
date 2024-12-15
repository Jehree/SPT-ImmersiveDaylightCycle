using Fika.Core.Networking;
using Comfort.Common;
using Fika.Core.Coop.Utils;

namespace Jehree.ImmersiveDaylightCycle.Fika
{
    internal class DaylightSyncWrapper
    {
        public static bool IAmHost()
        {
            return Singleton<FikaServer>.Instantiated;
        }

        public static string GetRaidId()
        {
            return FikaBackendUtils.GroupId;
        }

        public static string GetProfileId()
        {
            return FikaBackendUtils.Profile.ProfileId;
        }
    }
}
