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

        public GameObject Indicator;

        public BlockData[] blocks;

        public Camera arCamera;

        public ARPointCloudManager arPointCloud;

        public ARPlaneManager arPlaneManager;

        public MobileTouchBar touchBar;

        public InventorySystem inventorySystem;

        public Inventory[] defaultInventories;

        private readonly TrackableType trackableType = TrackableType.PlaneWithinPolygon;

        private void Awake()
        {
            inventorySystem.inventoryStorageList = new List<InventoryStorage>();

            for (var i = 0; i < defaultInventories.Length; i++)
            {
                var inventory = defaultInventories[i];

                inventorySystem.inventoryStorageList.Add(new InventoryStorage()
                {
                    count = 100,
                    inventory = inventory,
                    inventoryName = inventory.inventoryName,
                    slotID = i
                });
            }
        }

        private void Start()
        {
            placeWorldBtn.onClick.AddListener(() =>
            {
                var hits = new List<ARRaycastHit>();

                arRaycastManager.Raycast(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f), hits, trackableType);

                if (hits.Count > 0)
                {
                    worldManager.transform.position = hits[0].pose.position - Vector3.up * WorldManager.scaleSize * worldManager.mapData.grassHeight;
                    worldManager.transform.rotation = hits[0].pose.rotation;
                //placeWorldBtn.interactable = false;
            }
            });
        }

        private void Update()
        {
            var hits = new List<ARRaycastHit>();

            arRaycastManager.Raycast(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f), hits, trackableType);

            if (hits.Count > 0)
            {
                Indicator.SetActive(true);

                Indicator.transform.SetPositionAndRotation(hits[0].pose.position, hits[0].pose.rotation);
            }
            else
            {
                Indicator.SetActive(false);
            }

            //Debug.Log(hits.Count);
        }
    }

}
