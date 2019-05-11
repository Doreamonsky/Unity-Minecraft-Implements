using Newtonsoft.Json;
using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName = "MapData", menuName = "MapData")]
    public class MapData : ScriptableObject
    {
        public string mapName = "Default";
        //Height Width Length
        //Origin Point (0,0,0)
        public int max_width = 64, max_length = 64, max_height = 16;

        public string worldDataSeralized;

        public int[,,] WorldData { get => JsonConvert.DeserializeObject<int[,,]>(worldDataSeralized); set => worldDataSeralized = JsonConvert.SerializeObject(value); }
    }

}
