using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace MC.Core.AR
{
    public class ARPlaceWorld : MonoBehaviour
    {
        public Button placeWorldBtn;

        public WorldManager worldManager;

        public ARRaycastManager arRaycastManager;

        private void Start()
        {
            placeWorldBtn.onClick.AddListener(() =>
            {
                var hits = new List<ARRaycastHit>();

                arRaycastManager.Raycast(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f), hits, TrackableType.All);

                if (hits.Count > 0)
                {
                    worldManager.transform.position = hits[0].pose.position - Vector3.up * 15;
                    worldManager.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

                    placeWorldBtn.interactable = false;
                }
            });
        }
    }

}
