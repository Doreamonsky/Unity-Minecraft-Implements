using MC.Core;
using UnityEditor;
using UnityEngine;

namespace MC.CoreEditor
{
    [CustomEditor(typeof(WorldManager))]
    public class WorldManagerEditor : Editor
    {
        private WorldManager worldManager;

        private void OnEnable()
        {
            worldManager = target as WorldManager;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Preview World"))
            {
                worldManager.Start();
            }
        }
    }

}
