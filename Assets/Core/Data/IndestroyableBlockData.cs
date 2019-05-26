using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName = "DestroyableBlockData", menuName = "DestroyableBlockData")]
    public class IndestroyableBlockData : BlockData
    {
        public override void RemoveBlock(WorldManager worldManager, int height, int x, int y)
        {
        }
    }
}