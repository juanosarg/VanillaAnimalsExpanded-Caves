using System;
using RimWorld;
using Verse;
using System.Collections.Generic;
using Verse.Sound;
using UnityEngine;

namespace VAECaves
{
    public class Building_Cocoon : Building, IThingHolder
    {

        public ThingOwner innerContainer = null;
        protected bool contentsKnown;
        public bool scheduledToOpen = false;
        public int tickCounter = 0;

        public int ticksToDown = 2500;
        public int ticksToDeath = 3750;

        public Building_Cocoon()
        {
            //Constructor initializes the building container
            this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);

        }


        public override void Tick()
        {
            base.Tick();
            tickCounter++;
        }
        public override void ExposeData()
        {
            //Save all the key variables so they work on game save / load
            base.ExposeData();
            Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[] { this });
            Scribe_Values.Look<bool>(ref this.scheduledToOpen, "scheduledToOpen", false, false);
            Scribe_Values.Look<int>(ref this.tickCounter, "tickCounter", 0, false);


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
                Pawn pawn = this.innerContainer.FirstOrFallback() as Pawn;
                if (pawn != null)
                {
                    if (tickCounter > this.ticksToDeath)
                    {
                        pawn.Kill(null);
                    } else if (tickCounter > this.ticksToDown)
                    {
                        HealthUtility.DamageUntilDowned(pawn);
                    }

                }

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
                    FilthMaker.TryMakeFilth(c, this.Map, ThingDef.Named("VAECaves_Filth_Webs"));

                }
                SoundDefOf.Hive_Spawn.PlayOneShot(new TargetInfo(this.Position, this.Map, false));
            }
            CocoonsAndSpiderLairs_MapComponent mapComp = this.Map.GetComponent<CocoonsAndSpiderLairs_MapComponent>();
            if (mapComp != null)
            {
                mapComp.RemoveObjectFromMap(this);
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
                    FilthMaker.TryMakeFilth(c, this.Map, ThingDef.Named("VAECaves_Filth_Webs"));

                }
                SoundDefOf.Hive_Spawn.PlayOneShot(new TargetInfo(this.Position, this.Map, false));
            }
            CocoonsAndSpiderLairs_MapComponent mapComp = this.Map.GetComponent<CocoonsAndSpiderLairs_MapComponent>();
            if (mapComp != null)
            {
                mapComp.RemoveObjectFromMap(this);
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
            if (!scheduledToOpen) {
                if (tickCounter > this.ticksToDeath)
                {
                    return "VAE_CocoonDeath".Translate();
                }
                else if (tickCounter > this.ticksToDown)
                {
                    return "VAE_CocoonCritical".Translate();
                }
                else
                {
                    return "VAE_CocoonHurt".Translate();
                }
            } else {
                return "VAE_Scheduled".Translate();
            }

          
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in base.GetGizmos())
            {
                yield return gizmo;
            }
            if (!scheduledToOpen)
            {
                yield return new Command_Action
                {
                    defaultLabel = "VAE_OpenCocoon".Translate(),
                    defaultDesc = "VAE_OpenCocoonDesc".Translate(),
                    icon = ContentFinder<Texture2D>.Get("UI/VAE_RemoveCocoon", true),
                    action = delegate ()
                    {
                        CocoonsAndSpiderLairs_MapComponent mapComp = this.Map.GetComponent<CocoonsAndSpiderLairs_MapComponent>();
                        if (mapComp != null)
                        {
                            mapComp.AddObjectToMap(this);
                            scheduledToOpen = true;
                        }
                    }
                };
            }
            yield break;
        }


    }
}
