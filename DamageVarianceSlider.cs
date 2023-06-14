using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DamageVarianceSlider
{
	public class DamageVarianceSliderMod : Mod
	{
		public static LocalizedText AcceptClientChangesText { get; private set; }

		public override void Load()
		{
			On_Main.DamageVar_float_int_float += On_Main_DamageVar_float_int_float;

			string configCategory = $"Configs.Common.";
			AcceptClientChangesText ??= Language.GetOrRegister(this.GetLocalizationKey($"{configCategory}AcceptClientChanges"));
		}

		private static int On_Main_DamageVar_float_int_float(On_Main.orig_DamageVar_float_int_float orig, float dmg, int percent, float luck)
		{
			if (Config.Instance.DisableLuckVariance)
			{
				luck = 0;
			}

			if (percent == Main.DefaultDamageVariationPercent)
			{
				percent = Config.Instance.DamageVariance;
			}
			else if (Main.DefaultDamageVariationPercent != 0)
			{
				//If damage variation is different from the default one (mods using modifiers.damagevariationscale), scale the one set by the config by the amount it's different by
				float ratio = percent / (float)Main.DefaultDamageVariationPercent;
				if (ratio != 0)
				{
					percent = (int)(Config.Instance.DamageVariance / ratio);
				}
			}

			int ret = orig(dmg, percent, luck);

			return ret;
		}
	}
}
