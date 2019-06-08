using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MC.Core {
    public class MainUI : MonoBehaviour {
        public Button PlotBtn, SurvivalBtn, ExitBtn, CleanData;

        public Toggle TouchToggle;

        public GameObject LoadingEffect;

        private void Start () {
            PlotBtn.onClick.AddListener (() => {
                LoadingEffect.SetActive (true);
                SceneManager.LoadScene ("SampleScene");
            });

            SurvivalBtn.onClick.AddListener (() => {
                LoadingEffect.SetActive (true);
                SceneManager.LoadScene ("InfiniteScene");
            });

            ExitBtn.onClick.AddListener (() => {
                Application.Quit ();
            });

            TouchToggle.onValueChanged.AddListener (val => {
                Util.isTouchingScreen = val;
                PlayerPrefs.SetString ("Touching", val.ToString ());

                var text = TouchToggle.transform.Find ("Label").GetComponent<Text> ();

                if (val) {
                    text.text = "触屏控制";
                } else {
                    text.text = "键鼠控制";
                }
            });

            var isTouching = PlayerPrefs.GetString ("Touching", true.ToString ()) == true.ToString ();

            TouchToggle.isOn = isTouching;

            if (PlayerPrefs.HasKey ("Plot")) {
                SurvivalBtn.interactable = true;
            }

            CleanData.onClick.AddListener (() => {
                PlayerPrefs.DeleteAll ();
                GeneralStorageSystem.DeleteFolder ();
            });

#if UNITY_EDITOR
            SurvivalBtn.interactable = true;
            SurvivalBtn.colors = new ColorBlock () {
                normalColor = Color.red
            };
#else
            CleanData.interactable = false;
#endif

#if UNITY_IOS

            var isIpad = Device.generation.ToString ().ToLower ().Contains ("ipad");

            if (isIpad) {
                var screenWidth = PlayerPrefs.GetInt ("ScreenWidth", Screen.width);
                var screenHeight = PlayerPrefs.GetInt ("ScreenHeight", Screen.height);

                if (!PlayerPrefs.HasKey ("ScreenWidth")) {
                    PlayerPrefs.SetInt ("ScreenWidth", screenWidth);
                    PlayerPrefs.SetInt ("ScreenHeight", screenHeight);
                }
                Screen.SetResolution (Mathf.RoundToInt (screenWidth * 0.6f), Mathf.RoundToInt (screenHeight * 0.6f), true);
            }
#endif

        }
    }

}