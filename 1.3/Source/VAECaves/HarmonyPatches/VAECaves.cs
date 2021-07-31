using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using HarmonyLib;

namespace VAECaves
{

    public class VAECaves : Mod
    {
        public VAECaves(ModContentPack content) : base(content)
        {
            HarmonyInstance = new Harmony("OskarPotocki.VAECaves");
        }

        public static Harmony HarmonyInstance;

    }

}
