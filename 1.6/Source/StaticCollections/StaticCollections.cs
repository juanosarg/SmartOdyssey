
using Verse;
using System;
using RimWorld;
using System.Collections.Generic;
using System.Linq;


namespace SmartOdyssey
{
    [StaticConstructorOnStartup]
    public static class StaticCollections
    {
        static StaticCollections()
        {
          

            foreach (MutatorIconDef mutatorAndIconList in DefDatabase<MutatorIconDef>.AllDefsListForReading)
            {
                foreach (MutatorAndIcon mutatorAndIcon in mutatorAndIconList.mutatorIcons)
                {
                    tileMutatorIcons[mutatorAndIcon.mutator] = mutatorAndIcon.icon;

                }

            }
        }

      
        public static Dictionary<TileMutatorDef, string> tileMutatorIcons = new Dictionary<TileMutatorDef, string>();

    }
}
