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
	/// <summary>
	/// Crafting and Building Quality is less random, higher skill Pawns can't make awful things anymore.
	/// In Detail: Pawns with Skill Level 17+ will make at least good things, Skill Level 13+ will make at least normal things, 
	/// Skill Level 9+ will make at least poor things, Skill Level 7+ will make at least shoddy things
	/// </summary>
	public class Controller : Mod
	{
		public Settings settings;
		
		public Controller(ModContentPack content) : base (content)
		{
			var harmony = new Harmony("rimworld.phomor.craftingqualityrebalanced");
			var redoQuality = typeof(QualityUtility).GetMethod("GenerateQualityCreatedByPawn", new Type[] { typeof(int), typeof(bool) });
			var postfix = typeof(HarmonyPatches).GetMethod("Postfix");
			harmony.Patch(redoQuality, null, new HarmonyMethod(postfix));
			var supressMessages = typeof(QualityUtility).GetMethod("SendCraftNotification");
			var prefix = typeof(HarmonyPatches).GetMethod("Prefix");
			harmony.Patch(supressMessages, new HarmonyMethod(prefix), null);
			settings = GetSettings<Settings>();
			UpdatePatches();
		}
		
		public override string SettingsCategory()
    	{
			return "CraftingQualityRebalanced".Translate();
    	}
		
		public override void DoSettingsWindowContents(Rect inRect)
		{
			settings.DoWindowContents(inRect);
			UpdatePatches();
		}
		
		public void UpdatePatches()
		{
			foreach (QualityCategory qc in Enum.GetValues(typeof(QualityCategory)))
			{
				HarmonyPatches.minSkill[(int)qc] = settings.GetSkillValue(qc);
			}
			HarmonyPatches.legendaryChanceAt20 = (float)(settings.legendaryChance / 100f);
			if(HarmonyPatches.minSkill[(int)QualityCategory.Legendary] != 21)
				HarmonyPatches.gradientLegendary = (float)(HarmonyPatches.legendaryChanceAt20 /(20 - (HarmonyPatches.minSkill[(int)QualityCategory.Legendary] - 1)));
			HarmonyPatches.supressMasterwork = settings.supressMasterworkMessages;
			HarmonyPatches.supressLegendary = settings.supressLegendaryMessages;
			HarmonyPatches.setQualityInsteadOfReroll = settings.setQualityInsteadOfReroll;
		}
	}
}
