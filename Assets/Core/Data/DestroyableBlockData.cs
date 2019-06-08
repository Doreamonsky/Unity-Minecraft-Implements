using MC.Core.Interface;
using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName = "DestroyableBlockData", menuName = "DestroyableBlockData")]
    public class DestroyableBlockData : BlockData, IDigable
    {
        public float digTime = 1;

        public AudioClip digSound;

        public Inventory dropInventory;

        public AudioClip DigSound()
        {
            return digSound;
        }

        public float DigTime()
        {
            return digTime;
        }

        public void DropInventory(Vector3 pos)
        {
            if (dropInventory != null)
            {
                InventoryDropManager.Instance.CreateDropBlockInventory(pos + new Vector3(1, 1, 1) * 0.5f, new InventoryStorage()
                {
                    count = 1,
                    inventory = dropInventory,
                    slotID = -1
                }
            );
            }
        }

        public override void RemoveBlock(WorldManager worldManager, int height, int x, int y)
        {
            worldManager.RemoveBlock(height, x, y);
        }
    }
}