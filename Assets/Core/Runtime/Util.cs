using UnityEngine;

namespace MC.Core
{
    public static class Util
    {
        public static System.Action OnRequireSave;

        public static System.Action<bool> OnToggleCraftingMode;

        public static bool isCrafting = false;

        public static bool isTouchingScreen = false;

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

    }
}
