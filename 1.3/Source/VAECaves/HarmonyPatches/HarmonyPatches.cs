using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;

namespace VAECaves
{

    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {

        static HarmonyPatches()
        {
            // SpawnRandomWildAnimalAt's first predicate
            VAECaves.HarmonyInstance.Patch(typeof(WildAnimalSpawner).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).
                First(m => m.HasAttribute<CompilerGeneratedAttribute>() && m.Name.Contains("SpawnRandomWildAnimalAt") && m.ReturnType == typeof(bool)),
                transpiler: new HarmonyMethod(typeof(Patch_WildAnimalSpawner.manual_SpawnWildAnimalAt_predicate), "Transpiler"));
        }

    }

}
