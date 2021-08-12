using RimWorld;
using System;
using Verse;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VAECaves
{
    public class DamageWorker_CocoonRanged : DamageWorker_AddInjury
    {
        public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
        {
           
            Pawn pawn = victim as Pawn;
            if (pawn != null &&pawn.def.defName!= "VAECaves_AncientGiantSpider" && pawn.def.defName != "VAECaves_GiantSpiderHatchling" && pawn.def.defName != "VAECaves_GiantSpider")
            {
                Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(DefDatabase<HediffDef>.GetNamed("VAE_SilkCovered"));
                if (pawn.stances.stunner.Stunned || hediff != null)
                {
                    TryCoverPawn(pawn);
                    
                } 
            }

            DamageWorker.DamageResult damageResult = base.Apply(dinfo, victim);


            return damageResult;
        }

        public void TryCoverPawn(Pawn pawn)
        {
            Thing cocoon = ThingMaker.MakeThing(ThingDef.Named("VAECaves_Cocoon"), null);
            GenSpawn.Spawn(cocoon, pawn.Position, pawn.Map, WipeMode.FullRefund);
            Building_Cocoon cocoonAsSarcophagus = cocoon as Building_Cocoon;
            cocoonAsSarcophagus.TryAcceptThing(pawn);
        }
    }

    
}

