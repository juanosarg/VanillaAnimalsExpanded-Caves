using RimWorld;
using UnityEngine;
using Verse;
using System.Collections.Generic;
using System.Linq;

namespace VAECaves
{
    public class VAECaves_Mod : Mod
    {
        public static VAECaves_Settings settings;

        public VAECaves_Mod(ModContentPack content) : base(content)
        {
            settings = GetSettings<VAECaves_Settings>();
        }
        public override string SettingsCategory() => "Vanilla Animals Expanded - Caves";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            settings.DoWindowContents(inRect);
        }
    }
}
