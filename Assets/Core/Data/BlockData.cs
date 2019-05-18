using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName = "Block", menuName = "BlockData")]
    public abstract class BlockData : ScriptableObject
    {
        public Material topTex, bottomTex, rightTex, leftTex, frontTex, backTex;

        public abstract void RemoveBlock(int height, int x, int y);
    }

}
