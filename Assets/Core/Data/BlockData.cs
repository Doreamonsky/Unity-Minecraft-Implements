using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName = "Block", menuName = "BlockData")]
    public abstract class BlockData : ScriptableObject
    {
        public Material topTex, bottomTex, rightTex, leftTex, frontTex, backTex;

        public float destroyTime = 5;

        public abstract void RemoveBlock(int height, int x, int y);
    }

}
