using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MC.Core
{
    public class MainUI : MonoBehaviour
    {
        public Button PlotBtn, SurvivalBtn, ExitBtn;

        public GameObject LoadingEffect;

        private void Start()
        {
            PlotBtn.onClick.AddListener(() =>
            {
                LoadingEffect.SetActive(true);
                SceneManager.LoadScene("SampleScene");
            });

            SurvivalBtn.onClick.AddListener(() =>
            {
                LoadingEffect.SetActive(true);
                SceneManager.LoadScene("InfiniteScene");
            });
            ExitBtn.onClick.AddListener(() =>
            {
                Application.Quit();
            });

        }
    }

}
