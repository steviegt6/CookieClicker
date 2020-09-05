using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terraria;

namespace CookieClicker.AchievementsSystem
{
	public class CustomDoubleCondition : AchievementCondition
	{
		[JsonProperty("Value")]
		private double _value;
		private readonly double _maxValue;

		public double Value
		{
			get
			{
				return _value;
			}
			set
			{
				double num = Utils.Clamp(value, 0f, _maxValue);
				if (_tracker != null)
                {
                    ((ConditionDoubleTracker)_tracker).SetValue(num);
                }

                _value = num;
				if (_value == _maxValue)
                {
                    Complete();
                }
            }
		}

		private CustomDoubleCondition(string name, double maxValue)
			: base(name)
		{
			_maxValue = maxValue;
			_value = 0f;
		}

		public override void Clear()
		{
			_value = 0f;
			base.Clear();
		}

		public override void Load(JObject state)
		{
			base.Load(state);
			_value = (double)state["Value"];
			if (_tracker != null)
            {
                ((ConditionDoubleTracker)_tracker).SetValue(_value, reportUpdate: false);
            }
        }

		protected override IAchievementTracker CreateAchievementTracker() => new ConditionDoubleTracker(_maxValue);
		public static AchievementCondition Create(string name, double maxValue) => new CustomDoubleCondition(name, maxValue);

		public override void Complete()
		{
			if (_tracker != null)
            {
                ((ConditionDoubleTracker)_tracker).SetValue(_maxValue);
            }

            _value = _maxValue;
			base.Complete();
		}
	}
}
