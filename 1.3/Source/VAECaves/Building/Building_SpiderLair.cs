using System;
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;

using Verse.Sound;
using UnityEngine;

namespace VAECaves
{
    public class Building_SpiderLair : Building
    {

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            CocoonsAndSpiderLairs_MapComponent mapComp = this.Map.GetComponent<CocoonsAndSpiderLairs_MapComponent>();
            if (mapComp != null)
            {
                mapComp.AddSpiderLairBuilt(this);

            }
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {         
            CocoonsAndSpiderLairs_MapComponent mapComp = this.Map.GetComponent<CocoonsAndSpiderLairs_MapComponent>();
            if (mapComp != null)
            {
                mapComp.RemoveSpiderLairBuilt(this);
            }
            MakeAncientsMad();
            base.Destroy(mode);
        }

        public override void Kill(DamageInfo? dinfo, Hediff exactCulprit = null)
        {
            CocoonsAndSpiderLairs_MapComponent mapComp = this.Map.GetComponent<CocoonsAndSpiderLairs_MapComponent>();
            if (mapComp != null)
            {
                mapComp.RemoveSpiderLairBuilt(this);
            }
            MakeAncientsMad();
            base.Kill(dinfo, exactCulprit);
        }
        public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            base.PostApplyDamage(dinfo, totalDamageDealt);
            if(dinfo.Instigator.Faction!=null && dinfo.Instigator.Faction.IsPlayer)
            {
                MakeAncientsMad();
            }
        }

        public void MakeAncientsMad()
        {
            List<Pawn> pawnsaffected = (from x in this.Map.mapPawns.AllPawnsSpawned
                                        where x.kindDef == PawnKindDef.Named("VAECaves_AncientGiantSpider")
                                        select x).ToList();
            foreach (Pawn pawnaffected in pawnsaffected)
            {
                pawnaffected.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, null, false, false, false);
            }

        }



    }
}
