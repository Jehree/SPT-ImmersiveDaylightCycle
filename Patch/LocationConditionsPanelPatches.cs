﻿using Aki.Reflection.Patching;
using Comfort.Common;
using EFT;
using EFT.UI.Matchmaker;
using HarmonyLib;
using Jehree.ImmersiveDaylightCycle.Helpers;
using JsonType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Jehree.ImmersiveDaylightCycle.Patch {
    internal class TimeUIPanelPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(LocationConditionsPanel), nameof(LocationConditionsPanel.Set));
        }

        [PatchPostfix]
        static void Postfix(
            RaidSettings raidSettings,
            bool takeFromCurrent,
            ref TextMeshProUGUI ____currentPhaseTime,
            ref TextMeshProUGUI ____nextPhaseTime,
            ref Toggle ____pmTimeToggle,
            ref Toggle ____amTimeToggle
        )
        {
            if (!Settings.modEnabled.Value) {
                Utils.EnableTimeUI(____nextPhaseTime, ____pmTimeToggle, "03:28:00");
                Utils.EnableTimeUI(____currentPhaseTime, ____amTimeToggle, "15:28:00");
                return;
            }

            DateTime dateTime = Settings.GetCurrentGameTime();

            if (raidSettings.SelectedLocation.Id == "factory4_day" || raidSettings.SelectedLocation.Id == "factory4_night") {

                if (Utils.IsDayTime(dateTime)) {
                    Utils.DisableTimeUI(____nextPhaseTime, ____pmTimeToggle);
                    Utils.EnableTimeUI(____currentPhaseTime, ____amTimeToggle, "15:28:00");
                } else {
                    Utils.DisableTimeUI(____currentPhaseTime, ____amTimeToggle);
                    Utils.EnableTimeUI(____nextPhaseTime, ____pmTimeToggle, "03:28:00");
                }
                return;
            }

            Utils.DisableTimeUI(____nextPhaseTime, ____pmTimeToggle);
            Utils.EnableTimeUI(____currentPhaseTime, ____amTimeToggle, dateTime.ToString("HH:mm:ss"));
#if DEBUG
            Plugin.LogSource.LogError(raidSettings.SelectedLocation.Id);
#endif
        }
    }

    internal class TimeUIUpdatePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(LocationConditionsPanel), nameof(LocationConditionsPanel.Update));
        }

        [PatchPrefix]
        static bool Prefix()
        {
            if (!Settings.modEnabled.Value) return true;
            return false;
        }
    }

    internal class LocationConditionsPanelPatch : ModulePatch
    {

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.FirstMethod(typeof(LocationConditionsPanel), x => x.Name == nameof(LocationConditionsPanel.Set) && x.GetParameters()[0].Name == "session");
        }

        [PatchPostfix]
        static void Postfix(RaidSettings raidSettings, bool takeFromCurrent, MatchMakerAcceptScreen __instance)
        {
            if (!Settings.modEnabled.Value) return;

            DateTime dateTime = Settings.GetCurrentGameTime();

            TextMeshProUGUI timePanel;

            try {
                timePanel = __instance.transform.Find("TimePanel").gameObject.transform.Find("Time").gameObject.GetComponent<TextMeshProUGUI>();
            }
            catch (Exception) { return; }

            if (raidSettings.SelectedLocation.Id == "factory4_day" || raidSettings.SelectedLocation.Id == "factory4_night") {

                if (Utils.IsDayTime(dateTime)) {
                    SetTimePanelText(timePanel, "15:28:00");
                }
                else {
                    SetTimePanelText(timePanel, "03:28:00");
                }
                return;
            }

            SetTimePanelText(timePanel, Settings.GetCurrentGameTime().ToString("HH:mm:ss"));
        }

        static void SetTimePanelText(TextMeshProUGUI timePanel, string text)
        {
            try {
                timePanel.text = text;
            } catch(Exception) { }
        }
    }
}
