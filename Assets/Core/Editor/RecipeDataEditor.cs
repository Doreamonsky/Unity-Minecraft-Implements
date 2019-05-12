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

            for (int i = 0; i < 3; i++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < 3; j++)
                {
                    recipeData.Recipe[i * 3 + j] = (Inventory)EditorGUILayout.ObjectField(recipeData.Recipe[i * 3 + j], typeof(Inventory));
                }
                EditorGUILayout.EndHorizontal();
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }

}
