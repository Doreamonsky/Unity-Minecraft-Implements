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

        public void OnLoad()
        {
            if (isSaveable)
            {
                GeneralStorageSystem.LoadFile(this, $"{mapName}_mapData");
            }
        }

        public void OnSave()
        {
            if (isSaveable)
            {
                GeneralStorageSystem.SaveFile(this, $"{mapName}_mapData");
            }
        }

        public bool HasSaving()
        {
            if (isSaveable)
            {
                return GeneralStorageSystem.HasFile($"{mapName}_mapData");
            }
            else
            {
                return false;
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
