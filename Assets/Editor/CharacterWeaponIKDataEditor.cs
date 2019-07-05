using MC.Core;
using UnityEditor;
using UnityEngine;

namespace MC.CoreEditor
{
    [CustomEditor(typeof(CharacterWeaponIKData))]
    public class CharacterWeaponIKDataEditor : Editor
    {
        private GameObject activeWeapon;

        private void OnEnable()
        {
            var characterWeaponIKData = target as CharacterWeaponIKData;
        }

        public override void OnInspectorGUI()
        {
            activeWeapon = (GameObject)EditorGUILayout.ObjectField("Active Weapon", activeWeapon, typeof(GameObject));


            if (GUILayout.Button("Get Weapon Info"))
            {
                if (PrefabUtility.GetCorrespondingObjectFromOriginalSource(activeWeapon) == null)
                {
                    EditorUtility.DisplayDialog("Failed", "Active Target is not prefab", "OK");

                    return;
                }

                var leftHand = activeWeapon.transform.Find("Left Hand");
                var rightHand = activeWeapon.transform.Find("Right Hand");

                var slotData = new CharacterWeaponIKData.SlotData()
                {
                    leftHandPos = leftHand.localPosition,
                    leftHandRot = leftHand.localRotation,
                    rightHandPos = rightHand.localPosition,
                    rightHandRot = rightHand.localRotation,
                    weapon = PrefabUtility.GetCorrespondingObjectFromOriginalSource(activeWeapon),
                    weaponPos = activeWeapon.transform.localPosition,
                    weaponRot = activeWeapon.transform.localRotation
                };

                var characterWeaponIKData = target as CharacterWeaponIKData;

                var prevData = characterWeaponIKData.slotDataList.Find(val =>
                    val.weapon == slotData.weapon
                );

                if (prevData != null)
                {
                    characterWeaponIKData.slotDataList.Remove(prevData);
                }

                characterWeaponIKData.slotDataList.Add(slotData);
            }

            base.OnInspectorGUI();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
