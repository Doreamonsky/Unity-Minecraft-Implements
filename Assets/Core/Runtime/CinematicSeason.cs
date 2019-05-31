using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC.Core
{
    public class CinematicSeason : MonoBehaviour
    {
        public enum Season
        {
            Spring,
            Summer,
            Autumn,
            Winter
        }

        [System.Serializable]
        public class ReplaceTex
        {
            public Season season;
            public Material sourceMat, targetMat;
        }

        private Season currentSeason = Season.Autumn;

        public Season CurrentSeason { get => currentSeason; set { currentSeason = value; OnSeasonChanged(value); } }

        public List<ReplaceTex> texList = new List<ReplaceTex>();

        public bool isAutoStart = false;

        public void OnSeasonChanged(Season newSeason)
        {
            foreach (var meshRenderer in GameObject.FindObjectsOfType<MeshRenderer>())
            {
                var desireMat = texList.Find(val => $"{ val.sourceMat.name  }_Temp" == meshRenderer.gameObject.name && val.season == newSeason);

                if (desireMat != null)
                {
                    meshRenderer.sharedMaterial = desireMat.targetMat;
                }
            }
        }

        private void Start()
        {
            if (isAutoStart)
            {
                StartCoroutine(AutoPlay());
            }
        }

        private IEnumerator AutoPlay()
        {
            yield return new WaitForEndOfFrame();

            OnSeasonChanged(currentSeason);
        }
    }

}
