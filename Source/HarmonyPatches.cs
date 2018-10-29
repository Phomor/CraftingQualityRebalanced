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
	static class HarmonyPatches
	{
		public static int minSkillLegendary = 19;
		public static int minSkillMasterwork = 22;
		public static int minSkillExcellent = 21;
		public static int minSkillGood = 17;
		public static int minSkillNormal = 13;
		public static int minSkillPoor = 9;
		public static float legendaryChanceAt20 = 0.05f;
		public static float gradientLegendary = 0.025f;
		public static bool supressMasterwork = false;
		public static bool supressLegendary = false;
		
    	[HarmonyPostfix]
		public static void Postfix(ref int relevantSkillLevel, ref bool inspired, ref QualityCategory __result)
		{
			if(relevantSkillLevel >= minSkillLegendary && __result < QualityCategory.Legendary && Rand.Chance(legendaryChanceAt20 - ((20 - relevantSkillLevel) * gradientLegendary)))
			{
				__result = QualityCategory.Legendary;
			}
			else if(relevantSkillLevel >= minSkillMasterwork && __result < QualityCategory.Masterwork)
			{
				__result = QualityUtility.GenerateQualityCreatedByPawn(relevantSkillLevel, inspired);
			}
			else if(relevantSkillLevel >= minSkillExcellent && __result < QualityCategory.Excellent)
			{
				__result = QualityUtility.GenerateQualityCreatedByPawn(relevantSkillLevel, inspired);
			}
			else if(relevantSkillLevel >= minSkillGood && __result < QualityCategory.Good)
			{
				__result = QualityUtility.GenerateQualityCreatedByPawn(relevantSkillLevel, inspired);
			}
			else if(relevantSkillLevel >= minSkillNormal && __result < QualityCategory.Normal)
			{
				__result = QualityUtility.GenerateQualityCreatedByPawn(relevantSkillLevel, inspired);
			}
			else if(relevantSkillLevel >= minSkillPoor && __result < QualityCategory.Poor)
			{
				__result = QualityUtility.GenerateQualityCreatedByPawn(relevantSkillLevel, inspired);
			}
		}
		
		[HarmonyPrefix]
		public static bool Prefix(ref Thing thing)
		{
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			if(compQuality != null) {
				if(compQuality.Quality == QualityCategory.Masterwork && supressMasterwork) {
					return false;
				}
				else if(compQuality.Quality == QualityCategory.Legendary && supressLegendary) {
					return false;
				}
			}
			return true;
		}
	}
}