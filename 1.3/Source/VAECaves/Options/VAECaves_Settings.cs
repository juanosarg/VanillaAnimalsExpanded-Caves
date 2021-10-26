using RimWorld;
using UnityEngine;
using Verse;
using System.Collections.Generic;
using System.Linq;
using System;


namespace VAECaves
{
   

    public class VAECaves_Settings : ModSettings

    {


        public const float animalSpawnMultiplierBase = 1;
        public float animalSpawnMultiplier = animalSpawnMultiplierBase;
        public bool arachnophobiaMode = false;
        public bool insectoidHulkDisabled = false;


        public override void ExposeData()
        {
            base.ExposeData();
            
            Scribe_Values.Look(ref animalSpawnMultiplier, "animalSpawnMultiplier", 1, true);
            Scribe_Values.Look(ref arachnophobiaMode, "arachnophobiaMode", false, true);
            Scribe_Values.Look(ref insectoidHulkDisabled, "insectoidHulkDisabled", false, true);


        }

        public void DoWindowContents(Rect inRect)
        {
            Listing_Standard ls = new Listing_Standard();


            ls.Begin(inRect);
            ls.Gap(10f);


            ls.Label("VAE_AnimalSpawnMultiplier".Translate() + ": " + animalSpawnMultiplier, -1, "VAE_AnimalSpawnMultiplierTooltip".Translate());
            animalSpawnMultiplier = (float)Math.Round(ls.Slider(animalSpawnMultiplier, 0.1f, 5f), 2);

            if (ls.Settings_Button("VAE_Reset".Translate(), new Rect(0f, ls.CurHeight, 180f, 29f)))
            {
                animalSpawnMultiplier = animalSpawnMultiplierBase;
            }
            ls.Gap(26f);
            ls.CheckboxLabeled("VAE_ArachnophobiaMode".Translate(), ref arachnophobiaMode, null);
            ls.Gap(26f);
            ls.CheckboxLabeled("VAE_InsectoidHulkDisabled".Translate(), ref insectoidHulkDisabled, null);


            ls.End();
        }



    }










}
