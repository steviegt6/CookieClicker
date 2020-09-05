using System;
using System.Threading;

namespace CookieClicker.Utilities
{
    public static class FileUtils
    {
        public static void ProtectedInvoke(Action action)
        {
            bool isBackground = Thread.CurrentThread.IsBackground;

            try
            {
                Thread.CurrentThread.IsBackground = false;
                action();
            }
            finally
            {
                Thread.CurrentThread.IsBackground = isBackground;
            }
        }
    }
}
