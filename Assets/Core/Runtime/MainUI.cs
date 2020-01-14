using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Localization;
using System.Collections;

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


        public CommandLineLocaleSelector localeSelector;

        public static MainUI instance;

        private readonly UnityADCallBack unityADCallBack = new UnityADCallBack();

        private LocalizedStringReference stringReference;

        private static StringTableBase table;

        private void Awake()
        {
            instance = this;
        }


        private IEnumerator Start()
        {
            DontDestroyOnLoad(Instantiate(runtimeSupport));

            if (Application.platform == RuntimePlatform.Android)
            {
                Advertisement.Initialize("3428103", false);
            }
            else
            {
                Advertisement.Initialize("3428102", false);
            }

            Advertisement.AddListener(unityADCallBack);

            yield return LocalizationSettings.InitializationOperation;


            stringReference = new LocalizedStringReference()
            {
                TableName = "MainTable",
            };

            //if (PlayerPrefs.HasKey("Lang"))
            //{
            //    LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[PlayerPrefs.GetInt("Lang")];
            //}

            //languageDrop.value = LocalizationSettings.AvailableLocales.Locales.FindIndex(x => x == LocalizationSettings.SelectedLocale);
            //languageDrop.RefreshShownValue();

            //languageDrop.onValueChanged.AddListener(val =>
            //{
            //    LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[val];
            //    PlayerPrefs.SetInt("Lang", val);
            //    SceneManager.LoadScene("Main");
            //});


            var task = stringReference.GetLocalizedStringTable();

            task.Completed += (s) =>
            {
                table = s.Result;

                foreach (var mapItem in defaultMapItems)
                {
                    mapItem.mapName = table.GetLocalizedString(mapItem.mapName);
                    mapItem.mapSize = table.GetLocalizedString(mapItem.mapSize);
                }

                shopTemplate.SetActive(false);

                // 0 
                shopItems.Add(new ShopItem()
                {
                    itemName = string.Format(table.GetLocalizedString("Get{0}Coin"), 2),
                    itemDescription = table.GetLocalizedString("CoinUsage"),
                    purchaseType = table.GetLocalizedString("ByAD"),
                    OnPurchased = () =>
                    {
                        var state = Advertisement.GetPlacementState("GetCoin");

                        if (state == PlacementState.Ready)
                        {
                            Advertisement.Show("GetCoin");
                        }
                        else
                        {
                            PushPopup(table.GetLocalizedString("ADNotReady"), r => { }, false);
                        }
                    }
                });

                // 1
                shopItems.Add(new ShopItem()
                {
                    itemName = string.Format(table.GetLocalizedString("Get{0}Land"), 1),
                    itemDescription = table.GetLocalizedString("LandUsage"),
                    purchaseType = table.GetLocalizedString("ByAD"),
                    OnPurchased = () =>
                    {
                        var state = Advertisement.GetPlacementState("GetLand");

                        if (state == PlacementState.Ready)
                        {
                            Advertisement.Show("GetLand");
                        }
                        else
                        {
                            PushPopup(table.GetLocalizedString("ADNotReady"), r => { }, false);
                        }
                    }
                });

                // 2
                shopItems.Add(new ShopItem()
                {
                    itemName = string.Format(table.GetLocalizedString("Get{0}Coin"), 4),
                    itemDescription = table.GetLocalizedString("CoinUsage"),
                    purchaseType = table.GetLocalizedString("ByFollower"),
                    OnPurchased = () =>
                    {
                        if (AchievementManager.Data.isBiliBiliFollower)
                        {
                            PushPopup(table.GetLocalizedString("FollowerDuplicate"), state => { }, false);
                        }
                        else
                        {
                            Application.OpenURL("https://space.bilibili.com/4738690");

                            AchievementManager.Data.isBiliBiliFollower = true;
                            AchievementManager.Data.Coin += 4;

                            instance.UpdateResources();
                            PushPopup(string.Format(table.GetLocalizedString("Get{0}Coin"), 4), state => { }, false);
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
                    itemName = string.Format(table.GetLocalizedString("Get{0}Land"), 1),
                    itemDescription = table.GetLocalizedString("LandUsage"),
                    purchaseType = string.Format(table.GetLocalizedString("By{0}Coin"), 2),
                    OnPurchased = () =>
                    {
                        if (AchievementManager.Data.Coin - 2 < 0)
                        {
                            PushPopup(table.GetLocalizedString("CoinInsufficient"), r => { }, false);
                            return;
                        }

                        PushPopup(string.Format(table.GetLocalizedString("By{0}Coin"), 2), state =>
                         {
                             if (state)
                             {
                                 AchievementManager.Data.Coin -= 2;
                                 AchievementManager.Data.Land += 1;
                                 PushPopup(string.Format(table.GetLocalizedString("Get{0}Land"), 1), r => { }, false);

                                 UpdateResources();
                             }
                         });
                    }
                });

                // 4
                shopItems.Add(new ShopItem()
                {
                    itemName = string.Format(table.GetLocalizedString("{0}Wood"), 60),
                    itemDescription = table.GetLocalizedString("ResourceUsage"),
                    purchaseType = string.Format(table.GetLocalizedString("By{0}Coin"), 1),
                    OnPurchased = () =>
                    {
                        if (AchievementManager.Data.Coin - 1 < 0)
                        {
                            PushPopup(table.GetLocalizedString("CoinInsufficient"), r => { }, false);
                            return;
                        }

                        PushPopup(string.Format(table.GetLocalizedString("By{0}Coin"), 1), state =>
                        {
                            if (state)
                            {
                                var isPurchased = AchievementManager.Data.AddInv("Oak Wood", 60);

                                if (isPurchased)
                                {
                                    AchievementManager.Data.Coin -= 1;
                                    PushPopup(string.Format(table.GetLocalizedString("{0}Wood"), 60), r => { }, false);
                                    UpdateResources();
                                }
                                else
                                {
                                    PushPopup(table.GetLocalizedString("BagSpaceInsufficient"), r => { }, false);
                                }
                            }
                        });
                    }
                });

                // 5
                shopItems.Add(new ShopItem()
                {
                    itemName = string.Format(table.GetLocalizedString("{0}Stone"), 40),
                    itemDescription = table.GetLocalizedString("ResourceUsage"),
                    purchaseType = string.Format(table.GetLocalizedString("By{0}Coin"), 1),
                    OnPurchased = () =>
                    {
                        if (AchievementManager.Data.Coin - 1 < 0)
                        {
                            PushPopup(table.GetLocalizedString("CoinInsufficient"), r => { }, false);
                            return;
                        }

                        PushPopup(string.Format(table.GetLocalizedString("By{0}Coin"), 1), state =>
                        {
                            if (state)
                            {
                                var isPurchased = AchievementManager.Data.AddInv("Stone", 40);

                                if (isPurchased)
                                {
                                    AchievementManager.Data.Coin -= 1;
                                    PushPopup(string.Format(table.GetLocalizedString("{0}Stone"), 40), r => { }, false);
                                    UpdateResources();
                                }
                                else
                                {
                                    PushPopup(table.GetLocalizedString("BagSpaceInsufficient"), r => { }, false);
                                }
                            }
                        });
                    }
                });

                // 6 
                shopItems.Add(new ShopItem()
                {
                    itemName = string.Format(table.GetLocalizedString("{0}HandGun"), 1),
                    itemDescription = table.GetLocalizedString("HandGunUsage"),
                    purchaseType = string.Format(table.GetLocalizedString("By{0}Coin"), 10),
                    OnPurchased = () =>
                    {
                        if (AchievementManager.Data.Coin - 10 < 0)
                        {
                            PushPopup(table.GetLocalizedString("CoinInsufficient"), r => { }, false);
                            return;
                        }

                        PushPopup(table.GetLocalizedString("CoinInsufficient"), state =>
                        {
                            if (state)
                            {
                                var isPurchased = AchievementManager.Data.AddInv("Hand Gun", 1);

                                if (isPurchased)
                                {
                                    AchievementManager.Data.Coin -= 10;
                                    PushPopup(string.Format(table.GetLocalizedString("{0}HandGun"), 1), r => { }, false);
                                    UpdateResources();
                                }
                                else
                                {
                                    PushPopup(table.GetLocalizedString("BagSpaceInsufficient"), r => { }, false);
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

                        PushPopup(string.Format(table.GetLocalizedString("DeleteMapConfirm{0}{1}{2}"), mapStorage.MapName, mapItem.mapSize, earnLand), (state) =>
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
                    PushPopup(table.GetLocalizedString("ConfirmTutorial"), res =>
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

                versionText.text = $"{table.GetLocalizedString("Version")}: v{Application.version}@Unity {Application.unityVersion}";

                StartCoroutine(ARSession.CheckAvailability());

                ARSession.stateChanged += state =>
                {
                    switch (state.state)
                    {
                        case ARSessionState.None:
                            //PushPopup("AR Core 未知错误", r => { }, false);
                            break;
                        case ARSessionState.Unsupported:
                            PushPopup(table.GetLocalizedString("ARUnsupported"), r =>
                            {
                                Application.Quit();
                            }, false);
                            break;
                        case ARSessionState.CheckingAvailability:
                            break;
                        case ARSessionState.NeedsInstall:
                            PushPopup(table.GetLocalizedString("ARNeedsInstall"), r => { }, false);
                            break;
                        case ARSessionState.Installing:
                            PushPopup(table.GetLocalizedString("ARInstalling"), r => { }, false);
                            break;
                        case ARSessionState.Ready:
                            //PushPopup("AR Core 启动完毕", r => { }, false);
                            break;
                        case ARSessionState.SessionInitializing:
                            break;
                        case ARSessionState.SessionTracking:
                            break;
                    }
                };

                PushPopup(table.GetLocalizedString("ARWarning"), r => { }, false);
            };
        }

        private void CreateNewMapStorage(MapItem mapItem, int id)
        {
            if (AchievementManager.Data.Land < mapItem.landCost)
            {
                PushPopup(string.Format(table.GetLocalizedString("FailedMap{0}Land{1}"), mapItem.mapName, mapItem.landCost), res =>
                  {
                      shopBar.SetActive(true);
                  });
            }
            else
            {
                PushPopup(string.Format(table.GetLocalizedString("ConfirmCreateMap{0}Land{1}"), mapItem.mapName, mapItem.landCost), res =>
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
            PushPopup(table.GetLocalizedString("ExitGameConfirm"), state =>
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