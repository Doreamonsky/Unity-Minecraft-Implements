using UnityEngine;
using UnityEngine.UI;

namespace MC.Core
{
    public struct InputData
    {
        public float x, y;
    }

    public class Player : MonoBehaviour
    {

        public float velocity = 10;

        public float jumpVelocity = 8.0f;

        public float gravity = -5f;

        public int blockID = 2;

        public bool isCreatorMode = false;

        public GameObject craftingUI;

        public InventorySystem inventorySystem;

        public Button openCraftingBtn, closeCraftingBtn;

        public Button gunFireBtn;

        public GameObject weaponBar;

        public System.Action OnUpdated;

        private Vector3 moveDirection;

        //private CharacterController m_CharacterContoller;

        //private CameraController m_CameraController;

        private float mobileX, mobileY;

        private bool isControllable = true;

        private bool isMobile = false;

        private void Start()
        {
            isMobile = Util.IsMobile();

            //m_CharacterContoller = GetComponent<CharacterController>();
            //m_CameraController = GetComponent<CameraController>();

            craftingUI.SetActive(Util.isCrafting);

            MouseLockModule.Instance.OnLock();

            //ControlEvents.OnControllerInput += data =>
            //{
            //    if (!isControllable)
            //    {
            //        return;
            //    }

            //    var delta = m_CameraController.m_Camera.transform.forward * data.y + m_CameraController.m_Camera.transform.right * data.x;

            //    if (!isCreatorMode)
            //    {
            //        delta = Vector3.ProjectOnPlane(delta, Vector3.up);
            //    }

            //    m_CharacterContoller.Move(delta * Time.deltaTime * velocity);
            //};

            openCraftingBtn.onClick.AddListener(() =>
            {
                ToggleCrafting();
            });

            closeCraftingBtn.onClick.AddListener(() =>
            {
                ToggleCrafting();
            });


        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleCrafting();
            }

            if (!isControllable)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            if (isMobile)
            {
                ControlEvents.OnControllerInput?.Invoke(new InputData()
                {
                    x = mobileX,
                    y = mobileY
                });
            }
            else
            {
                ControlEvents.OnControllerInput?.Invoke(new InputData()
                {
                    x = Input.GetAxis("Horizontal"),
                    y = Input.GetAxis("Vertical")
                });

                ControlEvents.OnCameraControllerInput?.Invoke(new InputData()
                {
                    x = Input.GetAxis("Mouse X"),
                    y = Input.GetAxis("Mouse Y")
                });

                if (Input.GetKey(KeyCode.Mouse0))
                {
                    ControlEvents.OnPressingScreen?.Invoke(new Vector2(Screen.width, Screen.height) * 0.5f);
                }

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    ControlEvents.OnBeginPressScreen?.Invoke();
                }

                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    ControlEvents.OnEndPressScreen?.Invoke();
                }

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    ControlEvents.OnClickScreen?.Invoke(new Vector2(Screen.width, Screen.height) * 0.5f);
                }


            }


            //if (!isCreatorMode)
            //{
            //    m_CharacterContoller.Move(Vector3.up * gravity * Time.deltaTime);
            //}

            for (var i = 0; i < 10; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    ControlEvents.OnClickInventoryByID?.Invoke(i);
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                ControlEvents.OnClickInventoryByID?.Invoke(9);
            }

            OnUpdated?.Invoke();
        }

        public void Jump()
        {
            ControlEvents.OnJumped?.Invoke();
        }

        private void ToggleControl(bool state)
        {
            isControllable = state;
            //m_CameraController.enabled = state;
        }

        private void ToggleCrafting()
        {
            Util.isCrafting = !Util.isCrafting;

            craftingUI.SetActive(Util.isCrafting);

            if (Util.isCrafting)
            {
                MouseLockModule.Instance.OnUnlock();

                ToggleControl(false);
            }
            else
            {
                MouseLockModule.Instance.OnLock();

                ToggleControl(true);
            }

            Util.OnToggleCraftingMode?.Invoke(Util.isCrafting);
        }

        public void StartForward()
        {
            mobileY = 1;
        }
        public void CancelForward()
        {
            mobileY = 0;
        }
        public void StartBack()
        {
            mobileY = -1;
        }
        public void CancelBack()
        {
            mobileY = 0;
        }
        public void StartRight()
        {
            mobileX = 1;
        }
        public void CancelRight()
        {
            mobileX = 0;
        }
        public void StartLeft()
        {
            mobileX = -1;
        }
        public void CancelLeft()
        {
            mobileX = 0;
        }


        public void StartForwardRight()
        {
            mobileX = 1;
            mobileY = 1;
        }
        public void CancelForwardRight()
        {
            mobileX = 0;
            mobileY = 0;
        }
        public void StartForwardLeft()
        {
            mobileX = -1;
            mobileY = 1;
        }
        public void CancelForwardLeft()
        {
            mobileX = 0;
            mobileY = 0;
        }
    }

}
