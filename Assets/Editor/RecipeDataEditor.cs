using MC.Core;
using UnityEditor;
using UnityEngine;

namespace MC.CoreEditor
{
    [CustomEditor(typeof(RecipeData))]
    public class RecipeDataEditor : Editor
    {
        private RecipeData recipeData;

        private void OnEnable()
        {
            recipeData = target as RecipeData;
        }
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            EditorGUILayout.LabelField("Recipe:");

            recipeData.requireHeating = EditorGUILayout.Toggle("Heating", recipeData.requireHeating);

            for (int i = 0; i < 3; i++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < 3; j++)
                {
                    recipeData.Recipe[i * 3 + j] = (Inventory)EditorGUILayout.ObjectField(recipeData.Recipe[i * 3 + j], typeof(Inventory), allowSceneObjects: false);
                }
                EditorGUILayout.EndHorizontal();
            }

            recipeData.CraftedInventory = (Inventory)EditorGUILayout.ObjectField("Crafted Inventory:", recipeData.CraftedInventory, typeof(Inventory), allowSceneObjects: false);

            recipeData.CraftedCount = EditorGUILayout.IntField("Count:", recipeData.CraftedCount);

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }

}
