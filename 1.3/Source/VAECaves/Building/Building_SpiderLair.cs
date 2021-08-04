using System;
using RimWorld;
using Verse;
using System.Collections.Generic;
using Verse.Sound;
using UnityEngine;

namespace VAECaves
{
    public class Building_SpiderLair : Building
    {

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            CoccoonsAndSpiderLairs_MapComponent mapComp = this.Map.GetComponent<CoccoonsAndSpiderLairs_MapComponent>();
            if (mapComp != null)
            {
                mapComp.AddSpiderLairBuilt(this);

            }
        }

       


        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {         
            CoccoonsAndSpiderLairs_MapComponent mapComp = this.Map.GetComponent<CoccoonsAndSpiderLairs_MapComponent>();
            if (mapComp != null)
            {
                mapComp.RemoveSpiderLairBuilt(this);
            }
            base.Destroy(mode);
        }

        public override void Kill(DamageInfo? dinfo, Hediff exactCulprit = null)
        {
            CoccoonsAndSpiderLairs_MapComponent mapComp = this.Map.GetComponent<CoccoonsAndSpiderLairs_MapComponent>();
            if (mapComp != null)
            {
                mapComp.RemoveSpiderLairBuilt(this);
            }
           
            base.Kill(dinfo, exactCulprit);
        }

     

    }
}
