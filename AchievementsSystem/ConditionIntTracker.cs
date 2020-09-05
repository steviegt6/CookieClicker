namespace CookieClicker.AchievementsSystem
{
	public class ConditionIntTracker : AchievementTracker<int>
	{
		public ConditionIntTracker() : base(TrackerType.Int) { }

		public ConditionIntTracker(int maxValue) : base(TrackerType.Int) => _maxValue = maxValue;

		public override void ReportUpdate() { }

		protected override void Load() { }
	}
}
