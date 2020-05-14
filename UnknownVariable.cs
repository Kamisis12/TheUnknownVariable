using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SVGImporter;
using UnityEngine;
using BattleTech;
using BattleTech.UI;
using Harmony;
using Newtonsoft;
using UnityEngine.UI;
using System.Reflection;


namespace UnknownVariable
{
	public static class Main
	{

		public static void Init()
		{
			

			var harmony = HarmonyInstance.Create("UnknownVariable");
			harmony.PatchAll(Assembly.GetExecutingAssembly());
		}
	}

	[HarmonyPatch(typeof(SGDifficultyIndicatorWidget), "SetDifficulty")] 
	static class PatchSGDifficultyIndicatorWidget_SetDifficulty
	{

		
		static bool Prefix(SGDifficultyIndicatorWidget __instance, int difficulty,
			List<UIColorRefTracker> ___pips, ref UIColor ___defaultColor, ref UIColor ___activeColor)
		{
			 difficulty=__instance.Difficulty;
			__instance.Reset();
			int num = new System.Random().Next(0, 100) + 1;
			float num2 = 0f;

			if (num > 95 && difficulty >= 3)
			{
				num2 = (float)difficulty / 2f - 1f;
			}
			if (num <= 95 && num > 65 && difficulty >= 2)
			{
				num2 = (float)difficulty / 2f - 0.5f;
			}
			if (num <= 65 && num > 40 && difficulty <= 8)
			{
				num2 = (float)difficulty / 2f + 0.5f;
			}
			if (num <= 40 || difficulty < 2 || difficulty > 8)
			{
				num2 = (float)difficulty / 2f;
			}

			int i;
			for (i = 0; i < Mathf.FloorToInt(num2); i++)
			{
				UIColorRefTracker uicolorRefTracker = ___pips[i];
				uicolorRefTracker.GetComponent<SVGImage>().fillAmount = 1f;
				uicolorRefTracker.SetUIColor(___activeColor);
			}
			if ((float)i < num2)
			{
				UIColorRefTracker uicolorRefTracker2 = ___pips[i];
				SVGImage component = uicolorRefTracker2.GetComponent<SVGImage>();
				uicolorRefTracker2.SetUIColor(___activeColor);
				component.fillAmount = num2 - (float)i;
			}
			return false;
		}

		// Token: 0x06013CC1 RID: 81089 RVA: 0x00509D40 File Offset: 0x00507F40
		static void Reset(UIColor ___defaultColor, List<UIColorRefTracker> ___pips)
		{
			if (___defaultColor == UIColor.Custom)
			{
				return;
			}
			foreach (UIColorRefTracker uicolorRefTracker in ___pips)
			{
				uicolorRefTracker.SetUIColor(___defaultColor);
				SVGImage component = uicolorRefTracker.GetComponent<SVGImage>();
				if (component != null)
				{
					component.fillAmount = 0f;
				}
			}
		}

	}
}