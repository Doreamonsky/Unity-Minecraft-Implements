using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MC.Core;

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

            mapData.worldData = new int[mapData.max_height, mapData.max_width, mapData.max_length];

            for (var heightIndex = 0; heightIndex < mapData.max_height; heightIndex++)
            {
                for (var i = 0; i < mapData.max_width; i++)
                {
                    for (var j = 0; j < mapData.max_length; j++)
                    {
                        if (heightIndex == 1)
                        {
                            mapData.worldData[heightIndex, i, j] = 1;
                        }
                        else if (heightIndex == 14 && i == 8 && j == 8)
                        {
                            mapData.worldData[heightIndex, i, j] = 1;
                        }
                        else if (i == 5 && heightIndex == 2)
                        {
                            mapData.worldData[heightIndex, i, j] = 1;
                        }
                        else
                        {
                            mapData.worldData[heightIndex, i, j] = 0;
                        }

                    }
                }
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }

}
