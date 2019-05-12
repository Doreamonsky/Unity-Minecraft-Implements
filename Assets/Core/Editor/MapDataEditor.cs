using MC.Core;
using UnityEditor;
using UnityEngine;

namespace MC.CoreEditor
{
    [CustomEditor(typeof(MapData))]
    public class MapDataEditor : Editor
    {
        private MapData mapData;

        private readonly int[,] Xiao = new int[,]
        {
           {0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0, },
           {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0, },
           {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0, },
           {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0, },
           {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0, },
           {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0, },
           {0,0,0,0,0,2,0,0,1,0,0,2,0,0,0,0, },
           {0,0,0,0,2,0,0,0,1,0,0,0,2,0,0,0, },
           {0,0,0,2,0,0,0,0,1,0,0,0,0,2,0,0, },
           {0,0,2,0,0,0,0,0,1,0,0,0,0,0,2,0, },
           {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0, },
           {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0, },
           {0,0,0,0,2,0,0,0,1,0,0,0,0,0,0,0, },
           {0,0,0,0,0,2,0,0,1,0,0,0,0,0,0,0, },
           {0,0,0,0,0,0,2,0,1,0,0,0,0,0,0,0, },
           {0,0,0,0,0,0,0,2,1,0,0,0,0,0,0,0, },
        };
        private readonly int[,] Yang = new int[,]
        {
           {0,0,0,2,0,0,2,2,2,2,2,2,2,2,2,2, },
           {0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0, },
           {0,0,0,1,0,0,0,0,0,0,0,0,0,1,0,0, },
           {0,0,0,1,0,0,0,0,0,0,0,1,1,0,0,0, },
           {2,2,2,2,2,2,2,0,0,2,2,2,2,2,2,2, },
           {0,0,1,1,1,0,0,0,0,0,1,0,0,2,0,1, },
           {0,2,0,1,0,2,0,0,0,0,1,0,0,1,0,1, },
           {2,0,0,1,0,0,2,0,0,2,0,0,2,0,0,1, },
           {0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,1, },
           {0,0,0,1,0,0,0,0,2,0,0,2,0,0,0,1, },
           {0,0,0,1,0,0,0,0,1,0,0,1,0,0,0,1, },
           {0,0,0,1,0,0,0,2,0,0,2,0,0,0,0,1, },
           {0,0,0,1,0,0,0,1,0,0,1,0,0,0,0,1, },
           {0,0,0,1,0,0,2,0,0,2,0,0,1,0,0,1, },
           {0,0,0,1,0,0,0,0,0,1,0,0,0,1,0,1, },
           {0,0,0,1,0,0,0,0,0,0,0,0,0,0,2,2, },
        };

        //生
        private readonly int[,] Shen = new int[,]
        {
           {0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0, },
           {0,0,0,0,0,2,0,0,1,0,0,0,0,0,0,0, },
           {0,0,0,0,2,0,0,0,1,0,0,0,0,0,0,0, },
           {0,0,0,0,1,2,2,2,2,2,2,2,2,0,0,0, },
           {0,0,0,2,0,0,0,0,1,0,0,0,0,0,0,0, },
           {0,0,2,0,0,0,0,0,1,0,0,0,0,0,0,0, },
           {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0, },
           {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0, },
           {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0, },
           {0,0,0,2,2,2,2,2,2,2,2,2,2,0,0,0, },
           {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0, },
           {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0, },
           {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0, },
           {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0, },
           {0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0, },
           {0,0,2,2,2,2,2,2,2,2,2,2,2,2,0,0, },
        };

        //快
        private readonly int[,] Kuai = new int[,]
        {
           {0,0,0,2,0,0,0,0,0,2,0,0,0,0,0,0, },
           {0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0, },
           {0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0, },
           {0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0, },
           {0,0,0,1,0,2,0,2,2,2,2,2,2,2,0,0, },
           {0,2,0,1,0,1,0,0,0,1,0,0,0,2,0,0, },
           {0,1,0,1,0,0,2,0,0,1,0,0,0,2,0,0, },
           {2,0,0,1,0,0,0,0,0,1,0,0,0,2,0,0, },
           {1,0,0,1,0,2,2,2,2,2,2,2,2,2,2,2, },
           {0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0, },
           {0,0,0,1,0,0,0,0,2,0,2,0,0,0,0,0, },
           {0,0,0,1,0,0,0,0,1,0,0,2,0,0,0,0, },
           {0,0,0,1,0,0,0,1,0,0,0,0,2,0,0,0, },
           {0,0,0,1,0,0,2,0,0,0,0,0,0,2,0,0, },
           {0,0,0,1,0,0,1,0,0,0,0,0,0,0,2,0, },
           {0,0,0,2,0,2,0,0,0,0,0,0,0,0,0,2, },
        };

        private void OnEnable()
        {
            mapData = target as MapData;
        }

        private void AddArrayToMap(int[,] anyArray, int arrayX, int arrayY, int x, int y, int height)
        {
            var worldData = mapData.WorldData;

            for (var i = 0; i < arrayX; i++)
            {
                for (var j = 0; j < arrayY; j++)
                {
                    worldData[height + i, x + j, y] = anyArray[arrayX - i - 1, j];
                }
            }
            mapData.WorldData = worldData;
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            mapData.max_height = EditorGUILayout.IntField("Height", mapData.max_height);
            mapData.max_width = EditorGUILayout.IntField("Width", mapData.max_width);
            mapData.max_length = EditorGUILayout.IntField("Length", mapData.max_length);

            if (GUILayout.Button("Generate World By Rule One"))
            {
                var data = new int[mapData.max_height, mapData.max_width, mapData.max_length];

                for (var heightIndex = 0; heightIndex < mapData.max_height; heightIndex++)
                {
                    for (var i = 0; i < mapData.max_width; i++)
                    {
                        for (var j = 0; j < mapData.max_length; j++)
                        {
                            var blockID = 0;

                            if (heightIndex == 0)
                            {
                                blockID = 3;
                            }
                            else if (heightIndex < 15)
                            {
                                blockID = 1;
                            }
                            else if (heightIndex < 16)
                            {
                                blockID = 2;
                            }

                            data[heightIndex, i, j] = blockID;
                        }
                    }
                }

                mapData.WorldData = data;
            }

            if (GUILayout.Button("Add Happy Birthday"))
            {
                AddArrayToMap(Xiao, 16, 16, 2, 58, 17);
                AddArrayToMap(Yang, 16, 16, 16, 62, 17);
                AddArrayToMap(Shen, 16, 16, 34, 58, 17);
                AddArrayToMap(Kuai, 16, 16, 50, 62, 17);
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }

}
