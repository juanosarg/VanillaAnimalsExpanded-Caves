using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using RimWorld;

namespace VAECaves
{
    public class IncidentWorker_Hulk : IncidentWorker
    {
        private readonly IntRange scarabAnimalsCount = new IntRange(2, 6);
        private readonly IntRange spelopedeAnimalsCount = new IntRange(1, 3);
        private readonly IntRange megaspiderAnimalsCount = new IntRange(1, 3);

        protected override bool CanFireNowSub(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            PawnKindDef pawnKindDef;
            IntVec3 intVec;
            bool JellyFlag = true;
            System.Random rand = new System.Random();
            int numberOfJelly = map.resourceCounter.GetCount(ThingDefOf.InsectJelly);
            if (numberOfJelly < 100)
            {
                if (rand.NextDouble() < 0.5)
                {
                    JellyFlag = false;
                }
            }

            
            return this.TryFindAnimalKind(map.Tile, out pawnKindDef) && this.TryFindEntryCell(map, out intVec) && JellyFlag && !VAECaves_Mod.settings.insectoidHulkDisabled;
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            PawnKindDef pawnKindDef;
            if (!this.TryFindAnimalKind(map.Tile, out pawnKindDef))
            {
                return false;
            }
            IntVec3 intVec;
          
            if (!this.TryFindEntryCell(map, out intVec))
            {
                return false;
            }
            Rot4 rot = Rot4.FromAngleFlat((map.Center - intVec).AngleFlat);
            PawnGenerationRequest request = new PawnGenerationRequest(pawnKindDef, null, PawnGenerationContext.NonPlayer);
            Pawn hulk = PawnGenerator.GeneratePawn(request);
            GenSpawn.Spawn(hulk, intVec, map, rot, WipeMode.Vanish, false);
            hulk.health.AddHediff(HediffDef.Named("VAE_WallBreaker"));
            hulk.mindState.mentalStateHandler.TryStartMentalState(DefDatabase<MentalStateDef>.GetNamed("Manhunter", true), null, true, false, null, false);
            Find.LetterStack.ReceiveLetter("VAE_LetterLabelInsectoidHulk".Translate(), "VAE_LetterInsectoidHulk".Translate(), LetterDefOf.ThreatBig, hulk, null, null);

            int numScarabs = this.scarabAnimalsCount.RandomInRange;
            for (int i = 0; i < numScarabs; i++)
            {
                request = new PawnGenerationRequest(PawnKindDef.Named("Megascarab"), null, PawnGenerationContext.NonPlayer);
                Pawn scarab = PawnGenerator.GeneratePawn(request);
                GenSpawn.Spawn(scarab, CellFinder.RandomClosewalkCellNear(hulk.Position, hulk.Map, 5, null), map, rot, WipeMode.Vanish, false);
                scarab.mindState.mentalStateHandler.TryStartMentalState(DefDatabase<MentalStateDef>.GetNamed("Manhunter", true), null, true, false, null, false);

            }
            int numSpelopedes = this.spelopedeAnimalsCount.RandomInRange;
            for (int i = 0; i < numSpelopedes; i++)
            {
                request = new PawnGenerationRequest(PawnKindDef.Named("Spelopede"), null, PawnGenerationContext.NonPlayer);
                Pawn spelopede = PawnGenerator.GeneratePawn(request);
                GenSpawn.Spawn(spelopede, CellFinder.RandomClosewalkCellNear(hulk.Position, hulk.Map, 5, null), map, rot, WipeMode.Vanish, false);
                spelopede.mindState.mentalStateHandler.TryStartMentalState(DefDatabase<MentalStateDef>.GetNamed("Manhunter", true), null, true, false, null, false);

            }
            int numMegaspiders = this.megaspiderAnimalsCount.RandomInRange;
            for (int i = 0; i < numMegaspiders; i++)
            {
                request = new PawnGenerationRequest(PawnKindDef.Named("Megaspider"), null, PawnGenerationContext.NonPlayer);
                Pawn megaspider = PawnGenerator.GeneratePawn(request);
                GenSpawn.Spawn(megaspider, CellFinder.RandomClosewalkCellNear(hulk.Position, hulk.Map, 5, null), map, rot, WipeMode.Vanish, false);
                megaspider.mindState.mentalStateHandler.TryStartMentalState(DefDatabase<MentalStateDef>.GetNamed("Manhunter", true), null, true, false, null, false);

            }
           

            return true;
        }

        private bool TryFindAnimalKind(int tile, out PawnKindDef animalKind)
        {
            return (from k in DefDatabase<PawnKindDef>.AllDefs
                    where Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(tile, ThingDef.Named("VAECaves_InsectoidHulk")) && k.defName == "VAECaves_InsectoidHulk"
                    select k).TryRandomElement(out animalKind);
        }

        private bool TryFindEntryCell(Map map, out IntVec3 cell)
        {
            return RCellFinder.TryFindRandomPawnEntryCell(out cell, map, CellFinder.EdgeRoadChance_Animal + 0.2f);
        }

       
    }
}
