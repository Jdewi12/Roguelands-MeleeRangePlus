using GadgetCore.API;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeleeRangePlus.Patches.GearMods
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch(nameof(GameScript.GetGearAspect))]
    [HarmonyGadget(nameof(MeleeRangePlus))]
    public static class Patch_GameScript_GetGearAspect
    {
        [HarmonyPostfix]
        public static void Postfix(int id, ref string __result)
        {
            if (ItemRegistry.Singleton.TryGetEntry(id + 200, out ItemInfo itemInfo))
            {
                __result = itemInfo.GetName();
            }
        }
    }
}
