using EFT;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmersiveDaylightCycle.Common
{
    public class IDCClientExitInfo
    {
        [JsonProperty("raid_id")]
        public string RaidId { get; set; }

        [JsonProperty("profile_id")]
        public string ProfileId { get; set; }

        [JsonProperty("exit_status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ExitStatus ExitStatus { get; set; }

        [JsonProperty("is_host")]
        public bool IsHost { get; set; }

        [JsonProperty("is_dedicated_client")]
        public bool IsDedicatedClient { get; set; }

        [JsonProperty("seconds_in_raid")]
        public int SecondsInRaid { get; set; }
    }

    public class IDCTime
    {
        [JsonProperty("hour")]
        public int Hour { get; set; }

        [JsonProperty("minute")]
        public int Minute { get; set; }

        [JsonProperty("second")]
        public int Second { get; set; }

        [JsonProperty("cycle_rate")]
        public int CycleRate { get; set; }
    }

    public class IDCCommand
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; } = "";

        public IDCCommand(string type)
        {
            Type = type;
        }
    }
}
