using System;
using System.Collections.Generic;

namespace CookieClicker.AchievementsSystem
{
	public class ConditionsCompletedTracker : ConditionIntTracker
	{
		private readonly List<AchievementCondition> _conditions = new List<AchievementCondition>();

		public void AddCondition(AchievementCondition condition)
		{
			_maxValue++;
			condition.OnComplete += OnConditionCompleted;
			_conditions.Add(condition);
		}

		private void OnConditionCompleted(AchievementCondition condition) => SetValue(Math.Min(_value + 1, _maxValue));

		protected override void Load()
		{
			for (int i = 0; i < _conditions.Count; i++)
			{
				if (_conditions[i].IsCompleted)
				{
					_value++;
				}
			}
		}
	}
}
