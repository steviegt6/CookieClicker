namespace CookieClicker.AchievementsSystem
{
	public class CustomFlagCondition : AchievementCondition
	{
		private CustomFlagCondition(string name) : base(name) { }

		public static AchievementCondition Create(string name) => new CustomFlagCondition(name);
	}
}
