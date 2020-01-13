using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

namespace MC.Core
{
    [System.Serializable]
    public class ShopItem
    {
        public string itemName;
        public string itemDescription;
        public string purchaseType;
        public System.Action OnPurchased;
        public System.Action<GameObject> OnInitialized;
    }

    [System.Serializable]
    public class MapItem
    {
        public string mapName;
        public string mapSize;
        public Sprite mapIcon;
        public int landCost;
        public MapData mapData;
    }


    public class UnityADCallBack : IUnityAdsListener
    {
        public void OnUnityAdsDidError(string message)
        {
            MainUI.instance.PushPopup(message, state => { });
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            switch (placementId)
            {
                case "GetCoin":
                    AchievementManager.Data.Coin += 2;
                    MainUI.instance.UpdateResources();
                    MainUI.instance.PushPopup("获得2金币!", state => { }, false);
                    break;
                case "GetLand":
                    AchievementManager.Data.Land += 1;
                    MainUI.instance.UpdateResources();
                    MainUI.instance.PushPopup("获得1空地!", state => { }, false);
                    break;
            }
        }

        public void OnUnityAdsDidStart(string placementId)
        {
        }

        public void OnUnityAdsReady(string placementId)
        {

        }
    }

    public class MainUI : MonoBehaviour
    {
        public static UserMapStorage activeMapStorage;

        public List<MapItem> defaultMapItems = new List<MapItem>();

        [Header("PopUp")]
        public GameObject popBar;
        public Text popText;
        public Button popConfirm;
        public Button popCancel;

        [Space(10)]
        [Header("Templates")]
        public GameObject createMapTemplate, continueMapTemplate;

        [Space(10)]
        [Header("ShopItem")]
        public GameObject shopTemplate;

        public List<Sprite> shopItemIcons = new List<Sprite>();
        private readonly List<ShopItem> shopItems = new List<ShopItem>();

        [Space(50)]
        public Text coinText;
        public Text landText;

        public GameObject shopBar;

        [Space(50)]
        public Button TutorialBtn;
        public Button AchievementBtn;
        public Button ExitGameBtn;

        public GameObject runtimeSupport;

        public GameObject loadingBar;

        public Text versionText;

        public static MainUI instance;

        private readonly UnityADCallBack unityADCallBack = new UnityADCallBack();

        private void Awake()
        {
            instance = this;
        }

        private IEnumerator ShowBanner()
        {
            yield return new WaitForSeconds(5);

            if (Advertisement.Banner.isLoaded)
            {
                Advertisement.Banner.Show();
            }
        }
        private void Start()
        {
            DontDestroyOnLoad(Instantiate(runtimeSupport));
            //try
            //{
            //    var javaClass = new AndroidJavaObject("com.ShanghaiWindy.CraftingARLib.MainActivity");
            //    javaClass.Call("InitializeSDK");
            //}
            //catch(System.Exception exception)
            //{
            //    Debug.Log(exception.Message);
            //}

            //Debug.Log()
            if (Application.platform == RuntimePlatform.Android)
            {
                Advertisement.Initialize("3428103", false);
            }
            else
            {
                Advertisement.Initialize("3428102", false);
            }

            Advertisement.AddListener(unityADCallBack);

            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_RIGHT);

            StartCoroutine(ShowBanner());

            shopTemplate.SetActive(false);

            // 0 
            shopItems.Add(new ShopItem()
            {
                itemName = "2 金币",
                itemDescription = "用于购买建造素材",
                purchaseType = "看广告获得",
                OnPurchased = () =>
                {
                    var state = Advertisement.GetPlacementState("GetCoin");

                    if (state == PlacementState.Ready)
                    {
                        Advertisement.Show("GetCoin");
                    }
                    else
                    {
                        PushPopup("广告尚未准备完毕，请稍后重试!", r => { }, false);
                    }
                }
            });

            // 1
            shopItems.Add(new ShopItem()
            {
                itemName = "1 空地",
                itemDescription = "用于创建地形",
                purchaseType = "看广告获得",
                OnPurchased = () =>
                {
                    var state = Advertisement.GetPlacementState("GetLand");

                    if (state == PlacementState.Ready)
                    {
                        Advertisement.Show("GetLand");
                    }
                    else
                    {
                        PushPopup("广告尚未准备完毕，请稍后重试!", r => { }, false);
                    }
                }
            });

