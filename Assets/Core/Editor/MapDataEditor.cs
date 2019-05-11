using MC.Core;
using UnityEditor;
using UnityEngine;

namespace MC.CoreEditor
{
    [CustomEditor(typeof(MapData))]
    public class MapDataEditor : Editor
    {
        private MapData mapData;


        private void OnEnable()
        {
            mapData = target as MapData;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Generate World By Rule One"))
            {
                var data = new int[mapData.max_height, mapData.max_width, mapData.max_length];

                for (var heightIndex = 0; heightIndex < mapData.max_height; heightIndex++)
                {
                    for (var i = 0; i < mapData.max_width; i++)
                    {
                        for (var j = 0; j < mapData.max_length; j++)
                        {
                            if (heightIndex == 1)
                            {
                                data[heightIndex, i, j] = 1;
                            }
                            else if (heightIndex == 14 && i == 8 && j == 8)
                            {
                                data[heightIndex, i, j] = 1;
                            }
                            else if (i == 5 && heightIndex == 2)
                            {
                                data[heightIndex, i, j] = 1;
                            }
                            else
                            {
                                data[heightIndex, i, j] = 0;
                            }

                        }
                    }
                }

                mapData.WorldData = data;
            }


            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }

}
