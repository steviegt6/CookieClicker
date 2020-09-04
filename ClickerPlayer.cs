using CookieClicker.UI;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace CookieClicker
{
    public class ClickerPlayer : ModPlayer
    {
        public bool clickerUIActivated = false;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (CookieClicker.OpenCookieClickerUI.JustPressed)
            {
                if (!clickerUIActivated)
                {
                    CookieClicker.instance.clickerUI.Activate();
                    clickerUIActivated = true;
                }
                else
                {
                    CookieClicker.instance.clickerUI.Deactivate();
                    clickerUIActivated = false;
                }
            }
        }
    }
}
