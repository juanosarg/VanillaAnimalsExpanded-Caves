using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;
using RimWorld.Planet;

namespace VAECaves
{
    public class CocoonsAndSpiderLairs_MapComponent : MapComponent
    {

        //This class receives calls when a new object appears on the map, storing or deleting it from a List
        //This List is used on WorkGivers. They'll only look for things on the List, causing less lag

        public HashSet<Thing> objects_InMap = new HashSet<Thing>();
        public HashSet<Thing> spiderLairsInMap = new HashSet<Thing>();


        public override void ExposeData()
        {
            //Save all the key variables so they work on game save / load
            base.ExposeData();
            Scribe_Collections.Look<Thing>(ref this.objects_InMap, true, "objects_InMap",LookMode.Reference);
            Scribe_Collections.Look<Thing>(ref this.spiderLairsInMap, true, "spiderLairsInMap", LookMode.Reference);

        }
        public CocoonsAndSpiderLairs_MapComponent(Map map) : base(map)
        {

        }

        public override void FinalizeInit()
        {

            base.FinalizeInit();

        }

        public void AddObjectToMap(Thing thing)
        {
            if (!objects_InMap.Contains(thing))
            {
                objects_InMap.Add(thing);
            }

        }

        public void RemoveObjectFromMap(Thing thing)
        {
            if (objects_InMap.Contains(thing))
            {
                objects_InMap.Remove(thing);
            }

        }

        public void AddSpiderLairBuilt(Thing thing)
        {
            if (!spiderLairsInMap.Contains(thing))
            {
                spiderLairsInMap.Add(thing);
            }
        }

        public void RemoveSpiderLairBuilt(Thing thing)
        {
            if (spiderLairsInMap.Contains(thing))
            {
                spiderLairsInMap.Remove(thing);
            }

        }


    }


}
