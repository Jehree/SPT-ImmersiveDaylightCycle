using EFT.Console.Core;
using EFT.UI;
using Jehree.ImmersiveDaylightCycle;
using Jehree.ImmersiveDaylightCycle.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmersiveDaylightCycle.Common
{
    internal class CommandGroup
    {
        [ConsoleCommand("idc_status", "", null, "Print ImmersiveDaylightCycle raid session status to console")]
        public static void LogRaidSessionStatus()
        {
            IDCCommand command = new IDCCommand("idc_status");
            string json = JsonConvert.SerializeObject(command);

            Plugin.LogSource.LogError(json);

            IDCCommand returnCommand = Utils.ServerRoute<IDCCommand>(Utils.ConsoleCommandURL, json);
            ConsoleScreen.Log(returnCommand.Message);
        }

        [ConsoleCommand("idc_clear", "", null, "Clear ImmersiveDaylightCycle raid session status if a crash caused it to not auto clear")]
        public static void ClearRaidSession()
        {
            IDCCommand command = new IDCCommand("idc_clear");
            string message = Utils.ServerRoute<IDCCommand>(Utils.ConsoleCommandURL, JsonConvert.SerializeObject(command)).Message;
            ConsoleScreen.Log(message);
        }
    }
}
