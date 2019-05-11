﻿using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName = "Block", menuName = "BlockData")]
    public class BlockData : ScriptableObject
    {
        public Texture topTex, bottomTex, rightTex, leftTex, frontTex, backTex;
    }

}
