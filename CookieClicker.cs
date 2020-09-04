using CookieClicker.UI;
using Terraria;
using Terraria.ModLoader;

namespace CookieClicker
{
	public class CookieClicker : Mod
	{
        internal static ModHotKey OpenCookieClickerUI;

        internal static CookieClicker instance;

        internal CookieClickerUI clickerUI;

        public CookieClicker() => Properties = new ModProperties { Autoload = false, AutoloadBackgrounds = false, AutoloadGores = false, AutoloadSounds = false };

        public override void Load()
        {
            instance = this;

            OpenCookieClickerUI = RegisterHotKey("Cookie Clicker UI", "C");

            if (!Main.dedServ)
            {
                clickerUI = new CookieClickerUI();
            }
        }

        public override void Unload()
        {
            clickerUI.Deactivate();

            instance = null;
            OpenCookieClickerUI = null;
            clickerUI = null;
        }
    }
}