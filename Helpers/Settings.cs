using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jehree.ImmersiveDaylightCycle.Helpers {
    internal class Settings
    {
        public static ConfigEntry<bool> modEnabled;
        public static ConfigEntry<int> raidExitTimeJump;
        public static ConfigEntry<float> daylightCycleRate;

        public static ConfigEntry<int> currentHour;
        public static ConfigEntry<int> currentMinute;
        public static ConfigEntry<int> currentSecond;

        public static ConfigEntry<bool> timeResetsOnDeath;
        public static ConfigEntry<int> resetHour;
        public static ConfigEntry<int> resetMinute;
        public static ConfigEntry<int> resetSecond;

        public static void Init(ConfigFile Config)
        {
            modEnabled = Config.Bind(
                "Daylight Cycle",
                "1: Mod Enabled",
                true,
                "Enables and disables the mod (wow woah crazy bro INSANE)."
            );

            raidExitTimeJump = Config.Bind(
                "Daylight Cycle",
                "Raid End Time Jump (hours)",
                2,
                "Number of hours to skip ahead in time when a raid ends."
            );

            daylightCycleRate = Config.Bind(
                "Daylight Cycle",
                "Cycle Rate",
                7f,
                "Rate at which the daylight cycle progresses. Vanilla default is 7."
            );

            currentHour = Config.Bind(
                "Daylight Cycle",
                "Current Out Of Raid Hour",
                8,
                new ConfigDescription(
                    "Current hour (updates at raid end) you may need to manually sync this with your friends if using Fika and playing on Factory to ensure the correct time selection is made",
                    new AcceptableValueRange<int>(0, 23)
                )
            );

            currentMinute = Config.Bind(
                "Daylight Cycle",
                "Current Out Of Raid Minute",
                0,
                new ConfigDescription(
                "Current minute (updates at raid end)",
                new AcceptableValueRange<int>(0, 59)
                )
            );

            currentSecond = Config.Bind(
                "Daylight Cycle",
                "Current Out Of Raid Second",
                0,
                new ConfigDescription(
                "Current second (updates at raid end)",
                new AcceptableValueRange<int>(0, 59)
                )
            );

            timeResetsOnDeath = Config.Bind(
                "Daylight Cycle",
                "Time Resets On Death",
                true,
                "Enable this to make the time reset to the time below on death."
            );

            resetHour = Config.Bind(
                "Daylight Cycle",
                "Death Reset Hour",
                8,
                new ConfigDescription(
                    "Death reset hour, time will update to this time after death.",
                    new AcceptableValueRange<int>(0, 23)
                )
            );

            resetMinute = Config.Bind(
                "Daylight Cycle",
                "Death Reset Minute",
                0,
                new ConfigDescription(
                "Death reset minute, time will update to this time after death.",
                new AcceptableValueRange<int>(0, 59)
                )
            );

            resetSecond = Config.Bind(
                "Daylight Cycle",
                "Death Reset Second",
                0,
                new ConfigDescription(
                "Death reset second, time will update to this time after death.",
                new AcceptableValueRange<int>(0, 59)
                )
            );
        }

        public static DateTime GetCurrentGameTime(bool resetOrNah = false)
        {
            if ( resetOrNah ) {
                return new DateTime(2024, 6, 8, resetHour.Value, resetMinute.Value, resetSecond.Value);
            } else {
                return new DateTime(2024, 6, 8, currentHour.Value, currentMinute.Value, currentSecond.Value);
            }
        }

        public static void SetCurrentGameTime(int hour, int minute, int second)
        {
            currentHour.Value = hour;
            currentMinute.Value = minute;
            currentSecond.Value = second;
        }
    }
}
