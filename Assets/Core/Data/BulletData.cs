using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName = "BulletData",menuName = "BulletData")]
    public class BulletData : ScriptableObject
    {
        public float Velocity = 540;

        public float Damage = 10;

        public string hitEffect;

        public string weaponProjectile;
    }

}
