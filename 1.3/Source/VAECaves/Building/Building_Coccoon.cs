using System;
using RimWorld;
using Verse;
using System.Collections.Generic;
using Verse.Sound;

namespace VAECaves
{
    public class Building_Coccoon : Building, IThingHolder
    {

        public ThingOwner innerContainer = null;
        protected bool contentsKnown;
        public int tickCounter = 0;
      
        public Building_Coccoon()
        {
            //Constructor initializes the building container
            this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);

        }

       
        public override void Tick()
        {
            base.Tick();
            this.innerContainer.ThingOwnerTick(true);
        }
        public override void ExposeData()
        {
            //Save all the key variables so they work on game save / load
            base.ExposeData();
            Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[] { this });
        }
        public ThingOwner GetDirectlyHeldThings()
        {
            //Not used, included just in case something external calls it           
            return this.innerContainer;
        }

        public void GetChildHolders(List<IThingHolder> outChildren)
        {
            //Not used, included just in case something external calls it
            ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
        }

        public virtual void EjectContents()
        {
            //Remove ingredients from the container. 
            if (this.Map != null)
            {
                this.innerContainer.TryDropAll(this.Position, base.Map, ThingPlaceMode.Near, null, null);
            }
        }

        public void DestroyContents()
        {
            //Empties all containers and destroys contents

            if (this.innerContainer != null && this.innerContainer.Any)
            {
                this.innerContainer.ClearAndDestroyContents();
            }
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {

            if (this.Map != null)
            {
                EjectContents();
                for (int i = 0; i < 20; i++)
                {
                    IntVec3 c;
                    CellFinder.TryFindRandomReachableCellNear(this.Position, this.Map, 2, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), null, null, out c);
                    FilthMaker.TryMakeFilth(c, this.Map, ThingDefOf.Filth_Slime);

                }
                SoundDefOf.Hive_Spawn.PlayOneShot(new TargetInfo(this.Position, this.Map, false));
            }

            base.Destroy(mode);
        }

        public override void Kill(DamageInfo? dinfo, Hediff exactCulprit = null)
        {
            if (this.Map != null)
            {
                EjectContents();
                for (int i = 0; i < 20; i++)
                {
                    IntVec3 c;
                    CellFinder.TryFindRandomReachableCellNear(this.Position, this.Map, 2, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), null, null, out c);
                    FilthMaker.TryMakeFilth(c, this.Map, ThingDefOf.Filth_Slime);

                }
                SoundDefOf.Hive_Spawn.PlayOneShot(new TargetInfo(this.Position, this.Map, false));
            }

            base.Kill(dinfo, exactCulprit);
        }

        public virtual bool Accepts(Thing thing)
        {
            return this.innerContainer.CanAcceptAnyOf(thing, true);
        }

        public virtual bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
        {
            if (!this.Accepts(thing))
            {
                return false;
            }
            bool flag;
            if (thing.holdingOwner != null)
            {
                thing.holdingOwner.Remove(thing);
                this.innerContainer.TryAdd(thing, thing.stackCount, false);
                flag = true;
            }
            else
            {
                flag = this.innerContainer.TryAdd(thing, true);
            }
            if (flag)
            {
                if (thing.Faction != null && thing.Faction.IsPlayer)
                {
                    this.contentsKnown = true;
                }
                return true;
            }
            return false;
        }

        

        public override string GetInspectString()
        {
            string stomachContents = "";

            

            return base.GetInspectString() + stomachContents;
        }




    }
}
