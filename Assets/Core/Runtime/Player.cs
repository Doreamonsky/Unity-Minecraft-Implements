using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MC.Core {
    public struct InputData {
        public float x, y;
    }

    public class Player : MonoBehaviour {
        public static System.Action<Vector3> OnPlayerMove;

        public float health = 120;

        public float velocity = 10;

        public float jumpVelocity = 8.0f;

        public int blockID = 2;

        public bool isCreatorMode = false;

        public GameObject craftingUI;

        public InventorySystem inventorySystem;

        public Button openCraftingBtn, closeCraftingBtn;

        public Button gunFireBtn;

        public Button exit;

        public GameObject weaponBar;

        public Camera playerCamera;

        public System.Action OnUpdated;

        public Button sleepBtn;

        public Image sleepEffect;

        public Text textInfomation;
        private Vector3 moveDirection;

        //private CharacterController m_CharacterContoller;

        //private CameraController m_CameraController;

        private bool isControllable = true;

        private bool isMobile = false;

        private void Start () {
            isMobile = Util.IsMobile ();

            craftingUI.SetActive (Util.isCrafting);

            MouseLockModule.Instance.OnLock ();

            openCraftingBtn.onClick.AddListener (() => {
                ToggleCrafting ();
            });

            closeCraftingBtn.onClick.AddListener (() => {
                ToggleCrafting ();
            });

            exit.onClick.AddListener (() => {
                Util.OnRequireSave?.Invoke ();

                SceneManager.LoadScene ("Main");
            });

            Util.OnToggleCraftingMode += (state) => {
                if (state) {
                    ControlEvents.OnControllerInput (new InputData () {
                        x = 0,
                            y = 0
                    });
                }
            };

            sleepBtn.onClick.AddListener (() => {
                StartCoroutine (ToSleep ());
            });
        }

        private void Update () {
            if (Input.GetKeyDown (KeyCode.E)) {
                ToggleCrafting ();
            }

            if (!isControllable) {
                return;
            }

            if (Input.GetKeyDown (KeyCode.Space)) {
                Jump ();
            }

            if (isMobile) {

            } else {
                ControlEvents.OnControllerInput?.Invoke (new InputData () {
                    x = Input.GetAxis ("Horizontal"),
                        y = Input.GetAxis ("Vertical")
                });

                ControlEvents.OnCameraControllerInput?.Invoke (new InputData () {
                    x = Input.GetAxis ("Mouse X"),
                        y = Input.GetAxis ("Mouse Y")
                });

                if (Input.GetKey (KeyCode.Mouse0)) {
                    ControlEvents.OnPressingScreen?.Invoke (new Vector2 (Screen.width, Screen.height) * 0.5f);
                }

                if (Input.GetKeyDown (KeyCode.Mouse0)) {
                    ControlEvents.OnBeginPressScreen?.Invoke ();
                }

                if (Input.GetKeyUp (KeyCode.Mouse0)) {
                    ControlEvents.OnEndPressScreen?.Invoke ();
                }

                if (Input.GetKeyDown (KeyCode.Mouse1)) {
                    ControlEvents.OnClickScreen?.Invoke (new Vector2 (Screen.width, Screen.height) * 0.5f);
                }

            }

            //if (!isCreatorMode)
            //{
            //    m_CharacterContoller.Move(Vector3.up * gravity * Time.deltaTime);
            //}

            for (var i = 0; i < InventorySystem.max_bottom_slot_count; i++) {
                if (Input.GetKeyDown (KeyCode.Alpha1 + i)) {
                    ControlEvents.OnClickInventoryByID?.Invoke (i);
                }
            }

            if (Input.GetKeyDown (KeyCode.Alpha0)) {
                ControlEvents.OnClickInventoryByID?.Invoke (9);
            }

            OnUpdated?.Invoke ();

            OnPlayerMove?.Invoke (transform.position);
        }

        public void Jump () {
            ControlEvents.OnJumped?.Invoke ();
        }

        private void ToggleControl (bool state) {
            isControllable = state;
            //m_CameraController.enabled = state;
        }

        private void ToggleCrafting () {
            Util.isCrafting = !Util.isCrafting;

            craftingUI.SetActive (Util.isCrafting);

            if (Util.isCrafting) {
                MouseLockModule.Instance.OnUnlock ();

                ToggleControl (false);
            } else {
                MouseLockModule.Instance.OnLock ();

                ToggleControl (true);
            }

            Util.OnToggleCraftingMode?.Invoke (Util.isCrafting);
        }

        public void ApplyDamage (float damage) {
            health -= damage;

            PoolManager.CreateObject ("Hurt Sound", transform.position, Vector3.zero);
        }

        private void OnDestroy () {
            ControlEvents.CleanActions ();
        }

        private void OnApplicationPause (bool pause) {
            Util.OnRequireSave?.Invoke ();
        }

        private void OnApplicationQuit () {
            Util.OnRequireSave?.Invoke ();
        }

        private IEnumerator ToSleep () {
            float t = 0f;

            sleepEffect.gameObject.SetActive (true);

            while (true) {
                if (t < 5) {
                    var val = t / 5f;

                    sleepEffect.color = new Color (0, 0, 0, val);
                }

                yield return new WaitForEndOfFrame ();

                if (t > 7) {
                    FindObjectOfType<TOD_Sky> ().Cycle.Hour = 5f + Random.value;
                    sleepEffect.gameObject.SetActive (false);
                    yield break;
                } else {
                    t += Time.deltaTime;
                }
            }
        }
    }

}