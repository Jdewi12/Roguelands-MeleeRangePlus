/*using GadgetCore.API;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MeleeRangePlus.Patches
{
    [HarmonyPatch(typeof(PlayerScript))]
    [HarmonyPatch(nameof(PlayerScript.Awake))]
    [HarmonyGadget(nameof(MeleeRangePlus))]
    public static class Patch_PlayerScript_Awake
    {
        [HarmonyPostfix]
        public static void Postfix(PlayerScript __instance)
        {
            __instance.StartCoroutine(Delay(0.01f));
        }

        static IEnumerator Delay(float f)
        {
            yield return new WaitForSeconds(f);
            MeleeRangePlus.Log("GetRefGearetc.");
            
        }
    }
}*/
