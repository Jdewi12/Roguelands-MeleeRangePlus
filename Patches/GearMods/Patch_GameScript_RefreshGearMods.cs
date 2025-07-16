using GadgetCore.API;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MeleeRangePlus.Patches.GearMods
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch(nameof(GameScript.RefreshGearMods))]
    [HarmonyGadget(nameof(MeleeRangePlus))]
    public static class Patch_GameScript_RefreshGearMods
    {
        public static void Postfix(Item[] ___modSlot, TextMesh[] ___txtMods)
        {
            int id = ___modSlot[0].id;
            if (id > 0) // there's an item here
            {
                for (int i = 0; i < 3; i++)
                {

                    if (ItemRegistry.Singleton.TryGetEntry(___modSlot[0].aspect[i], out ItemInfo itemInfo) && ___modSlot[0].aspectLvl[i] > 0)
                    {
                        ___txtMods[i].text = itemInfo.Name + " " + ___modSlot[0].aspectLvl[i];
                        ___txtMods[i].color = Color.yellow;
                    }
                }
            }
        }

    }
}
