using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace MC.Core.AR
{
    public class ARPlaceWorld : MonoBehaviour
    {
        public Button placeWorldBtn;

        public Button resetWorldBtn;

        public Button exitGameBtn;

        public Button backHomeBtn;

        public Button enterWorldBtn;

        public Button exitWorldBtn;

        public GameObject adLayerBar;

        public Button adCancelBtn;

        public Button adPlayBtn;

        public GameObject settingBar;

        public GameObject arConfimBar;

        public GameObject detectingStateBar;

        public ARRaycastManager arRaycastManager;

        public GameObject Indicator;

        public Camera arCamera;

        public ARPointCloudManager arPointCloud;

        public ARPlaneManager arPlaneManager;

        public ARSessionOrigin arSessionOrigin;

        public InventorySystem inventorySystem;

        public Inventory[] defaultInventories;

        public BlockStorageData blockStorageData;

        private readonly TrackableType trackableType = TrackableType.PlaneWithinPolygon;

        private bool isEditingWorld = false;

        private TrackableId currentEditingTrackableID = TrackableId.invalidId;

        private WorldManager worldManager;

        private bool inGamePopAd = false;

        private void Start()
        {
            worldManager = new GameObject("World Manager", typeof(WorldManager)).GetComponent<WorldManager>();

            worldManager.InstancingRenderer = true;
            worldManager.blockStorageData = blockStorageData;

            var mapData = ScriptableObject.CreateInstance<MapData>();
            mapData.mapName = MainUI.activeMapStorage.MapName;
            mapData.isSaveable = true;
            mapData.OnLoad();

            worldManager.mapData = mapData;

            arConfimBar.SetActive(true);

            worldManager.OnLoaded += () =>
            {
                //#if UNITY_EDITOR

                //#else
                worldManager.gameObject.SetActive(false);
                //#endif
            };

            placeWorldBtn.onClick.AddListener(() =>
            {
                var hits = new List<ARRaycastHit>();

                arRaycastManager.Raycast(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f), hits, trackableType);

                if (hits.Count > 0)
                {
                    worldManager.gameObject.SetActive(true);

                    worldManager.transform.rotation = hits[0].pose.rotation;
                    worldManager.transform.position = hits[0].pose.position - Vector3.up * WorldManager.scaleSize * worldManager.mapData.grassHeight - 0.5f * worldManager.transform.right * worldManager.mapData.max_width * WorldManager.scaleSize - 0.5f * worldManager.transform.forward * worldManager.mapData.max_length * WorldManager.scaleSize;

                    placeWorldBtn.gameObject.SetActive(false);
                    isEditingWorld = true;

                    currentEditingTrackableID = hits[0].trackableId;
                }
            });

            enterWorldBtn.onClick.AddListener(() =>
            {
                ResetWorldPlacement();

                arSessionOrigin.transform.localScale = new Vector3(4, 4, 4);

                exitWorldBtn.gameObject.SetActive(true);
            });

            exitWorldBtn.onClick.AddListener(() =>
            {
                ResetWorldPlacement();

                arSessionOrigin.transform.localScale = new Vector3(40, 40, 40);

                exitWorldBtn.gameObject.SetActive(false);
            });

            resetWorldBtn.onClick.AddListener(() =>
            {
                ResetWorldPlacement();

                settingBar.SetActive(false);
            });

            backHomeBtn.onClick.AddListener(() =>
            {
                AchievementManager.Data.OnValueChanged?.Invoke();

                worldManager.SaveMapData();

                SceneManager.LoadScene("Main");
            });

            exitGameBtn.onClick.AddListener(() =>
            {
                Application.Quit();
            });

            UpdatePlaneTipUI();

            arPlaneManager.planesChanged += args =>
            {
                UpdatePlaneTipUI();

                if (isEditingWorld)
                {
                    ToggleVisualization(false);
                }
            };

            StartCoroutine(LoadInventory());

            adPlayBtn.onClick.AddListener(() =>
            {
                Advertisement.Show("GetCoin");
                AchievementManager.Data.Coin += 5;

                adLayerBar.SetActive(false);

                inGamePopAd = true;
            });

            adCancelBtn.onClick.AddListener(() =>
            {
                adLayerBar.SetActive(false);

                inGamePopAd = true;
            });
        }

        private IEnumerator LoadInventory()
        {
            yield return new WaitForSeconds(0.25f);

            inventorySystem.inventoryStorageList = AchievementManager.Data.inventoryStorageList;
            inventorySystem.UpdateInvetoryUI();

            CraftSystem.Instance.craftInventoryList = AchievementManager.Data.craftStorageList;
            CraftSystem.Instance.UpdateInvetoryUI();
        }


        private void ResetWorldPlacement()
        {
            isEditingWorld = false;
            currentEditingTrackableID = TrackableId.invalidId;

            worldManager.gameObject.SetActive(false);
            ToggleVisualization(true);
        }

        private void ToggleVisualization(bool state)
        {
            var points = arPointCloud.trackables;

            foreach (var point in points)
            {
                point.gameObject.SetActive(state);
            }

            arPointCloud.pointCloudPrefab.gameObject.SetActive(state);
            //arPointCloud.enabled = state;

            var planes = arPlaneManager.trackables;

            foreach (var plane in planes)
            {
                plane.gameObject.SetActive(state);
            }

            arPlaneManager.planePrefab.gameObject.SetActive(state);
            //arPlaneManager.enabled = state;

            if (currentEditingTrackableID != TrackableId.invalidId)
            {
                planes[currentEditingTrackableID].gameObject.SetActive(true);
            }
        }

        private void UpdatePlaneTipUI()
        {
            if (arPlaneManager.trackables.count > 0)
            {
                if (!isEditingWorld)
                {
                    placeWorldBtn.gameObject.SetActive(true);
                }

                detectingStateBar.gameObject.SetActive(false);
            }
            else
            {
                placeWorldBtn.gameObject.SetActive(false);
                detectingStateBar.gameObject.SetActive(true);
            }
        }

        private void Update()
        {
            var hits = new List<ARRaycastHit>();

            arRaycastManager.Raycast(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f), hits, trackableType);

            if (hits.Count > 0 && !isEditingWorld)
            {
                Indicator.SetActive(true);

                Indicator.transform.SetPositionAndRotation(hits[0].pose.position, hits[0].pose.rotation);
            }
            else
            {
                Indicator.SetActive(false);
            }

            if (!inGamePopAd && Time.time > 60)
            {
                if (Advertisement.GetPlacementState("GetCoin") == PlacementState.Ready)
                {
                    adLayerBar.SetActive(true);
                }
            }

            //Debug.Log(hits.Count);
        }

        private void OnApplicationQuit()
        {
            worldManager.SaveMapData();
        }

        private void OnApplicationPause(bool pause)
        {
            worldManager.SaveMapData();
            AchievementManager.Data.OnValueChanged?.Invoke();
        }
    }

}
