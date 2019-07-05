using System.Collections.Generic;
using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName = "CharacterWeaponIKData", menuName = "CharacterWeaponIKData")]
    public class CharacterWeaponIKData : ScriptableObject
    {
        /// <summary>
        /// Slot 信息 均为 Local Transform 下的
        /// </summary>
        [System.Serializable]
        public class SlotData
        {
            public GameObject weapon;

            public Vector3 weaponPos;
            public Quaternion weaponRot;

            public Vector3 leftHandPos;
            public Quaternion leftHandRot;

            public Vector3 rightHandPos;
            public Quaternion rightHandRot;
        }

        public List<SlotData> slotDataList = new List<SlotData>();
    }

}
