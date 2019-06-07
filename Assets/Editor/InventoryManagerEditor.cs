using MC.Core;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MC.CoreEditor
{
    [CustomEditor(typeof(InventoryManager))]
    public class InventoryManagerEditor : Editor
    {
        private InventoryManager inventoryManager;

        private void OnEnable()
        {
            inventoryManager = target as InventoryManager;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            if (GUILayout.Button("Update Inventories"))
            {
                inventoryManager.inventories = new List<Inventory>();

                foreach (var guid in AssetDatabase.FindAssets("t:Inventory"))
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);

                    inventoryManager.inventories.Add(AssetDatabase.LoadAssetAtPath<Inventory>(path));
                }
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }

}
