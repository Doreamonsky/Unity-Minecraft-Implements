using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName = "DestroyableBlockData", menuName = "DestroyableBlockData")]
    public class DestroyableBlockData : BlockData
    {
        public float digTime = 1;

        public AudioClip digSound;

        public Inventory dropInventory;

        public override void RemoveBlock(int height, int x, int y)
        {
            WorldManager.Instance.RemoveBlock(height, x, y);

            if (dropInventory != null)
            {
                InventoryDropManager.Instance.CreateDropBlockInventory(new Vector3(x, height, y) + new Vector3(1, 1, 1) * 0.5f, new InventoryStorage()
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