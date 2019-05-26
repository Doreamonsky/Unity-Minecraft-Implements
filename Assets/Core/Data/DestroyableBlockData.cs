using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName = "DestroyableBlockData", menuName = "DestroyableBlockData")]
    public class DestroyableBlockData : BlockData
    {
        public float digTime = 1;

        public AudioClip digSound;

        public Inventory dropInventory;

        public override void RemoveBlock(WorldManager worldManager, int height, int x, int y)
        {
            worldManager.RemoveBlock(height, x, y);

            if (dropInventory != null)
            {
                InventoryDropManager.Instance.CreateDropBlockInventory(new Vector3(x + worldManager.mapData.startPos.x, height, y + worldManager.mapData.startPos.z) + new Vector3(1, 1, 1) * 0.5f, new InventoryStorage()
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