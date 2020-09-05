using Terraria;

namespace CookieClicker.AchievementsSystem
{
    public class AchievementsHelper
    {
        public delegate void CookieClickEvent(Player player, double count);

        public delegate void GoldenCookieClickEvent(Player player, int count);

        public delegate void SugarLumpCollectEvent(Player player, int count);

        public static event CookieClickEvent OnCookieClick;
        public static event GoldenCookieClickEvent OnGoldenCookieClick;
        public static event SugarLumpCollectEvent OnSugarLumpCollect;

        public static void NotifyCookieClick(Player player, double count) => OnCookieClick?.Invoke(player, count);

        public static void NotifyGoldenCookieClick(Player player, int count) => OnGoldenCookieClick?.Invoke(player, count);

        public static void NotifySugarLumpCollect(Player player, int count) => OnSugarLumpCollect?.Invoke(player, count);

        public static void Initialize() => Player.Hooks.OnEnterWorld += OnPlayerEnteredWorld;

        internal static void OnPlayerEnteredWorld(Player player)
        {
            ClickerPlayer clickerPlayer = player.GetModPlayer<ClickerPlayer>();

            OnCookieClick?.Invoke(player, clickerPlayer.cookieCount);
            OnGoldenCookieClick?.Invoke(player, clickerPlayer.goldenCookieCount);
            OnSugarLumpCollect?.Invoke(player, clickerPlayer.sugarLumpCount);
        }
    }
}
