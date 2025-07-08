using RimWorld;
using UnityEngine;
using Verse;
using System;
using System.Collections.Generic;
using System.Linq;



namespace SmartOdyssey
{
   

    public class SmartOdyssey_Settings : ModSettings

    {

     
        public const float mutatorMultiplierBase = 1;
        public static float mutatorMultiplier = mutatorMultiplierBase;
        public const float landmarkMutatorMultiplierBase = 1;
        public static float landmarkMutatorMultiplier = landmarkMutatorMultiplierBase;
        private static Vector2 scrollPosition = Vector2.zero;
        public Dictionary<string, float> mutatorCommonalities = new Dictionary<string, float>();
        private List<string> mutatorKeys;
        private List<float> mutatorValues;


        public override void ExposeData()
        {
            base.ExposeData();
           
            Scribe_Values.Look<float>(ref mutatorMultiplier, "mutatorMultiplier", mutatorMultiplierBase, true);
            Scribe_Values.Look<float>(ref landmarkMutatorMultiplier, "landmarkMutatorMultiplier", landmarkMutatorMultiplierBase, true);
            Scribe_Collections.Look(ref mutatorCommonalities, "mutatorCommonalities", LookMode.Value, LookMode.Value, ref mutatorKeys, ref mutatorValues);
        }

        public void DoWindowContents(Rect inRect)
        {
            Listing_Standard ls = new Listing_Standard();

            var scrollContainer = inRect.ContractedBy(10);
            scrollContainer.height -= ls.CurHeight;
            scrollContainer.y += ls.CurHeight;
            Widgets.DrawBoxSolid(scrollContainer, Color.grey);
            var innerContainer = scrollContainer.ContractedBy(1);
            Widgets.DrawBoxSolid(innerContainer, new ColorInt(42, 43, 44).ToColor);
            var frameRect = innerContainer.ContractedBy(5);
            frameRect.y += 15;
            frameRect.height -= 15;
            var contentRect = frameRect;
            contentRect.x = 0;
            contentRect.y = 0;
            contentRect.width -= 20;

            int numberOfMutators = (from k in DefDatabase<TileMutatorDef>.AllDefsListForReading
                                    where (k.chanceOnNonLandmarkTile > 0)
                                    select k).Count(); ;
            contentRect.height = numberOfMutators * 24f + 350;

            Listing_Standard ls2 = new Listing_Standard();

            Widgets.BeginScrollView(frameRect, ref scrollPosition, contentRect, true);
            ls2.Begin(contentRect.AtZero());

           
            var mutatorLabel = ls2.LabelPlusButton("SO_MutatorMultiplier".Translate() + ": " + mutatorMultiplier + "x", "SO_MutatorMultiplierTooltip".Translate());
            mutatorMultiplier = (float)Math.Round(ls2.Slider(mutatorMultiplier, 0f, 20f), 2);

            if (ls2.Settings_Button("SO_Reset_Plain".Translate(), new Rect(0f, mutatorLabel.position.y + 35, 250f, 29f)))
            {
                mutatorMultiplier = mutatorMultiplierBase;
            }
            var landmarkMutatorLabel = ls2.LabelPlusButton("SO_LandmarkMutatorMultiplier".Translate() + ": " + landmarkMutatorMultiplier + "x", "SO_LandmarkMutatorMultiplierTooltip".Translate());
            landmarkMutatorMultiplier = (float)Math.Round(ls2.Slider(landmarkMutatorMultiplier, 0f, 20f), 2);

            if (ls2.Settings_Button("SO_Reset_Plain".Translate(), new Rect(0f, landmarkMutatorLabel.position.y + 35, 250f, 29f)))
            {
                landmarkMutatorMultiplier = landmarkMutatorMultiplierBase;
            }

            ls2.GapLine();
            Text.Font = GameFont.Medium;
            ls2.Label("SO_SpecificMutatorCommonalities".Translate());
            Text.Font = GameFont.Small;
            ls2.Gap();

            List<string> mutatorKeys = mutatorCommonalities.Keys.ToList().OrderBy(x => DefDatabase<TileMutatorDef>.GetNamedSilentFail(x).label).ToList();

            if (mutatorKeys.Count > 0)
            {
                for (int num = 0; num < mutatorKeys.Count; num++)
                {
                    if (mutatorKeys[num] != "" && DefDatabase<TileMutatorDef>.GetNamedSilentFail(mutatorKeys[num]) != null)
                    {
                        TileMutatorDef mutator = DefDatabase<TileMutatorDef>.GetNamedSilentFail(mutatorKeys[num]);
                        var labelRect = new Rect(0, (num + 1) * 24 + 250, 250, 24);
                        Widgets.Label(labelRect, mutator.LabelCap + ": " + mutatorCommonalities[mutatorKeys[num]].ToStringPercent("F1"));
                        TooltipHandler.TipRegion(labelRect, mutator.description);
                        var sliderRect = new Rect(350, (num + 1) * 24 + 250, inRect.width - 450f, 24);
                        mutatorCommonalities[mutatorKeys[num]] = (float)Math.Round(Widgets.HorizontalSlider(sliderRect, mutatorCommonalities[mutatorKeys[num]], 0f, 1f), 3);
                    }
                    else
                    {
                        mutatorCommonalities.Remove(mutatorKeys[num]);
                    }
                }
            }

            if (ls2.Settings_Button("SO_Reset_Plain".Translate(), new Rect(0, (mutatorKeys.Count + 1) * 24 + 250, 250, 24)))
            {
                for (int num = 0; num < mutatorKeys.Count; num++)
                {
                    mutatorCommonalities[mutatorKeys[num]] = DefDatabase<TileMutatorDef>.AllDefsListForReading.Where(x => x.defName == mutatorKeys[num]).Select(x => x.chanceOnNonLandmarkTile).First();

                }
            }

            ls2.End();
            Widgets.EndScrollView();
            Write();
        }
    }
}
