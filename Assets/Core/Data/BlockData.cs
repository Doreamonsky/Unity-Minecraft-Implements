using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName = "Block", menuName = "BlockData")]
    public abstract class BlockData : ScriptableObject
    {
        public bool forceRenderer = false;

        public Material topTex, bottomTex, rightTex, leftTex, frontTex, backTex;

        public abstract void RemoveBlock(WorldManager worldManager, int height, int x, int y);
    }

}
