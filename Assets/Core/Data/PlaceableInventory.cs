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

        [Header("IDigable")]
        public float digTime = 1;

        public AudioClip digSound;

        public Inventory dropInventory;

        /// <summary>
        /// 放置物体
        /// </summary>
        /// <param name="worldManager"></param>
        /// <param name="pos"></param>
        /// <param name="dir">放置时候角度</param>
        /// <returns></returns>
        public bool Place(WorldManager worldManager, Vector3Int pos, Vector3 dir)
        {
            switch (placeType)
            {
                case PlaceType.Block:
                    //从当前 WorldManager LayerID 
                    var layerID = worldManager.blockStorageData.BlockMapping.Find(val => val.blockData?.name == blockData.name).layerID;
                    worldManager.CreateBlock(pos.y, pos.x, pos.z, layerID);

                    return true;
                case PlaceType.Item:
                    if (worldManager.GetItemData(pos) == null)
                    {
                        // 判断朝向WorldManager的哪个轴
                        var xProjectPos = Vector3.Dot(worldManager.transform.right, dir);
                        var zProjectPos = Vector3.Dot(worldManager.transform.forward, dir);
                        var xProjectNeg = Vector3.Dot(-worldManager.transform.right, dir);
                        var zProjectNeg = Vector3.Dot(-worldManager.transform.forward, dir);

                        var args = new float[] { xProjectPos, zProjectPos, xProjectNeg, zProjectNeg };

                        var placeDir = Vector3.forward;

                        if (Mathf.Abs(xProjectPos - Mathf.Max(args)) < Mathf.Epsilon)
                        {
                            placeDir = worldManager.transform.right;
                        }
                        else if (Mathf.Abs(zProjectPos - Mathf.Max(args)) < Mathf.Epsilon)
                        {
                            placeDir = worldManager.transform.forward;
                        }
                        else if (Mathf.Abs(xProjectNeg - Mathf.Max(args)) < Mathf.Epsilon)
                        {
                            placeDir = -worldManager.transform.right;
                        }
                        else if (Mathf.Abs(zProjectNeg - Mathf.Max(args)) < Mathf.Epsilon)
                        {
                            placeDir = -worldManager.transform.forward;
                        }
                        else
                        {
                            Debug.Log("Fail To Lock Dir");
                            return false;
                        }

                        placeDir = worldManager.transform.InverseTransformDirection(placeDir);

                        worldManager.CreatePlaceableInventory(this, pos, placeDir);
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
