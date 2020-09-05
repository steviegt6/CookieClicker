namespace CookieClicker.AchievementsSystem
{
    public class ConditionDoubleTracker : AchievementTracker<double>
    {
		public ConditionDoubleTracker(double maxValue) : base(TrackerType.Double) => _maxValue = maxValue;

		public ConditionDoubleTracker() : base(TrackerType.Double) { }

		public override void ReportUpdate() { }

		protected override void Load() { }
	}
}
