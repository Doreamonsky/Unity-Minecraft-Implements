using System.IO;
using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName = "MapData", menuName = "MapData")]
    public class MapData : ScriptableObject
    {
        public MapData()
        {
            UpdateMapSize();
        }

        public void UpdateMapSize()
        {
            worldDataSeralized = new int[max_width * max_length * max_height];
        }
        public string mapName = "Default";
        //Height Width Length
        //Origin Point (0,0,0)
        public bool isSaveable = false;

        public int max_width = 64, max_length = 64, max_height = 32;

        public int[] worldDataSeralized;

        public int[,,] WorldData { get => arrayToMatrix(worldDataSeralized); set => worldDataSeralized = matrixToArray(value); }

        public int seed;

        public Vector3 startPos = Vector3.zero;

        private int[] matrixToArray(int[,,] m)
        {
            int[] array = new int[max_width * max_length * max_height];

            var n = 0;

            for (var i = 0; i < max_height; i++)
            {
                for (var j = 0; j < max_width; j++)
                {
                    for (var k = 0; k < max_length; k++)
                    {
                        array[n] = m[i, j, k];
                        n++;
                    }
                }
            }

            return array;
        }

        private int[,,] arrayToMatrix(int[] array)
        {
            int[,,] m = new int[max_height, max_width, max_length];

            var n = 0;

            for (var i = 0; i < max_height; i++)
            {
                for (var j = 0; j < max_width; j++)
                {
                    for (var k = 0; k < max_length; k++)
                    {
                        m[i, j, k] = array[n];
                        n++;
                    }
                }
            }

            return m;
        }

        private string Storage {
            get {
                if (Application.platform == RuntimePlatform.Android)
                {
                    return $"{Application.persistentDataPath}/saves/{mapName}_map.json";
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    return $"{Application.persistentDataPath}/saves/{mapName}_map.json";
                }

                return $"{Application.dataPath}/../saves/{mapName}_map.json";
            }
        }

        public void OnLoad()
        {
            if (isSaveable)
            {
                var saveFile = new FileInfo(Storage);

                var dic = new DirectoryInfo(saveFile.DirectoryName);

                if (!dic.Exists)
                {
                    dic.Create();
                }

                if (saveFile.Exists)
                {
                    var fileStream = new FileStream(Storage, FileMode.Open);
                    var steamReader = new StreamReader(fileStream);

                    var json = steamReader.ReadToEnd();
                    JsonUtility.FromJsonOverwrite(json, this);

                    steamReader.Close();
                    fileStream.Close();
                }
            }
        }

        public void OnSave()
        {
            if (isSaveable)
            {
                var json = JsonUtility.ToJson(this);

                var fileStream = new FileStream(Storage, FileMode.Create);
                var streamWriter = new StreamWriter(fileStream);

                streamWriter.Write(json);

                streamWriter.Flush();
                fileStream.Flush();

                streamWriter.Close();
                fileStream.Close();
            }
        }
    }

    [System.Serializable]
    public class DataWrapper
    {
        [SerializeField]
        public int[,,] data;

        public DataWrapper(int[,,] val)
        {
            data = val;
        }
    }
}
