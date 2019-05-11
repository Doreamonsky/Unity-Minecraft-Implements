using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName = "MapData", menuName = "MapData")]
    public class MapData : ScriptableObject
    {
        public string mapName = "Default";
        //Height Width Length
        //Origin Point (0,0,0)
        public int[,,] worldData;

        public int max_width = 64, max_length = 64, max_height = 32;
    }

}