            // 2
            shopItems.Add(new ShopItem()
            {
                itemName = "4 金币",
                itemDescription = "用于购买建造素材",
                purchaseType = "B站关注",
                OnPurchased = () =>
                {
                    if (AchievementManager.Data.isBiliBiliFollower)
                    {
                        PushPopup("已经领取过此奖励!", state => { }, false);
                    }
                    else
                    {
                        Application.OpenURL("https://space.bilibili.com/4738690");

                        AchievementManager.Data.isBiliBiliFollower = true;
                        AchievementManager.Data.Coin += 4;

                        instance.UpdateResources();
                        PushPopup("获得4金币!", state => { }, false);
                    }
                },
                OnInitialized = (instance) =>
                {
                    if (AchievementManager.Data.isBiliBiliFollower)
                    {
                        instance.SetActive(false);
                    }
                }
            });

            // 3
            shopItems.Add(new ShopItem()
            {
                itemName = "1 空地",
                itemDescription = "用于创建地形",
                purchaseType = "2 金币购买",
                OnPurchased = () =>
                {
                    if (AchievementManager.Data.Coin - 2 < 0)
                    {
                        PushPopup("金币不足!", r => { }, false);
                        return;
                    }

                    PushPopup("是否花2金币购买1空地?", state =>
                    {
                        if (state)
                        {
                            AchievementManager.Data.Coin -= 2;
                            AchievementManager.Data.Land += 1;
                            PushPopup("获得1空地!", r => { }, false);

                            UpdateResources();
                        }
                    });
                }
            });

            // 4
            shopItems.Add(new ShopItem()
            {
                itemName = "60 木头",
                itemDescription = "用于放置/原料",
                purchaseType = "1 金币购买",
                OnPurchased = () =>
                {
                    if (AchievementManager.Data.Coin - 1 < 0)
                    {
                        PushPopup("金币不足!", r => { }, false);
                        return;
                    }

                    PushPopup("是否花1金币购买60木头?", state =>
                    {
                        if (state)
                        {
                            var isPurchased = AchievementManager.Data.AddInv("Oak Wood", 60);

                            if (isPurchased)
                            {
                                AchievementManager.Data.Coin -= 1;
                                PushPopup("获得60木头!", r => { }, false);
                                UpdateResources();
                            }
                            else
                            {
                                PushPopup("背包空间不足!", r => { }, false);
                            }
                        }
                    });
                }
            });

            // 5
            shopItems.Add(new ShopItem()
            {
                itemName = "40 石头",
                itemDescription = "用于放置/原料",
                purchaseType = "1 金币购买",
                OnPurchased = () =>
                {
                    if (AchievementManager.Data.Coin - 1 < 0)
                    {
                        PushPopup("金币不足!", r => { }, false);
                        return;
                    }

                    PushPopup("是否花1金币购买40石头??", state =>
                    {
                        if (state)
                        {
                            var isPurchased = AchievementManager.Data.AddInv("Stone", 40);

                            if (isPurchased)
                            {
                                AchievementManager.Data.Coin -= 1;
                                PushPopup("获得40石头!", r => { }, false);
                                UpdateResources();
                            }
                            else
                            {
                                PushPopup("背包空间不足!", r => { }, false);
                            }
                        }
                    });
                }
            });

            // 6 
            shopItems.Add(new ShopItem()
            {
                itemName = "1 手枪",
                itemDescription = "用于真人CS?",
                purchaseType = "10 金币购买",
                OnPurchased = () =>
                {
                    if (AchievementManager.Data.Coin - 10 < 0)
                    {
                        PushPopup("金币不足!", r => { }, false);
                        return;
                    }

                    PushPopup("是否花10金币购买1把手枪?", state =>
                    {
                        if (state)
                        {
                            var isPurchased = AchievementManager.Data.AddInv("Hand Gun", 1);

                            if (isPurchased)
                            {
                                AchievementManager.Data.Coin -= 10;
                                PushPopup("获得1 手枪!", r => { }, false);
                                UpdateResources();
                            }
                            else
                            {
                                PushPopup("背包空间不足!", r => { }, false);
                            }
                        }
                    });
                }
            });

