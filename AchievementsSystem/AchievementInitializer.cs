using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace CookieClicker.AchievementsSystem
{
    public class AchievementInitializer
    {
        public static void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Achievement achievement = new Achievement("TEST", "Test Name", "Test Description");
                achievement.AddCondition(CustomFlagCondition.Create("Test"));
                CookieClicker.Achievements.Register(achievement);

                int num = 0;
                CookieClicker.Achievements.RegisterIconIndex("TEST", num++);

                AchievementCategory category = AchievementCategory.CookiesBaked;
                CookieClicker.Achievements.RegisterAchievementCategory("TEST", category);

                category = AchievementCategory.GoldenCookies;

                category = AchievementCategory.Misc;

                category = AchievementCategory.PerSecond;

                category = AchievementCategory.SugarLumps;

                CookieClicker.Achievements.Load();
                CookieClicker.Achievements.OnAchievementCompleted += OnAchievementCompleted;
                AchievementsHelper.Initialize();
            }
        }

        private static void OnAchievementCompleted(Achievement achievement)
        {
            Main.NewText(Language.GetTextValue("Achievements.Completed", AchievementTagHandler.GenerateTag(achievement)));
            Main.PlaySound(SoundID.MenuClose);
        }
    }
}
