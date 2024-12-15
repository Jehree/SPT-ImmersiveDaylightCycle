using BepInEx.Configuration;

namespace Jehree.ImmersiveDaylightCycle.Helpers {
    internal class Settings
    {
        public static ConfigEntry<bool> modEnabled;
        public static ConfigEntry<bool> factoryTimeAlwaysSelectable;

        public static void Init(ConfigFile Config)
        {
            modEnabled = Config.Bind(
                "1: Mod",
                "Mod Enabled",
                true,
                "Enables and disables the mod (wow woah crazy bro INSANE)."
            );
            factoryTimeAlwaysSelectable = Config.Bind(
                "3: Factory",
                "Factory Time Always Selectable",
                false,
                "Enable this to make Factory always have selectable day or night time."
            );
        }
    }
}
