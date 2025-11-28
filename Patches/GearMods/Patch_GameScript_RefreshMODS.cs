using GadgetCore.API;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeleeRangePlus.Patches.GearMods
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch(nameof(GameScript.RefreshMODS))]
    [HarmonyGadget(nameof(MeleeRangePlus))]
    public static class Patch_GameScript_RefreshMODS
    {
        [HarmonyPrefix]
        public static bool Prefix(Item[] ___inventory) // replaces the vanilla method
        {
            for (int i = 0; i < 25; i++)
            {
                GameScript.MODS[i] = 0;
            }
            MeleeRangePlus.CurrentGearModCount = 0;
            for (int i = 36; i < 42; i++) // equipped gear
            {
                if (___inventory[i].id > 0) // there's an item equipped
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (___inventory[i].aspectLvl[j] > 0) // there's a gear mod
                        {
                            if (ItemRegistry.Singleton.TryGetEntry(___inventory[i].aspect[j], out ItemInfo itemInfo))
                            {
                                if (itemInfo == MeleeRangePlus.GearModItem)
                                    MeleeRangePlus.CurrentGearModCount += ___inventory[i].aspectLvl[j];
                            }
                            else
                            {
                                GameScript.MODS[___inventory[i].aspect[j] - 200] += ___inventory[i].aspectLvl[j];
                            }
                        }
                    }
                }
            }
            //MeleeRangePlus.GetReferencesAndApplyGearMods();
            return false;
        }
    }
}
