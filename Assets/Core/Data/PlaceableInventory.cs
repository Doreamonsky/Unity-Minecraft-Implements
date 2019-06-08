using MC.Core.Interface;
using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName = "PlaceableInventory")]
    public class PlaceableInventory : Inventory, IPlaceable, IDigable
    {
        public enum PlaceType
        {
            Block = 0,
            Item = 1
        }

        public PlaceType placeType = PlaceType.Block;

        public BlockData blockData;

        public GameObject itemModel;

        public Vector3 itemPos = new Vector3(0.5f, 0, 0.5f);

        [Header("IDigable")]
        public float digTime = 1;

        public AudioClip digSound;

        public Inventory dropInventory;

        public bool Place(WorldManager worldManager, Vector3 pos)
        {
            switch (placeType)
            {
                case PlaceType.Block:
                    //从当前 WorldManager LayerID 
                    var layerID = worldManager.blockStorageData.BlockMapping.Find(val => val.blockData?.name == blockData.name).layerID;
                    worldManager.CreateBlock((int)pos.y, (int)pos.x, (int)pos.z, layerID);

                    return true;
                case PlaceType.Item:
                    if (worldManager.GetItemData(pos + itemPos) == null)
                    {
                        worldManager.CreatePlaceableInventory(this, pos + itemPos, Vector3.zero);
                        return true;
                    }
                    break;
            }
            return false;
        }

        public override void OnSelected(InventorySystem inventorySystem)
        {
            //throw new System.NotImplementedException();
        }

        public override void OnUnselected(InventorySystem inventorySystem)
        {
            //throw new System.NotImplementedException();
        }

        public float DigTime()
        {
            return digTime;
        }

        public AudioClip DigSound()
        {
            return digSound;
        }

        public void DropInventory(Vector3 pos)
        {
            if (dropInventory != null)
            {
                InventoryDropManager.Instance.CreateDropBlockInventory(pos + new Vector3(0, 0.5f, 0), new InventoryStorage()
                {
                    count = 1,
                    inventory = dropInventory,
                    slotID = -1
                }
            );
            }
        }
    }
}
