using UnityEngine;
namespace MC.Core
{
    public class MouseLockModule : MonoBehaviour
    {
        private static MouseLockModule instance;

        public static bool isLock = false;

        public static MouseLockModule Instance { get => instance == null ? new GameObject("MouseLockModule", typeof(MouseLockModule)).GetComponent<MouseLockModule>() : instance; set => instance = value; }

        private bool isMobile = false;

        private void Start()
        {
            instance = this;

            //|| Application.platform == RuntimePlatform.WindowsEditor
            if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WP8Player || Application.platform == RuntimePlatform.WSAPlayerARM || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                isMobile = true;
            }
        }

        public void OnLock()
        {
            isLock = true;
        }
        public void OnUnlock()
        {
            isLock = false;
        }

        public void LockReverse()
        {
            isLock = !isLock;
        }

        private void Update()
        {
            if (isMobile)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                LockReverse();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LockReverse();
            }
        }
        private void OnDestroy()
        {
            OnUnlock();
            OnGUI();
        }
        private void OnGUI()
        {
            if (isMobile)
            {
                return;
            }

            if (isLock)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

    }
}