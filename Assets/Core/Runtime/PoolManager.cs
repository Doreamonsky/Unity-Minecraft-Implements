using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MC.Core
{
    public class PoolManager : MonoBehaviour
    {
        private Dictionary<string, int> ParamsList = new Dictionary<string, int>();
        private List<string> LoadedObjectName = new List<string>();

        public static PoolManager Instance;
        [System.Serializable]
        public class Params
        {
            public string ObjectName;
            public GameObject Object;
            public int CreateCount = 1;
            public float LifeTime = 5;
            public Dictionary<GameObject, bool> ObjectList = new Dictionary<GameObject, bool>();
            public bool isBuffer = false;
            public void Init()
            {
                for (int i = 0; i < CreateCount; i++)
                {
                    GameObject templateInstance = Instantiate(Object);

                    templateInstance.name = ObjectName;
                    ObjectList.Add(templateInstance, false);
                    templateInstance.transform.SetParent(Instance.transform);
                    if (isBuffer)
                    {
                        templateInstance.SetActive(true);
                        Instance.StartCoroutine(Sync(templateInstance));
                    }
                    else
                    {
                        templateInstance.SetActive(false);
                    }
                }
            }
            IEnumerator Sync(GameObject poolObject)
            {
                yield return new WaitForSeconds(5);
                poolObject.SetActive(false);
            }
        }
        public Params[] Objects;

        void Awake()
        {
            InitParams();

            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, mode) =>
            {
                LoadedObjectName = new List<string>();
            };
        }
        protected void InitParams()
        {
            Instance = this;
            for (int i = 0; i < Objects.Length; i++)
            {
                Objects[i].Init();
                LoadedObjectName.Add(Objects[i].ObjectName);
                ParamsList.Add(Objects[i].ObjectName, i);
            }
        }
        public static GameObject CreateObject(string GET_Name, Vector3 GET_Position, Vector3 GET_Rotation)
        {
            if (Instance == null)
            {
                return null;
            }

            if (Instance.ParamsList.ContainsKey(GET_Name) == false)
            {
                Debug.Log("Missing Resourece");
                return null;
            }

            int Index = Instance.ParamsList[GET_Name];
            Params MyParams = Instance.Objects[Index];
            float LifeTime = MyParams.LifeTime;
            foreach (GameObject Key in MyParams.ObjectList.Keys)
            {
                if (Key == null)
                {
                    continue;
                }

                if (MyParams.ObjectList[Key] == false)
                {
                    Key.transform.position = GET_Position;
                    Key.transform.eulerAngles = GET_Rotation;
                    Key.SetActive(true);
                    Instance.StartCoroutine(Instance.DisActiveObject(MyParams.ObjectList, Key, LifeTime, Key.name));
                    return Key;
                }
            }
            return null;

        }
        protected IEnumerator DisActiveObject(Dictionary<GameObject, bool> TheDictionary, GameObject Key, float WaitTime, string objectName)
        {

            TheDictionary[Key] = true;
            yield return new WaitForSeconds(WaitTime);
            TheDictionary[Key] = false;
            Key.SetActive(false);
        }
        public static void UpdateParams(string ObjectName, GameObject oj, int CreateCount, float LifeTime, bool isBuffer)
        {
            if (LoadedObject(ObjectName) == false)
            {
                Params newParams = new Params();
                newParams.ObjectName = ObjectName;
                newParams.Object = oj;
                newParams.LifeTime = LifeTime;
                newParams.CreateCount = CreateCount;
                newParams.isBuffer = isBuffer;
                newParams.Init();
                Instance.Objects = ParamsAdd(Instance.Objects, newParams);
                Instance.ParamsList.Add(ObjectName, Instance.Objects.Length - 1);
                Instance.LoadedObjectName.Add(ObjectName);
            }
            else
            {
                Debug.Log("Conflict");
            }
        }
        public static bool LoadedObject(string ObjectName)
        {
            if (Instance.LoadedObjectName.Contains(ObjectName) || Instance.ParamsList.ContainsKey(ObjectName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected static Params[] ParamsAdd(Params[] Target, Params AddedElement)
        {
            Params[] New = new Params[Target.Length + 1];
            for (int i = 0; i < Target.Length; i++)
            {
                New[i] = Target[i];
            }
            New[New.Length - 1] = AddedElement;
            return New;
        }

        //public static void Initialize()
        //{
        //    var assetRequest = new AssetRequestTask()
        //    {
        //        onAssetLoaded = val =>
        //        {
        //            DontDestroyOnLoad(Instantiate(val as GameObject));
        //        }
        //    };

        //    assetRequest.SetAssetBundleName("PoolManager", "Utility");

        //    AssetBundleManager.LoadAssetFromAssetBundle(assetRequest);
        //}
    }
}