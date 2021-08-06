using RimWorld;
using System;
using Verse;
using System.Collections.Generic;

namespace VAECaves
{


    public class VAEQuestMapSpawnsDef : Def
    {
        public ThingDef thingDef;
        public bool allowOnWater;
        public IntRange numberToSpawn;
        public bool ifPawnGoManhunter = false;
        public bool isPawn = false;
        public List<string> terrainValidationDisallowed;
        public PawnKindDef associatedMonster;

    }
}

