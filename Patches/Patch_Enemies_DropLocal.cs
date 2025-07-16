using GadgetCore.API;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace MeleeRangePlus.Patches
{
    [HarmonyPatch()]
    [HarmonyGadget(nameof(MeleeRangePlus))]
    public static class Patch_Enemies_DropLocal
    {
        [HarmonyTargetMethods]
        public static IEnumerable<MethodBase> TargetMethods()
        {
            List<Type> types = new List<Type>()
            {
                typeof(Destroyer),
                typeof(DestroyerTrue),
                typeof(EnemyScript),
                typeof(Hivemind),
                typeof(Millipede),
                typeof(ScarabScript),
                typeof(SliverScript),
                typeof(WormScript)
            };
            foreach (var type in types)
            {
                yield return type.GetMethod("DropLocal", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance); ;
            }
            yield return typeof(ObjectScript).GetNestedType("<DropItemLocal>c__Iterator1", BindingFlags.NonPublic)
                .GetMethod("MoveNext", BindingFlags.Public | BindingFlags.Instance);
        }

        public static MethodInfo RandomReplacementMethod = typeof(Patch_Enemies_DropLocal).GetMethod(nameof(RandomReplacement), BindingFlags.Public | BindingFlags.Static);

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase __originalMethod)
        {
            var codes = instructions.ToList();
            bool success = false;
            for (int i = 2; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Call && codes[i].operand.ToString() == "Int32 Range(Int32, Int32)"
                    && codes[i - 1].opcode == OpCodes.Ldc_I4
                    && codes[i - 2].opcode == OpCodes.Ldc_I4 && codes[i - 2].operand.ToString() == "201") // start of gear mod ids
                {
                    success = true;
                    codes[i].operand = RandomReplacementMethod; // call our method with the same parameters instead of Random.Range
                }
            }
            if (!success)
                MeleeRangePlus.logger.LogError("failed to patch drop of " + __originalMethod.ToString());
            return codes;
        }

        // Adds an additional chance to spawn our custom gear mod
        public static int RandomReplacement(int minInclusive, int maxExclusive)
        {
            var rng = UnityEngine.Random.Range(minInclusive, maxExclusive + 1);
            if (rng == maxExclusive)
                return MeleeRangePlus.GearModItem.GetID();
            return rng;
        }
    }
}
