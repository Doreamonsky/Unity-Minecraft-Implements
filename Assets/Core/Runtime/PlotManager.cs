using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace MC.Core
{
    public class PlotManager : MonoBehaviour
    {
        public GameObject player;

        public GameObject runtime;

        public GameObject timeline;

        public GameObject gamePlayGuide;

        public PlayableDirector director;

        private void Start()
        {
            timeline.SetActive(true);
            runtime.SetActive(false);
            gamePlayGuide.SetActive(false);

            StartCoroutine(PlotUpdate());

            
        }

        private IEnumerator PlotUpdate()
        {
            Debug.Log((float)director.duration + (float)director.initialTime);

            yield return new WaitForSeconds((float)director.duration + (float)director.initialTime);

            timeline.SetActive(false);

            runtime.SetActive(true);
            gamePlayGuide.SetActive(true);

        
        }
    }

}
