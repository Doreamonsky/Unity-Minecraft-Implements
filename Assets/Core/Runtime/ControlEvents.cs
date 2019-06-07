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

        public static System.Action OnGunFire;

        public static void CleanActions()
        {
            OnClickScreen = null;
            OnClickInventoryByID = null;
            OnPressingScreen = null;
            OnBeginPressScreen = null;
            OnEndPressScreen = null;
            OnControllerInput = null;
            OnCameraControllerInput = null;
            OnJumped = null;
            OnGunFire = null;

            InventoryIconUI.OnPointerClicked = null;
            InventoryIconUI.OnPointerEntered = null;
            InventoryIconUI.OnPointerExited = null;

            Player.OnPlayerMove = null;

            Util.OnToggleCraftingMode = null;
            Util.OnRequireSave = null;

            SwapManager.OnAllocateItem = null;
            SwapManager.OnSwapItem = null;
        }
    }
}
