using MC.Core.Interface;
using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName = "PlaceableInventory")]
    public class PlaceableInventory : Inventory, IPlaceable
    {
        public enum PlaceType
        {
            Block = 0,
            Item = 1
        }

        public PlaceType placeType = PlaceType.Block;

        public BlockData blockData;

        public GameObject itemModel;

        public void Place(WorldManager worldManager,Vector3 pos)
        {
            switch (placeType)
            {
                case PlaceType.Block:
                    //从当前 WorldManager LayerID 
                    var layerID = worldManager.blockStorageData.BlockMapping.Find(val => val.blockData?.name == blockData.name).layerID;
                    worldManager.CreateBlock((int)pos.y, (int)pos.x, (int)pos.z, layerID);
                    break;
                case PlaceType.Item:
                    worldManager.CreatePlaceableInventory(this, pos, Vector3.zero);
                    break;
            }

        }

        public override void OnSelected(InventorySystem inventorySystem)
        {
            //throw new System.NotImplementedException();
        }

        public override void OnUnselected(InventorySystem inventorySystem)
        {
            //throw new System.NotImplementedException();
        }
    }
}
