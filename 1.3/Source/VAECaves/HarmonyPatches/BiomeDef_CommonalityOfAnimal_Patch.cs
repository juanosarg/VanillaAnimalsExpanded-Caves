using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using System.Reflection.Emit;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Verse.AI;

namespace VAECaves
{
    /*This Harmony Postfix multiplies commonality of animals in the biome
    */
    [HarmonyPatch(typeof(BiomeDef))]
    [HarmonyPatch("CommonalityOfAnimal")]
    public static class VAECaves_BiomeDef_CommonalityOfAnimal_Patch
    {
        [HarmonyPostfix]
        public static void MultiplyVAECavesCommonality(PawnKindDef animalDef, ref float __result)

        {

            if (animalDef.defName.Contains("VAECaves_"))
            {
                __result *= VAECaves_Mod.settings.animalSpawnMultiplier;

            }


        }
    }
}
