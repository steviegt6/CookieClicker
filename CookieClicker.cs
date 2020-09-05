using CookieClicker.AchievementsSystem;
using CookieClicker.UI;
using CookieClicker.Utilities;
using Newtonsoft.Json;
using System;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace CookieClicker
{
    public class CookieClicker : Mod
	{
        public static UIAchievementsMenu AchievementsMenu = new UIAchievementsMenu();

        internal static ModHotKey OpenCookieClickerUI;
        internal static CookieClicker instance;
        internal CookieClickerUI clickerUI;

        private AchievementManager _achievements;

        public static AchievementManager Achievements => instance._achievements;

        public override void Load()
        {
            instance = this;
            _achievements = new AchievementManager();
            OpenCookieClickerUI = RegisterHotKey("Open Cookie Clicker UI", "C");

            if (!Main.dedServ)
            {
                clickerUI = new CookieClickerUI();
            }

            //Adding our achievement tag handler. Cleared on Unload().
            ChatManager.Register<AchievementTagHandler>(new string[2]
            {
                "ca",
                "cookieachievement"
            });

            Hooks.On_AddMenuButtons += Hooks_On_AddMenuButtons;
            On.Terraria.Player.SavePlayer += Player_SavePlayer;
        }

        public override void PostSetupContent() => AchievementInitializer.Load();

        public override void Unload()
        {
            clickerUI.Deactivate();

            instance = null;
            OpenCookieClickerUI = null;
            clickerUI = null;
        }

        private void Player_SavePlayer(On.Terraria.Player.orig_SavePlayer orig, PlayerFileData playerFile, bool skipMapSave)
        {
            orig(playerFile, skipMapSave);

            Achievements.Save();
        }

        private void Hooks_On_AddMenuButtons(Hooks.Orig_AddMenuButtons orig, Main main, int selectedMenu, string[] buttonNames, float[] buttonScales, ref int offY, ref int spacing, ref int buttonIndex, ref int numButtons)
        {
            MenuUtils.AddButton(Language.GetTextValue("UI.Achievements"), delegate
            {
                Main.MenuUI.SetState(AchievementsMenu);
                Main.menuMode = 888;
            }, selectedMenu, buttonNames, ref buttonIndex, ref numButtons);

            orig(main, selectedMenu, buttonNames, buttonScales, ref offY, ref spacing, ref buttonIndex, ref numButtons);
        }
    }
}