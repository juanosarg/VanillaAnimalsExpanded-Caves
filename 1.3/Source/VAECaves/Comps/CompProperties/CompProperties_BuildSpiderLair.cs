
using Verse;

namespace VAECaves
{
    public class CompProperties_BuildSpiderLair : CompProperties
    {
        public int checkingTime = 2000;
        public int checkingTimeForEggs = 180000;

        public CompProperties_BuildSpiderLair()
        {
            this.compClass = typeof(CompBuildSpiderLair);
        }


    }
}