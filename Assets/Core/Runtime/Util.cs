using UnityEngine;

namespace MC.Core
{
    public static class Util
    {
        public static bool IsMobile()
        {
            var isMobile = false;

            if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WSAPlayerARM || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                isMobile = true;
            }

            //return isMobile;
            return true;
        }
    }
}