            for (int i = 0; i < shopItems.Count; i++)
            {
                ShopItem shopItem = shopItems[i];

                var instance = Instantiate(shopTemplate, shopTemplate.transform.parent, true);
                instance.transform.Find("Content/Icon").GetComponent<Image>().sprite = shopItemIcons[i];
                instance.transform.Find("Content/ItemName").GetComponent<Text>().text = shopItem.itemName;
                instance.transform.Find("Content/ItemDescription").GetComponent<Text>().text = shopItem.itemDescription;
                instance.transform.Find("Content/Purchase").GetComponent<Button>().onClick.AddListener(() =>
                {
                    shopItem.OnPurchased();
                });
                instance.transform.Find("Content/Purchase/Text").GetComponent<Text>().text = shopItem.purchaseType;
                instance.SetActive(true);

                shopItem.OnInitialized?.Invoke(instance);
            }

            continueMapTemplate.SetActive(false);

            for (int i = 0; i < AchievementManager.Data.userMapStorageList.Count; i++)
            {
                var mapStorage = AchievementManager.Data.userMapStorageList[i];
                var mapItem = defaultMapItems[mapStorage.MapItemID];

                var mapCreateDate = Util.GetDateTimeFromTimeSpan(mapStorage.MapName);

                var instance = Instantiate(continueMapTemplate, continueMapTemplate.transform.parent, true);
                instance.transform.Find("MapImg").GetComponent<Image>().sprite = mapItem.mapIcon;
                instance.transform.Find("Detail/MapName").GetComponent<Text>().text = $"{mapCreateDate.Month}月{mapCreateDate.Day}日{mapCreateDate.Hour}时{mapCreateDate.Minute}分";
                instance.transform.Find("Detail/Size").GetComponent<Text>().text = mapItem.mapSize;
                instance.transform.Find("DeleteBtn").GetComponent<Button>().onClick.AddListener(() =>
                {
                    var earnLand = Mathf.FloorToInt(mapItem.landCost * 0.5f);

                    PushPopup($"删除地图:{mapStorage.MapName} {mapItem.mapSize}? 将得到 {earnLand} 空地，但此地图存档将消失!", (state) =>
                    {
                        if (state)
                        {
                            AchievementManager.Data.userMapStorageList.Remove(mapStorage);
                            AchievementManager.Data.Land += earnLand;
                            UpdateResources();

                            Destroy(instance);
                        }
                    });
                });
                instance.transform.Find("ContinueBtn").GetComponent<Button>().onClick.AddListener(() =>
                {
                    activeMapStorage = mapStorage;
                    loadingBar.gameObject.SetActive(true);
                    SceneManager.LoadScene("AR");
                });

                instance.SetActive(true);
            }

            createMapTemplate.SetActive(false);

            for (int i = 0; i < defaultMapItems.Count; i++)
            {
                var mapItem = defaultMapItems[i];
                var id = i;

                var instance = Instantiate(createMapTemplate, createMapTemplate.transform.parent, true);
                instance.transform.Find("MapImg").GetComponent<Image>().sprite = mapItem.mapIcon;
                instance.transform.Find("LandRequired/Cost").GetComponent<Text>().text = mapItem.landCost.ToString();
                instance.transform.Find("Detail/MapName").GetComponent<Text>().text = mapItem.mapName;
                instance.transform.Find("Detail/Size").GetComponent<Text>().text = mapItem.mapSize;

                instance.transform.Find("CreateBtn").GetComponent<Button>().onClick.AddListener(() =>
                {
                    CreateNewMapStorage(mapItem, id);
                });

                instance.transform.Find("LandRequired").GetComponent<Button>().onClick.AddListener(() =>
                {
                    CreateNewMapStorage(mapItem, id);
                });

                instance.SetActive(true);
            }

            UpdateResources();

