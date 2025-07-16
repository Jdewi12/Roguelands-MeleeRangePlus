using GadgetCore.API;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeleeRangePlus.Patches
{
    [HarmonyPatch(typeof(ItemStandScript))]
    [HarmonyPatch(nameof(ItemStandScript.GetItem))]
    [HarmonyGadget(nameof(MeleeRangePlus))]
    public static class Patch_ItemStandScript_GetItem
    {
        [HarmonyPostfix]
        public static void PostFix(ref int __result, ItemStandScript __instance)
        {
            if (__instance.isAdvanced)
                return;
            if (GameScript.challengeLevel <= 0)
                return;
            float chance = (0.05f + GameScript.challengeLevel * 0.1f) / 20f; // chance of mod multiplied by chance of this specific mod
            if (UnityEngine.Random.Range(0f, 1f) < chance)
            {
                MeleeRangePlus.Log("Replaced itemstand item");
                __result = MeleeRangePlus.GearModItem.GetID();
            }
        }
    }
    [HarmonyPatch(typeof(ItemStandScript))]
    [HarmonyPatch(nameof(ItemStandScript.GetItemCost))]
    [HarmonyGadget(nameof(MeleeRangePlus))]
    public static class Patch_ItemStandScript_GetItemCost
    {
        [HarmonyPrefix]
        public static bool Prefix(int id, ref int __result)
        {
            if (id == MeleeRangePlus.GearModItem.GetID())
            {
                __result = 5000;
                return false;
            }
            return true;
        }
    }
}
