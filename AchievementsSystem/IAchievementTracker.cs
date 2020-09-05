namespace CookieClicker.AchievementsSystem
{
	public interface IAchievementTracker
	{
		void ReportAs(string name);

		TrackerType GetTrackerType();

		void Load();

		void Clear();
	}
}
