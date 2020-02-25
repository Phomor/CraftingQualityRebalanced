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
using HarmonyLib;
using UnityEngine;
using Verse;
using RimWorld;

namespace CraftingQualityRebalanced
{
	public class Settings : ModSettings
	{
		private int[] skills = new int[] { 0, 9, 13, 17, 21, 22, 19};
		public int legendaryChance = 5;
		public bool supressMasterworkMessages = false;
		public bool supressLegendaryMessages = false;

		public int GetSkillValue(QualityCategory qc) => this.skills[(int)qc];
		private void SetSkillValue(QualityCategory qc, int value)
		{
			if(qc > QualityCategory.Awful && qc < QualityCategory.Legendary)
			{
				// If qc == poor then dont go in there so poor can be 0
				if(qc != QualityCategory.Poor && value <= skills[(int)qc - 1])
				{
					skills[(int)qc] = skills[(int)qc - 1] + 1;
				}
				// if qc == masterwork then dont go in there so masterwork doesnt clash with legendary
				else if (qc != QualityCategory.Masterwork && value >= skills[(int)qc + 1])
				{
					skills[(int)qc] = skills[(int)qc + 1] - 1;
				}
				else
				{
					skills[(int)qc] = value;
				}
			} else
			{
				skills[(int)qc] = value;
			}
		}


		public override void ExposeData()
		{
			if (skills[2] < 1)
				skills[2] = 1;
			if (skills[3] < 4)
				skills[3] = 4;
			if (skills[4] < 8)
				skills[4] = 8;
			if (skills[5] < 16)
				skills[5] = 16;

			base.ExposeData();

			Scribe_Values.Look(ref skills[0], "minskillawful", 0);
			Scribe_Values.Look(ref skills[1], "minskillpoor", 9);
			Scribe_Values.Look(ref skills[2], "minskillnormal", 13);
			Scribe_Values.Look(ref skills[3], "minskillgood", 17);
			Scribe_Values.Look(ref skills[4], "minskillexcellent", 21);
			Scribe_Values.Look(ref skills[5], "minskillmasterwork", 22);
			Scribe_Values.Look(ref skills[6], "minskilllegendary", 19);
			Scribe_Values.Look(ref legendaryChance, "legendarychance", 5);
			Scribe_Values.Look(ref supressMasterworkMessages, "supressmasterworkmessages", false);
			Scribe_Values.Look(ref supressLegendaryMessages, "supresslegendarymessages", false);
		}	
		
		public void DoWindowContents(Rect inRect)
		{
			{
				var list = new Listing_Standard();
				list.Begin(inRect);
				
				list.Label("CraftingQualityRebalanced.SliderWarning".Translate());
				
				list.Label("CraftingQualityRebalanced.MinimumSkillPoor".Translate() + GetSkillValue(QualityCategory.Poor));
				SetSkillValue(QualityCategory.Poor, (int)list.Slider(GetSkillValue(QualityCategory.Poor), 0, 20));
			
				list.Label("CraftingQualityRebalanced.MinimumSkillNormal".Translate() + GetSkillValue(QualityCategory.Normal));
				SetSkillValue(QualityCategory.Normal, (int) list.Slider(GetSkillValue(QualityCategory.Normal), 1, 20));
			
				list.Label("CraftingQualityRebalanced.MinimumSkillGood".Translate() + GetSkillValue(QualityCategory.Good));
				SetSkillValue(QualityCategory.Good, (int) list.Slider(GetSkillValue(QualityCategory.Good), 4, 20));
				
				list.Label("CraftingQualityRebalanced.MinimumSkillExcellent".Translate() + GetSkillValue(QualityCategory.Excellent));
				SetSkillValue(QualityCategory.Excellent, (int) list.Slider(GetSkillValue(QualityCategory.Excellent), 8, 21));
				
				list.Label("CraftingQualityRebalanced.MinimumSkillMasterwork".Translate() + GetSkillValue(QualityCategory.Masterwork));
				SetSkillValue(QualityCategory.Masterwork, (int) list.Slider(GetSkillValue(QualityCategory.Masterwork), 16, 22));
				
				list.Label("CraftingQualityRebalanced.MinimumSkillLegendary".Translate() + GetSkillValue(QualityCategory.Legendary));
				SetSkillValue(QualityCategory.Legendary, (int) list.Slider(GetSkillValue(QualityCategory.Legendary), 0, 21));
				
				list.Label("CraftingQualityRebalanced.LegendaryChance".Translate() + legendaryChance + "%");
				legendaryChance = (int) list.Slider(legendaryChance, 0, 100);

				list.Label("CraftingQualityRebalanced.LegendaryChanceExplanation".Translate());

				list.Gap();
				
				list.CheckboxLabeledSelectable("CraftingQualityRebalanced.SupressMasterworkMessages".Translate(), ref supressMasterworkMessages, ref supressMasterworkMessages);
				
				list.CheckboxLabeledSelectable("CraftingQualityRebalanced.SupressLegendaryMessages".Translate(), ref supressLegendaryMessages, ref supressLegendaryMessages);
				
				list.End();
			}
		}
	}
}
