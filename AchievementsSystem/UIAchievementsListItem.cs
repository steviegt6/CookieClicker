using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace CookieClicker.AchievementsSystem
{
    public class UIAchievementListItem : UIPanel
	{
		private readonly Achievement _achievement;
		private readonly UIImageFramed _achievementIcon;
		private readonly UIImage _achievementIconBorders;
		private Rectangle _iconFrame;
		private Rectangle _iconFrameUnlocked;
		private Rectangle _iconFrameLocked;
		private readonly Texture2D _innerPanelTopTexture;
		private readonly Texture2D _innerPanelBottomTexture;
		private readonly Texture2D _categoryTexture;
		private bool _locked;
		private readonly bool _large;

		public UIAchievementListItem(Achievement achievement, bool largeForOtherLanguages)
		{
			_large = largeForOtherLanguages;
			BackgroundColor = new Color(26, 40, 89) * 0.8f;
			BorderColor = new Color(13, 20, 44) * 0.8f;
			float num = 16 + _large.ToInt() * 20;
			float num2 = _large.ToInt() * 6;
			float num3 = _large.ToInt() * 12;
			_achievement = achievement;
			Height.Set(66f + num, 0f);
			Width.Set(0f, 1f);
			PaddingTop = 8f;
			PaddingLeft = 9f;
			int num4 = CookieClicker.Achievements.GetIconIndex(achievement.Name);
			_iconFrameUnlocked = new Rectangle(num4 % 8 * 66, num4 / 8 * 66, 64, 64);
			_iconFrameLocked = _iconFrameUnlocked;
			_iconFrameLocked.X += 528;
			_iconFrame = _iconFrameLocked;
			UpdateIconFrame();
			_achievementIcon = new UIImageFramed(ModContent.GetTexture("Terraria/UI/Achievements"), _iconFrame);
			_achievementIcon.Left.Set(num2, 0f);
			_achievementIcon.Top.Set(num3, 0f);
			Append(_achievementIcon);
			_achievementIconBorders = new UIImage(ModContent.GetTexture("Terraria/UI/Achievement_Borders"));
			_achievementIconBorders.Left.Set(-4f + num2, 0f);
			_achievementIconBorders.Top.Set(-4f + num3, 0f);
			Append(_achievementIconBorders);
			_innerPanelTopTexture = ModContent.GetTexture("Terraria/UI/Achievement_InnerPanelTop");
			if (_large)
            {
                _innerPanelBottomTexture = ModContent.GetTexture("Terraria/UI/Achievement_InnerPanelBottom_Large");
            }
            else
            {
                _innerPanelBottomTexture = ModContent.GetTexture("Terraria/UI/Achievement_InnerPanelBottom");
            }

            _categoryTexture = ModContent.GetTexture("CookieClicker/UI/Categories");
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			int num = _large.ToInt() * 6;
			Vector2 value = new Vector2(num, 0f);
			_locked = !_achievement.IsCompleted;
			UpdateIconFrame();
			CalculatedStyle innerDimensions = GetInnerDimensions();
			CalculatedStyle dimensions = _achievementIconBorders.GetDimensions();
			float num2 = dimensions.X + dimensions.Width;
			Vector2 value2 = new Vector2(num2 + 7f, innerDimensions.Y);
			Tuple<decimal, decimal> trackerValues = GetTrackerValues();
			bool flag = false;
			if ((!(trackerValues.Item1 == 0m) || !(trackerValues.Item2 == 0m)) && _locked)
            {
                flag = true;
            }

            float num3 = innerDimensions.Width - dimensions.Width + 1f - num * 2;
			Vector2 baseScale = new Vector2(0.85f);
			Vector2 baseScale2 = new Vector2(0.92f);
			string text = Main.fontItemStack.CreateWrappedText(_achievement.Description, (num3 - 20f) * (1f / baseScale2.X), Language.ActiveCulture.CultureInfo);
			Vector2 stringSize = ChatManager.GetStringSize(Main.fontItemStack, text, baseScale2, num3);
			if (!_large)
            {
                stringSize = ChatManager.GetStringSize(Main.fontItemStack, _achievement.Description, baseScale2, num3);
            }

            float num4 = 38f + (_large ? 20 : 0);
			if (stringSize.Y > num4)
            {
                baseScale2.Y *= num4 / stringSize.Y;
            }

            Color value3 = _locked ? Color.Silver : Color.Gold;
			value3 = Color.Lerp(value3, Color.White, base.IsMouseHovering ? 0.5f : 0f);
			Color value4 = _locked ? Color.DarkGray : Color.Silver;
			value4 = Color.Lerp(value4, Color.White, base.IsMouseHovering ? 1f : 0f);
			Color color = base.IsMouseHovering ? Color.White : Color.Gray;
			Vector2 vector = value2 - Vector2.UnitY * 2f + value;
			DrawPanelTop(spriteBatch, vector, num3, color);
			AchievementCategory category = _achievement.Category;
			vector.Y += 2f;
			vector.X += 4f;
			spriteBatch.Draw(_categoryTexture, vector, _categoryTexture.Frame(5, 2, (int)category), base.IsMouseHovering ? Color.White : Color.Silver, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
			vector.X += 4f;
			vector.X += 17f;
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, _achievement.FriendlyName, vector, value3, 0f, Vector2.Zero, baseScale, num3);
			vector.X -= 17f;
			Vector2 position = value2 + Vector2.UnitY * 27f + value;
			DrawPanelBottom(spriteBatch, position, num3, color);
			position.X += 8f;
			position.Y += 4f;
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, text, position, value4, 0f, Vector2.Zero, baseScale2);
			if (flag)
			{
				Vector2 vector2 = vector + Vector2.UnitX * num3 + Vector2.UnitY;
				string text2 = (int)trackerValues.Item1 + "/" + (int)trackerValues.Item2;
				Vector2 baseScale3 = new Vector2(0.75f);
				Vector2 stringSize2 = ChatManager.GetStringSize(Main.fontItemStack, text2, baseScale3);
				float progress = (float)(trackerValues.Item1 / trackerValues.Item2);
				float num5 = 80f;
				Color color2 = new Color(100, 255, 100);
				if (!base.IsMouseHovering)
                {
                    color2 = Color.Lerp(color2, Color.Black, 0.25f);
                }

                Color color3 = new Color(255, 255, 255);
				if (!base.IsMouseHovering)
                {
                    color3 = Color.Lerp(color3, Color.Black, 0.25f);
                }

                DrawProgressBar(spriteBatch, progress, vector2 - Vector2.UnitX * num5 * 0.7f, num5, color3, color2, color2.MultiplyRGBA(new Color(new Vector4(1f, 1f, 1f, 0.5f))));
				vector2.X -= num5 * 1.4f + stringSize2.X;
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, text2, vector2, value3, 0f, new Vector2(0f, 0f), baseScale3, 90f);
			}
		}

		private void UpdateIconFrame()
		{
			if (!_locked)
            {
                _iconFrame = _iconFrameUnlocked;
            }
            else
            {
                _iconFrame = _iconFrameLocked;
            }

            if (_achievementIcon != null)
            {
                _achievementIcon.SetFrame(_iconFrame);
            }
        }

		private void DrawPanelTop(SpriteBatch spriteBatch, Vector2 position, float width, Color color)
		{
			spriteBatch.Draw(_innerPanelTopTexture, position, new Rectangle(0, 0, 2, _innerPanelTopTexture.Height), color);
			spriteBatch.Draw(_innerPanelTopTexture, new Vector2(position.X + 2f, position.Y), new Rectangle(2, 0, 2, _innerPanelTopTexture.Height), color, 0f, Vector2.Zero, new Vector2((width - 4f) / 2f, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(_innerPanelTopTexture, new Vector2(position.X + width - 2f, position.Y), new Rectangle(4, 0, 2, _innerPanelTopTexture.Height), color);
		}

		private void DrawPanelBottom(SpriteBatch spriteBatch, Vector2 position, float width, Color color)
		{
			spriteBatch.Draw(_innerPanelBottomTexture, position, new Rectangle(0, 0, 6, _innerPanelBottomTexture.Height), color);
			spriteBatch.Draw(_innerPanelBottomTexture, new Vector2(position.X + 6f, position.Y), new Rectangle(6, 0, 7, _innerPanelBottomTexture.Height), color, 0f, Vector2.Zero, new Vector2((width - 12f) / 7f, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(_innerPanelBottomTexture, new Vector2(position.X + width - 6f, position.Y), new Rectangle(13, 0, 6, _innerPanelBottomTexture.Height), color);
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			BackgroundColor = new Color(46, 60, 119);
			BorderColor = new Color(20, 30, 56);
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			BackgroundColor = new Color(26, 40, 89) * 0.8f;
			BorderColor = new Color(13, 20, 44) * 0.8f;
		}

		public Achievement GetAchievement() => _achievement;

		private Tuple<decimal, decimal> GetTrackerValues()
		{
			if (!_achievement.HasTracker)
            {
                return Tuple.Create(0m, 0m);
            }

            IAchievementTracker tracker = _achievement.GetTracker();
			if (tracker.GetTrackerType() == TrackerType.Int)
			{
				AchievementTracker<int> achievementTracker = (AchievementTracker<int>)tracker;
				return Tuple.Create((decimal)achievementTracker.Value, (decimal)achievementTracker.MaxValue);
			}

			if (tracker.GetTrackerType() == TrackerType.Double)
			{
				AchievementTracker<double> achievementTracker2 = (AchievementTracker<double>)tracker;
				return Tuple.Create((decimal)achievementTracker2.Value, (decimal)achievementTracker2.MaxValue);
			}

			return Tuple.Create(0m, 0m);
		}

		private void DrawProgressBar(SpriteBatch spriteBatch, float progress, Vector2 spot, float Width = 169f, Color BackColor = default, Color FillingColor = default, Color BlipColor = default)
		{
			if (BlipColor == Color.Transparent)
            {
                BlipColor = new Color(255, 165, 0, 127);
            }

            if (FillingColor == Color.Transparent)
            {
                FillingColor = new Color(255, 241, 51);
            }

            if (BackColor == Color.Transparent)
            {
                FillingColor = new Color(255, 255, 255);
            }

            Texture2D value = Main.colorBarTexture;
			_ = Main.colorBlipTexture;
			Texture2D value2 = Main.magicPixel;
			float num = MathHelper.Clamp(progress, 0f, 1f);
			float num2 = Width * 1f;
			float num3 = 8f;
			float num4 = num2 / 169f;
			Vector2 position = spot + Vector2.UnitY * num3 + Vector2.UnitX * 1f;
			spriteBatch.Draw(value, spot, new Rectangle(5, 0, value.Width - 9, value.Height), BackColor, 0f, new Vector2(84.5f, 0f), new Vector2(num4, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(value, spot + new Vector2((0f - num4) * 84.5f - 5f, 0f), new Rectangle(0, 0, 5, value.Height), BackColor, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
			spriteBatch.Draw(value, spot + new Vector2(num4 * 84.5f, 0f), new Rectangle(value.Width - 4, 0, 4, value.Height), BackColor, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
			position += Vector2.UnitX * (num - 0.5f) * num2;
			position.X -= 1f;
			spriteBatch.Draw(value2, position, new Rectangle(0, 0, 1, 1), FillingColor, 0f, new Vector2(1f, 0.5f), new Vector2(num2 * num, num3), SpriteEffects.None, 0f);
			if (progress != 0f)
            {
                spriteBatch.Draw(value2, position, new Rectangle(0, 0, 1, 1), BlipColor, 0f, new Vector2(1f, 0.5f), new Vector2(2f, num3), SpriteEffects.None, 0f);
            }

            spriteBatch.Draw(value2, position, new Rectangle(0, 0, 1, 1), Color.Black, 0f, new Vector2(0f, 0.5f), new Vector2(num2 * (1f - num), num3), SpriteEffects.None, 0f);
		}

		public override int CompareTo(object obj)
		{
            if (!(obj is UIAchievementListItem uIAchievementListItem))
            {
                return 0;
            }

            if (_achievement.IsCompleted && !uIAchievementListItem._achievement.IsCompleted)
            {
                return -1;
            }

            if (!_achievement.IsCompleted && uIAchievementListItem._achievement.IsCompleted)
            {
                return 1;
            }

            return _achievement.ID.CompareTo(uIAchievementListItem._achievement.ID);
		}
	}
}
