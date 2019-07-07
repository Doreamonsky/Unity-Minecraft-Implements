using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "WeaponData")]
    public class WeaponData : ScriptableObject
    {
        public GameObject fpsWeaponPrefab;

        public GameObject tpsWeaponPrefab;

        public float fireRate = 0.1f;

        public Mag[] defaultMags = new Mag[3];

        public SoundClips soundClips;

        public BulletData bulletData;
    }

}
