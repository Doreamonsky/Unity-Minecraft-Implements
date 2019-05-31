using MC.Core;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MC.CoreEditor
{
    [CustomEditor(typeof(CraftSystem))]
    public class CraftSystemEditor : Editor
    {
        private CraftSystem craftSystem;

        private void OnEnable()
        {
            craftSystem = target as CraftSystem;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            if(GUILayout.Button("Update Recipe"))
            {
                craftSystem.recipeList = new List<RecipeData>();

                foreach (var guid in AssetDatabase.FindAssets("t:RecipeData"))
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);

                    craftSystem.recipeList.Add(AssetDatabase.LoadAssetAtPath<RecipeData>(path));
                }
            }
   
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }

}
