using Terraria;
using Terraria.GameContent.UI.States;
using Terraria.GameInput;
using Terraria.UI.Gamepad;
using Terraria.ID;

namespace CookieClicker.AchievementsSystem
{
	public class IngameFancyUI
	{
		public static void OpenAchievements()
		{
			Main.playerInventory = false;
			Main.editChest = false;
			Main.npcChatText = "";
			Main.inFancyUI = true;
			ClearChat();
			Main.InGameUI.SetState(Main.AchievementsMenu);
		}

		public static void OpenAchievementsAndGoto(Achievement achievement)
		{
			OpenAchievements();
			CookieClicker.AchievementsMenu.GotoAchievement(achievement);
		}

		private static void ClearChat()
		{
			Main.drawingPlayerChat = false;
			PlayerInput.WritingText = true;
			Main.player[Main.myPlayer].releaseHook = false;
			Main.player[Main.myPlayer].releaseThrow = false;
			Main.chatText = "";
		}

		public static void Close()
		{
			Main.inFancyUI = false;
			Main.PlaySound(SoundID.MenuClose);
			bool flag = !Main.gameMenu;
			bool flag2 = !(Main.InGameUI.CurrentState is UIVirtualKeyboard);
			bool flag3 = false;
			int keyboardContext = UIVirtualKeyboard.KeyboardContext;
			if ((uint)(keyboardContext - 2) <= 1u)
            {
                flag3 = true;
            }

            if (flag && !(flag2 || flag3))
            {
                flag = false;
            }

            if (flag)
            {
                Main.playerInventory = true;
            }

            Main.LocalPlayer.releaseInventory = false;
			Main.InGameUI.SetState(null);
			UILinkPointNavigator.Shortcuts.FANCYUI_SPECIAL_INSTRUCTIONS = 0;
		}
	}
}
