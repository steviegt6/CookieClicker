using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI.Chat;

namespace CookieClicker.AchievementsSystem
{
    public class AchievementTagHandler : ITagHandler
    {
		private class AchievementSnippet : TextSnippet
		{
			private readonly Achievement _achievement;

			public AchievementSnippet(Achievement achievement)
				: base(achievement.FriendlyName, Color.LightBlue)
			{
				CheckForHover = true;
				_achievement = achievement;
			}

			public override void OnClick()
			{
				IngameOptions.Close();
				IngameFancyUI.OpenAchievementsAndGoto(_achievement);
			}
		}

		TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
		{
			Achievement achievement = CookieClicker.Achievements.GetAchievement(text);
			if (achievement == null)
            {
                return new TextSnippet(text);
            }

            return new AchievementSnippet(achievement);
		}

		public static string GenerateTag(Achievement achievement) => "[ca:" + achievement.Name + "]";
	}
}
