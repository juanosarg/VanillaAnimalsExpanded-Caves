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
using RimWorld.Planet;



namespace VAECaves
{


    [HarmonyPatch(typeof(TrainableUtility))]
    [HarmonyPatch("TamenessCanDecay")]

    public static class VAECaves_TrainableUtility_TamenessCanDecay_Patch
    {
        [HarmonyPrefix]
        public static bool RemoveTamenessDecayCheck(ThingDef def)

        {
            if ((def.defName== "VAECaves_GiantSpider"))
            {
                return false;

            }
            else return true;
        }
    }


}
