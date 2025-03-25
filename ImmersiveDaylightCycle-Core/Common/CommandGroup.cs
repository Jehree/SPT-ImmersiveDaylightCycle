using EFT.Console.Core;
using EFT.UI;
using Jehree.ImmersiveDaylightCycle.Helpers;

namespace ImmersiveDaylightCycle.Common
{
    internal class CommandGroup
    {
        [ConsoleCommand("idc_status", "", null, "Print ImmersiveDaylightCycle raid session status to console")]
        public static void LogRaidSessionStatus()
        {
            IDCCommand returnCommand = Utils.ServerRoute<IDCCommand>(Utils.ConsoleCommandURL, new IDCCommand("idc_status"));
            ConsoleScreen.Log(returnCommand.Message);
        }

        [ConsoleCommand("idc_clear", "", null, "Clear ImmersiveDaylightCycle raid session status if a crash caused it to not auto clear")]
        public static void ClearRaidSession()
        {
            IDCCommand returnCommand = Utils.ServerRoute<IDCCommand>(Utils.ConsoleCommandURL, new IDCCommand("idc_clear"));
            ConsoleScreen.Log(returnCommand.Message);
        }
    }
}
