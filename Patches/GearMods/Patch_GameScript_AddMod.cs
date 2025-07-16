using GadgetCore.API;
using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace MeleeRangePlus.Patches.GearMods
{
    [HarmonyPatch(typeof(GameScript))]
    [HarmonyPatch(nameof(GameScript.AddMod))]
    [HarmonyGadget(nameof(MeleeRangePlus))]
    public static class Patch_GameScript_AddMod
    {
        [HarmonyPrefix]
        public static bool Prefix(ref int a, GameScript __instance, Item ___holdingItem, Item[] ___modSlot, GameObject[] ___modObj)
        {
            // a is the slot being clicked on
            if (!ItemRegistry.Singleton.TryGetEntry(___holdingItem.id, out ItemInfo holdingInfo)) // not a modded item
                return true; // run original method

            if (holdingInfo.Type != ItemType.MOD)
                return false;
            Item toMod = ___modSlot[0];
            if (toMod.id <= 0) // no item to be modded
                return false;
            int maxMods = 5;
            if (!holdingInfo.Name.EndsWith("StackSize+")) // Subworlds' StackSize+ shouldn't go beyond 5
            {
                for (int i = 0; i < 3; i++)
                {
                    if (toMod.aspect[i] > 0 && ItemRegistry.Singleton.TryGetEntry(toMod.aspect[i], out ItemInfo aspectInfo) && aspectInfo.Name.EndsWith("StackSize+"))
                    {
                        maxMods += toMod.aspectLvl[i];
                    }
                }
            }
            if (toMod.aspectLvl[a] >= maxMods) // slot's mods already maxed out
                return false;
            for (int i = 0; i < 3; i++)
            {
                // if the mod is already in a different slot
                if (i != a && ___modSlot[0].aspect[i] == ___holdingItem.id)
                {
                    return false;
                }
            }
            // if there's already a different mod in this slot
            if (toMod.aspect[a] != 0 && toMod.aspect[a] != ___holdingItem.id)
                return false;

            toMod.aspectLvl[a]++;
            toMod.aspect[a] = ___holdingItem.id;
            ___holdingItem.q--;
            __instance.RefreshHoldingSlot();
            __instance.RefreshGearMods();
            ___modObj[a].GetComponent<Animation>().Play();
            __instance.GetComponent<AudioSource>().PlayOneShot((AudioClip)Resources.Load("Au/addmod"), Menuu.soundLevel / 10f);

            return false;
        }
    }
}