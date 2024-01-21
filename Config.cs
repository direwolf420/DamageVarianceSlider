using System.ComponentModel;
using System.Runtime.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace DamageVarianceSlider
{
	public class Config : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;
		public static Config Instance => ModContent.GetInstance<Config>();

		public const int DamageVarianceMin = 0;
		public const int DamageVarianceMax = 100;

		[Slider]
		[Range(DamageVarianceMin, DamageVarianceMax)]
		[DefaultValue(15)]
		public int DamageVariance;

		[DefaultValue(false)]
		public bool DisableLuckVariance;

		[OnDeserialized]
		internal void OnDeserializedMethod(StreamingContext context)
		{
			DamageVariance = Utils.Clamp(DamageVariance, DamageVarianceMin, DamageVarianceMax);
		}

		public static bool IsPlayerLocalServerOwner(int whoAmI)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return Netplay.Connection.Socket.GetRemoteAddress().IsLocalHost();
			}

			return NetMessage.DoesPlayerSlotCountAsAHost(whoAmI);
		}

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message)
		{
			if (Main.netMode == NetmodeID.SinglePlayer) return true;
			else if (!IsPlayerLocalServerOwner(whoAmI))
			{
				message = NetworkText.FromKey("tModLoader.ModConfigRejectChangesNotHost");
				return false;
			}
			return base.AcceptClientChanges(pendingConfig, whoAmI, ref message);
		}
	}
}
