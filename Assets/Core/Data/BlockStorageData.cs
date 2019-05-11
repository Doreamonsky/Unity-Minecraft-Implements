using System.Collections.Generic;
using UnityEngine;

namespace MC.Core
{
    [System.Serializable]
    public class BlockStorageMapping
    {
        public int layerID = 0;
        public BlockData blockData;
    }

    public class BlockStorageData : ScriptableObject
    {
        public List<BlockStorageMapping> BlockMapping = new List<BlockStorageMapping>();
    }
}
