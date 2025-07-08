using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;


namespace SmartOdyssey
{





    public class SmartOdyssey_Mod: Mod
    {
        public static SmartOdyssey_Settings settings;

        public SmartOdyssey_Mod(ModContentPack content) : base(content)
        {
            settings = GetSettings<SmartOdyssey_Settings>();
        }
        public override string SettingsCategory()
        {
           
                return "Smart Odyssey";
           
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            List<TileMutatorDef> allMutators = DefDatabase<TileMutatorDef>.AllDefsListForReading.Where(x => x.chanceOnNonLandmarkTile > 0).ToList();
            if (settings.mutatorCommonalities is null) settings.mutatorCommonalities = new Dictionary<string, float>();

            List<string> mutatorsToRemove = new List<string>();
            foreach (KeyValuePair<string, float> mutatorToRemove in settings.mutatorCommonalities)
            {
                if (DefDatabase<TileMutatorDef>.GetNamedSilentFail(mutatorToRemove.Key) is null)
                {
                    mutatorsToRemove.Add(mutatorToRemove.Key);
                }
            }
            foreach (string mutatorToRemove in mutatorsToRemove)
            {
                settings.mutatorCommonalities.Remove(mutatorToRemove);
            }
            foreach (TileMutatorDef mutator in allMutators)
            {
                if (!settings.mutatorCommonalities.ContainsKey(mutator.defName))
                {
                    settings.mutatorCommonalities[mutator.defName] = mutator.chanceOnNonLandmarkTile;
                }
            }



            settings.DoWindowContents(inRect);
        }

    }
}

