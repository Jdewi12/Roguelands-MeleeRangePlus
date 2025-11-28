using GadgetCore.API;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MeleeRangePlus.Patches
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch(nameof(GameScript.EnterCombatMode))]
    [HarmonyGadget(nameof(MeleeRangePlus))]
    public static class Patch_GameScript_EnterCombatMode
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            MeleeRangePlus.GetReferencesAndApplyGearMods();
            float scaleBonus = MeleeRangePlus.CurrentGearModCount * 0.04f;
            MeleeRangePlus.HeldAppearanceParent.localScale = Vector3.one * (1f + scaleBonus);
            MeleeRangePlus.HeldAppearanceParent.transform.localPosition = new Vector3(0f, 0f, 0f);// + Vector3.down * 0.35f * scaleBonus;
        }
    }
}
