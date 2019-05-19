using UnityEngine;

namespace MC.Core
{
    public class ControlEvents
    {
        public static System.Action<Vector2> OnClickScreen;

        public static System.Action<int> OnClickInventoryByID;

        public static System.Action<Vector2> OnPressingScreen;

        public static System.Action OnBeginPressScreen, OnEndPressScreen;

        public static System.Action<InputData> OnControllerInput, OnCameraControllerInput;

        public static System.Action OnJumped;

    }
}
