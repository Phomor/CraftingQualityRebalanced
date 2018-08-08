/*
 * User: Phomor
 * Date: 22.06.2018
 * Time: 18:18
 */
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Reflection;
using Harmony;
using UnityEngine;
using Verse;
using RimWorld;

namespace CraftingQualityRebalanced
{
	public class Settings : Verse.ModSettings
	{
		public int minSkillPoor = 9;
		public int minSkillNormal = 13;
		public int minSkillGood = 17;
		public int minSkillExcellent = 21;
		public int minSkillMasterwork = 21;
		public int minSkillLegendary = 21;
		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref minSkillPoor, "minskillpoor", 9);
			Scribe_Values.Look(ref minSkillNormal, "minskillnormal", 13);
			Scribe_Values.Look(ref minSkillGood, "minskillgood", 17);
			Scribe_Values.Look(ref minSkillExcellent, "minskillexcellent", 21);
			Scribe_Values.Look(ref minSkillMasterwork, "minskillmasterwork", 22);
			Scribe_Values.Look(ref minSkillLegendary, "minskilllegendary", 23);
		}	
		
		public void DoWindowContents(Rect inRect)
		{
			{
				var list = new Listing_Standard();
				Color defaultColor = GUI.color;
				list.Begin(inRect);
				
				list.Label("CraftingQualityRebalanced.SliderWarning".Translate());
				
				list.Label("CraftingQualityRebalanced.MinimumSkillPoor".Translate() + minSkillPoor);
				minSkillPoor = (int) list.Slider(minSkillPoor, 0, minSkillNormal - 1);
			
				list.Label("CraftingQualityRebalanced.MinimumSkillNormal".Translate() + minSkillNormal);
				minSkillNormal = (int) list.Slider(minSkillNormal, minSkillPoor + 1, minSkillGood - 1);
			
				list.Label("CraftingQualityRebalanced.MinimumSkillGood".Translate() + minSkillGood);
				minSkillGood = (int) list.Slider(minSkillGood, minSkillNormal + 1, minSkillExcellent - 1);
				
				list.Label("CraftingQualityRebalanced.MinimumSkillExcellent".Translate() + minSkillExcellent);
				minSkillExcellent = (int) list.Slider(minSkillExcellent, minSkillGood + 1, minSkillMasterwork - 1);
				
				list.Label("CraftingQualityRebalanced.MinimumSkillMasterwork".Translate() + minSkillMasterwork);
				minSkillMasterwork = (int) list.Slider(minSkillMasterwork, 7, minSkillLegendary - 1);
				
				list.Label("CraftingQualityRebalanced.MinimumSkillLegendary".Translate() + minSkillLegendary);
				minSkillLegendary = (int) list.Slider(minSkillLegendary, minSkillMasterwork + 1, 23);
				
				list.End();
			}
		}
	}
}
