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
    [HarmonyPatch(nameof(GameScript.UseItem))]
    [HarmonyGadget(nameof(MeleeRangePlus))]
    public static class Patch_GameScript_UseItem
    {
        public static void Prefix()
        {
            MeleeRangePlus.GetReferencesAndApplyGearMods();
            // Because we've messed something up with the animation when we changed the parent
            //MeleeRangePlus.HeldAppearanceParent.localScale = Vector3.one / 2f;
            MeleeRangePlus.HeldAppearanceParent.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
    }
}
