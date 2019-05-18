using MC.Core.Interface;
using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName = "PlaceableInventory")]
    public class PlaceableInventory : Inventory, IPlaceable
    {
        public enum PlaceType
        {
            Block = 0
        }

        public PlaceType placeType = PlaceType.Block;

        public BlockData blockData;


        public void Place(Vector3 pos)
        {
            //从当前 WorldManager LayerID 
            var layerID = WorldManager.Instance.blockStorageData.BlockMapping.Find(val => val.blockData?.name == blockData.name).layerID;

            WorldManager.Instance.CreateBlock((int)pos.y, (int)pos.x, (int)pos.z, layerID);
        }
    }
}
