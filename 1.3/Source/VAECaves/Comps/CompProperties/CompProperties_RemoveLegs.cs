
using Verse;

namespace VAECaves
{
    public class CompProperties_RemoveLegs : CompProperties
    {

        public int checkInterval = 500;

        public CompProperties_RemoveLegs()
        {
            this.compClass = typeof(CompRemoveLegs);
        }


    }
}