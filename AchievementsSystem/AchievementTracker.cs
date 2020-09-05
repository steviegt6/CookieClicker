namespace CookieClicker.AchievementsSystem
{
    public abstract class AchievementTracker<T> : IAchievementTracker
    {
		protected T _value;
		protected T _maxValue;
		protected string _name;
		private readonly TrackerType _type;

		public T Value => _value;

		public T MaxValue => _maxValue;

		protected AchievementTracker(TrackerType type) => _type = type;

		void IAchievementTracker.ReportAs(string name) => _name = name;

		TrackerType IAchievementTracker.GetTrackerType() => _type;

		void IAchievementTracker.Clear() => SetValue(default);

		public void SetValue(T newValue, bool reportUpdate = true)
		{
			if (newValue.Equals(_value))
            {
                return;
            }

            _value = newValue;
			if (reportUpdate)
			{
				ReportUpdate();
				if (_value.Equals(_maxValue))
                {
                    OnComplete();
                }
            }
		}

		public abstract void ReportUpdate();

		protected abstract void Load();

        void IAchievementTracker.Load() => Load();

        //Useless.
        protected void OnComplete() { }
	}
}
