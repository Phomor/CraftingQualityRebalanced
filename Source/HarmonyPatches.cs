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
				// Check first if it hit the random legendary chance, and if so, set it and move on.
				if (relevantSkillLevel >= minSkill[(int)QualityCategory.Legendary] && __result < QualityCategory.Legendary
					&& Rand.Chance(legendaryChanceAt20 - ((20 - relevantSkillLevel) * gradientLegendary)))
				{
					__result = QualityCategory.Legendary;
				}
				else
				{
					// Not a random legendary, step backwards through quality levels until we match the rolled quality or we find the minimum for this skill level
					// start at masterwork because legendary is handled already
					for (int i = (int)QualityCategory.Masterwork; i > (int)QualityCategory.Awful; i--)
					{

						if (relevantSkillLevel >= minSkill[i] 
							// if inspiration then quality to meet is two levels higher. For example if excellent is guaranteed then it's gonna be for sure legendary
							&& ((!inspired && __result < (QualityCategory)i) || (inspired && __result < (QualityCategory)Math.Min(i + 2, (int)QualityCategory.Legendary))))
						{
							if (setQualityInsteadOfReroll)
							{
								__result = (QualityCategory)(inspired ? Math.Min(i + 2, (int)QualityCategory.Legendary) : i);
							}
							else
							{
								__result = QualityUtility.GenerateQualityCreatedByPawn(relevantSkillLevel, inspired);
							}
							break;
						}
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