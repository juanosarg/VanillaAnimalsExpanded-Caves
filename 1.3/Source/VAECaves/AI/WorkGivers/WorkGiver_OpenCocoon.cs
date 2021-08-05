﻿using System;
using Verse;
using Verse.AI;
using RimWorld;
using System.Collections.Generic;

namespace VAECaves
{
    public class WorkGiver_OpenCocoon : WorkGiver_Scanner
    {
        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {

            return pawn.Map.GetComponent<CocoonsAndSpiderLairs_MapComponent>().objects_InMap;


        }

        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.Touch;
            }
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Building_Cocoon cocoon = t as Building_Cocoon;
            bool result;
            if (t == null || t.IsBurning()|| !cocoon.scheduledToOpen)
            {
                result = false;
            }

            else
            {

                if (!t.IsForbidden(pawn))
                {
                    LocalTargetInfo target = t;
                    if (pawn.CanReserve(target, 1, -1, null, forced))
                    {
                        result = true;
                        return result;
                    }
                }
                result = false;
            }
            return result;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return new Job(DefDatabase<JobDef>.GetNamed("VAECaves_OpenCocoon", true), t);
        }
    }
}
