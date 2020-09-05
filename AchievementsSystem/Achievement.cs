using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CookieClicker.AchievementsSystem
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Achievement
    {
        public delegate void AchievementCompleted(Achievement achievement);

        private static int _totalAchievements;
        public readonly string Name;
        public readonly string FriendlyName;
        public readonly string Description;
        public readonly int ID = _totalAchievements++;
        private IAchievementTracker _tracker;
        [JsonProperty("Conditions")]
        private readonly Dictionary<string, AchievementCondition> _conditions = new Dictionary<string, AchievementCondition>();
        private int _completedCount;

        public AchievementCategory Category { get; private set; }

        public bool HasTracker => _tracker != null;

        public bool IsCompleted => _completedCount == _conditions.Count;

        public event AchievementCompleted OnCompleted;

        public IAchievementTracker GetTracker() => _tracker;

        public Achievement(string name, string displayName, string description)
        {
            Name = name;
			FriendlyName = displayName;
			Description = description;
        }

		public void ClearProgress()
		{
			_completedCount = 0;

			foreach (KeyValuePair<string, AchievementCondition> condition in _conditions)
			{
				condition.Value.Clear();
			}

			if (_tracker != null)
            {
				_tracker.Clear();
			}
		}

		public void Load(Dictionary<string, JObject> conditions)
		{
			foreach (KeyValuePair<string, JObject> condition in conditions)
			{
				if (_conditions.TryGetValue(condition.Key, out AchievementCondition value))
				{
					value.Load(condition.Value);

					if (value.IsCompleted)
					{
						_completedCount++;
					}
				}
			}

			if (_tracker != null)
			{
				_tracker.Load();
			}
		}

		public void AddCondition(AchievementCondition condition)
		{
			_conditions[condition.Name] = condition;
			condition.OnComplete += OnConditionComplete;
		}

		private void OnConditionComplete(AchievementCondition condition)
		{
			_completedCount++;

			if (_completedCount == _conditions.Count)
			{
                OnCompleted?.Invoke(this);
            }
		}

		private void UseTracker(IAchievementTracker tracker)
		{
			tracker.ReportAs("STAT_" + Name);
			_tracker = tracker;
		}

		public void UseTrackerFromCondition(string conditionName) => UseTracker(GetConditionTracker(conditionName));

		public void UseConditionsCompletedTracker()
		{
			ConditionsCompletedTracker conditionsCompletedTracker = new ConditionsCompletedTracker();

			foreach (KeyValuePair<string, AchievementCondition> condition in _conditions)
			{
				conditionsCompletedTracker.AddCondition(condition.Value);
			}

			UseTracker(conditionsCompletedTracker);
		}

		public void UseConditionsCompletedTracker(params string[] conditions)
		{
			ConditionsCompletedTracker conditionsCompletedTracker = new ConditionsCompletedTracker();

			foreach (string key in conditions)
			{
				conditionsCompletedTracker.AddCondition(_conditions[key]);
			}

			UseTracker(conditionsCompletedTracker);
		}

		public void ClearTracker() => _tracker = null;

		private IAchievementTracker GetConditionTracker(string name) => _conditions[name].GetAchievementTracker();

		public void AddConditions(params AchievementCondition[] conditions)
		{
			for (int i = 0; i < conditions.Length; i++)
			{
				AddCondition(conditions[i]);
			}
		}

		public AchievementCondition GetCondition(string conditionName)
		{
			if (_conditions.TryGetValue(conditionName, out AchievementCondition value))
			{
				return value;
			}

			return null;
		}

		public void SetCategory(AchievementCategory category) => Category = category;
	}
}
