using System;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CookieClicker
{
    public class ClickerPlayer : ModPlayer
    {
        public DateTime exitTime;
        public double cookieCount;
        public float cookiesPerSecond;
        public int timeOffMultiplier;
        public int clicksPerSecond;
        public int cookiesPerClick;
        public int goldenCookieCount;
        public int sugarLumpCount;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (CookieClicker.OpenCookieClickerUI.JustPressed)
            {
                Main.NewText(timeOffMultiplier);
                CookieClicker.instance.clickerUI.Activate();
            }
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                { "cookieCount", cookieCount },
                { "cookiesPerSecond", cookiesPerSecond },
                { "goldenCookieCount", goldenCookieCount },
                { "sugarLumpCount", sugarLumpCount },
                { "exitTime", DateTime.Now.ToString() }
            };
        }

        public override void Load(TagCompound tag)
        {
            cookieCount = tag.GetDouble("cookieCount");
            cookiesPerSecond = tag.GetFloat("cookiesPerSecond");
            goldenCookieCount = tag.GetInt("goldenCookieCount");
            sugarLumpCount = tag.GetInt("sugerLumpCount");
            timeOffMultiplier = (int)(DateTime.Now - Convert.ToDateTime(tag.GetString("exitTime"))).TotalSeconds;
        }
    }
}
