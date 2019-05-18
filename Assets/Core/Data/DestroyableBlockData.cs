using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName = "DestroyableBlockData", menuName = "DestroyableBlockData")]
    public class DestroyableBlockData : BlockData
    {
        public float digTime = 1;

        public override void RemoveBlock(int height, int x, int y)
        {
            WorldManager.Instance.RemoveBlock(height, x, y);
        }
    }
}