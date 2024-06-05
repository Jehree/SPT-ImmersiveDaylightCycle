using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using EFT;
using Comfort.Common;

namespace Jehree.ImmersiveDaylightCycle.Helpers {
    internal class Utils
    {
        public static bool IsDayTime(DateTime dateTime)
        {
            if (dateTime.Hour > 5 && dateTime.Hour < 21) {
                return true;
            }
            else {
                return false;
            }
        }

        public static void DisableTimeUI(TextMeshProUGUI phaseToDisable, Toggle timeToggle)
        {
            try {
                timeToggle.isOn = false;
                timeToggle.enabled = false;
                phaseToDisable.transform.gameObject.SetActive(false);
                var bg = timeToggle.transform.Find("Background").gameObject;
                bg.GetComponent<Image>().color = Color.clear;
                bg.transform.Find("Checkmark").gameObject.GetComponent<Image>().color = Color.clear;
            }
            catch (Exception) { }
        }

        public static void EnableTimeUI(TextMeshProUGUI phaseToEnable, Toggle timeToggle, string enabledText, bool chooseThisTime = true)
        {
            try {
                timeToggle.enabled = true;
                if (chooseThisTime) timeToggle.isOn = true;
                phaseToEnable.transform.gameObject.SetActive(true);
                var bg = timeToggle.transform.Find("Background").gameObject;
                bg.GetComponent<Image>().color = Color.white;
                bg.transform.Find("Checkmark").gameObject.GetComponent<Image>().color = Color.white;
                phaseToEnable.text = enabledText;
            }
            catch (Exception) { }
        }

        public static void SetRaidTime(float daylightCycleRate)
        {
            if (!Singleton<GameWorld>.Instantiated) {
                throw new Exception("Utils.SetRaidTime was called when the GameWorld instance was not yet instantiated!");
            }

            DateTime dateTime;

            dateTime = Settings.GetCurrentGameTime();

            var gameDateTimeInst = Singleton<GameWorld>.Instance.GameDateTime;
            gameDateTimeInst.Reset(DateTime.Now, dateTime, daylightCycleRate);
        }
    }
}
