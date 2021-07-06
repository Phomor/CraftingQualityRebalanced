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
	static class HarmonyPatches
	{
		public static int[] minSkill = new int[] { 0, 9, 13, 17, 21, 22, 19 };
		public static float legendaryChanceAt20 = 0.05f;
		public static float gradientLegendary = 0.025f;
		public static bool supressMasterwork = false;
		public static bool supressLegendary = false;
		public static bool setQualityInsteadOfReroll = false;

		[HarmonyPostfix]
		public static void Postfix(ref int relevantSkillLevel, ref bool inspired, ref QualityCategory __result)
		{
			try
			{
				if (relevantSkillLevel >= minSkill[(int)QualityCategory.Legendary] && __result < QualityCategory.Legendary && Rand.Chance(legendaryChanceAt20 - ((20 - relevantSkillLevel) * gradientLegendary)))
				{
					__result = QualityCategory.Legendary;
				}
				else if (relevantSkillLevel >= minSkill[(int)QualityCategory.Masterwork] && __result < QualityCategory.Masterwork)
				{
					if (setQualityInsteadOfReroll)
                    {
						__result = QualityCategory.Masterwork;
                    } else
					{
						__result = QualityUtility.GenerateQualityCreatedByPawn(relevantSkillLevel, inspired);
					}
				}
				else if (relevantSkillLevel >= minSkill[(int)QualityCategory.Excellent] && __result < QualityCategory.Excellent)
				{
					if (setQualityInsteadOfReroll)
					{
						__result = QualityCategory.Excellent;
					}
					else
					{
						__result = QualityUtility.GenerateQualityCreatedByPawn(relevantSkillLevel, inspired);
					}
				}
				else if (relevantSkillLevel >= minSkill[(int)QualityCategory.Good] && __result < QualityCategory.Good)
				{
					if (setQualityInsteadOfReroll)
					{
						__result = QualityCategory.Good;
					}
					else
					{
						__result = QualityUtility.GenerateQualityCreatedByPawn(relevantSkillLevel, inspired);
					}
				}
				else if (relevantSkillLevel >= minSkill[(int)QualityCategory.Normal] && __result < QualityCategory.Normal)
				{
					if (setQualityInsteadOfReroll)
					{
						__result = QualityCategory.Normal;
					}
					else
					{
						__result = QualityUtility.GenerateQualityCreatedByPawn(relevantSkillLevel, inspired);
					}
				}
				else if (relevantSkillLevel >= minSkill[(int)QualityCategory.Poor] && __result < QualityCategory.Poor)
				{
					if (setQualityInsteadOfReroll)
					{
						__result = QualityCategory.Poor;
					}
					else
					{
						__result = QualityUtility.GenerateQualityCreatedByPawn(relevantSkillLevel, inspired);
					}
				}
			}
			catch(Exception ex)
			{
				Log.Error(ex.ToString());
			}
		}
		
		[HarmonyPrefix]
		public static bool Prefix(ref Thing thing)
		{
			try
			{
				CompQuality compQuality = thing.TryGetComp<CompQuality>();
				if (compQuality != null)
				{
					if (compQuality.Quality == QualityCategory.Masterwork && supressMasterwork)
					{
						return false;
					}
					else if (compQuality.Quality == QualityCategory.Legendary && supressLegendary)
					{
						return false;
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
				return true;
			}
		}
	}
}