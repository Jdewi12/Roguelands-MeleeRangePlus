using GadgetCore.API;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace MeleeRangePlus.Patches
{
    [HarmonyPatch()]
    [HarmonyGadget(nameof(MeleeRangePlus))]
    public static class Patch_RuneChalice_InitializeItem_MoveNext
    {
        static Type iteratorType = typeof(RuneChalice).GetNestedType("<InitializeItems>c__Iterator0", BindingFlags.NonPublic);
        [HarmonyTargetMethod]
        public static MethodBase TargetMethod()
        {
            return iteratorType.GetMethod("MoveNext", BindingFlags.Public | BindingFlags.Instance);
        }

        [HarmonyPostfix]
        public static void Postfix(object __instance)
        {
            float chance = GameScript.challengeLevel * 2 / 100f / 20f; // chance of mod multiplied by chance of this specific mod
            var chalice = (RuneChalice)DThis.GetValue(__instance);
            for (int i = 0; i < 5; i++)
            {
                if (UnityEngine.Random.Range(0f, 1f) < chance)
                {
                    int[] itemsArray = (int[])items.GetValue(chalice);
                    itemsArray[i] = MeleeRangePlus.GearModItem.GetID();
                    if (i == 0)
                    {
                        chalice.txtValue[0].text = "+" + MeleeRangePlus.GearModItem.Name;
                        chalice.txtValue[1].text = chalice.txtValue[0].text;
                    }
                }
            }
        }

        static FieldInfo DThis = iteratorType.GetField("$this", BindingFlags.NonPublic | BindingFlags.Instance);
        static FieldInfo items = typeof(RuneChalice).GetField("items", BindingFlags.NonPublic | BindingFlags.Instance);
    }
}
