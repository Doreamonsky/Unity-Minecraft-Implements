using UnityEngine;

namespace MC.Core
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "CharacterData", menuName = "CharacterData")]
    public class CharacterData : ScriptableObject
    {
        public string characterName;

        public GameObject character;

        public CharacterWeaponIKData weaponIKData;
    }

}