            TutorialBtn.onClick.AddListener(() =>
            {
                PushPopup("将跳转到游戏教程网站，是否继续?", res =>
                {
                    if (res)
                    {
                        Application.OpenURL("http://doreamonsky.coding.me/CraftingAR-Doc/#/");
                    }
                });
            });

            AchievementBtn.gameObject.SetActive(false);

            AchievementBtn.onClick.AddListener(() =>
            {
                PushPopup("成就系统正在制作ing...快了...", r => { }, false);
            });

            ExitGameBtn.onClick.AddListener(() =>
            {
                QuitGame();
            });

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                ExitGameBtn.gameObject.SetActive(false);
            }

            versionText.text = $"版本: v{Application.version}@Unity {Application.unityVersion}";

            StartCoroutine(ARSession.CheckAvailability());

            ARSession.stateChanged += state =>
            {
                switch (state.state)
                {
                    case ARSessionState.None:
                        //PushPopup("AR Core 未知错误", r => { }, false);
                        break;
                    case ARSessionState.Unsupported:
                        PushPopup("你的设备不支持 AR Core，无法正常使用本游戏", r =>
                        {
                            Application.Quit();
                        }, false);
                        break;
                    case ARSessionState.CheckingAvailability:
                        break;
                    case ARSessionState.NeedsInstall:
                        PushPopup("请下载安装 AR Core", r => { }, false);
                        break;
                    case ARSessionState.Installing:
                        PushPopup("AR Core 正在安装中...", r => { }, false);
                        break;
                    case ARSessionState.Ready:
                        PushPopup("AR Core 启动完毕", r => { }, false);
                        break;
                    case ARSessionState.SessionInitializing:
                        break;
                    case ARSessionState.SessionTracking:
                        break;
                }
            };
        }

        private void CreateNewMapStorage(MapItem mapItem, int id)
        {
            if (AchievementManager.Data.Land < mapItem.landCost)
            {
                PushPopup($"无法创建地图:{mapItem.mapName} 需要空地数量:{mapItem.landCost}。可前往左下角'商店'观看广告获取。", res =>
                {
                    shopBar.SetActive(true);
                });
            }
            else
            {
                PushPopup($"是否创建地图:{mapItem.mapName} 消耗空地数量:{mapItem.landCost}", res =>
                {
                    if (res)
                    {
                        var mapName = $"{Util.GetTimeStamp()}";

                        var userMapStorage = new UserMapStorage()
                        {
                            MapItemID = id,
                            MapName = mapName,
                        };

                        activeMapStorage = userMapStorage;

                        AchievementManager.Data.Land -= mapItem.landCost;

                        AchievementManager.Data.userMapStorageList.Add(userMapStorage);
                        AchievementManager.Data.OnValueChanged?.Invoke();

                        UpdateResources();

                        var instanceMapData = Instantiate(mapItem.mapData);
                        instanceMapData.mapName = mapName;
                        instanceMapData.OnSave();


                        Debug.Log($"Map Saved: {mapName}");

                        loadingBar.gameObject.SetActive(true);
                        SceneManager.LoadScene("AR");
                    }
                });
            }
        }

        public void UpdateResources()
        {
            coinText.text = AchievementManager.Data.Coin.ToString();
            landText.text = AchievementManager.Data.Land.ToString();
        }

        public void PushPopup(string message, System.Action<bool> onCompleted, bool hasCancel = true)
        {
            popConfirm.onClick.RemoveAllListeners();
            popCancel.onClick.RemoveAllListeners();

            popCancel.gameObject.SetActive(hasCancel);

            popConfirm.onClick.AddListener(() =>
            {
                popBar.SetActive(false);
                onCompleted?.Invoke(true);
            });

            popCancel.onClick.AddListener(() =>
            {
                popBar.SetActive(false);
                onCompleted?.Invoke(false);
            });

            popText.text = message;
            popBar.SetActive(true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                QuitGame();
            }
        }

        private void QuitGame()
        {
            PushPopup("是否退出游戏?", state =>
            {
                if (state)
                {
                    Application.Quit();
                }
            });
        }

        private void OnDestroy()
        {
            Advertisement.RemoveListener(unityADCallBack);
        }
    }

}