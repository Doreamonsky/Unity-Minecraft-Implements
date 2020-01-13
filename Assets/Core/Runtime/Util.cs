using System;

namespace MC.Core
{
    public static class Util
    {
        public static System.Action onRequireMapSave;

        public static System.Action<bool> OnToggleCraftingMode;

        public static bool isCrafting = false;

        public static bool isTouchingScreen = true;

        public static bool IsMobile()
        {
            if (isTouchingScreen)
            {
                return true;
            }
            else
            {
                return false;
            }

            //var isMobile = false;

            //if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WSAPlayerARM || Application.platform == RuntimePlatform.IPhonePlayer)
            //{
            //    isMobile = true;
            //}

            //return isMobile;
        }

        public static string GetTimeStamp()
        {
            return Convert.ToInt64(DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds).ToString();
        }

        public static DateTime GetDateTimeFromTimeSpan(string timeSpan)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(long.Parse(timeSpan));

        }
    }
}
