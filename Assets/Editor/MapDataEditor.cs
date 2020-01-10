using MC.Core;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MC.CoreEditor
{
    [CustomEditor(typeof(MapData))]
    public class MapDataEditor : Editor
    {
        private MapData mapData;

        private readonly int[,,] tree01 = new int[,,]
        {
            {
                {0,0,0,0,0},
                {0,0,0,0,0},
                {0,0,4,0,0},
                {0,0,0,0,0},
                {0,0,0,0,0},
            },
            {
                {0,0,0,0,0},
                {0,0,0,0,0},
                {0,0,4,0,0},
                {0,0,0,0,0},
                {0,0,0,0,0},
            },
            {
                {0,0,0,0,0},
                {0,0,0,0,0},
                {0,0,4,0,0},
                {0,0,0,0,0},
                {0,0,0,0,0},
            },
            {
                {0,0,0,0,0},
                {0,0,0,0,0},
                {0,0,4,0,0},
                {0,0,0,0,0},
                {0,0,0,0,0},
            },
            {
                {6,6,6,6,6},
                {6,6,6,6,6},
                {6,6,4,6,6},
                {6,6,6,6,6},
                {6,6,6,6,6},
            },
            {
                {0,0,0,0,0},
                {0,6,6,6,0},
                {0,6,4,6,0},
                {0,6,6,6,0},
                {0,0,0,0,0},
            },
            {
                {0,0,0,0,0},
                {0,0,0,0,0},
                {0,0,6,0,0},
                {0,0,0,0,0},
                {0,0,0,0,0},
            },
        };

        private readonly int[,,] tree02 = new int[,,]
{
            {
                {0,0,0,0,0},
                {0,0,0,0,0},
                {0,0,4,0,0},
                {0,0,0,0,0},
                {0,0,0,0,0},
            },
            {
                {0,0,0,0,0},
                {0,0,0,0,0},
                {0,0,4,0,0},
                {0,0,0,0,0},
                {0,0,0,0,0},
            },
            {
                {0,0,0,0,0},
                {0,6,0,6,0},
                {0,6,4,0,0},
                {0,0,0,0,0},
                {0,0,0,0,0},
            },
            {
                {0,0,0,0,0},
                {0,6,6,6,0},
                {0,6,4,0,0},
                {0,6,0,6,0},
                {0,0,0,0,0},
            },
            {
                {6,6,6,6,6},
                {6,6,6,6,6},
                {6,6,4,6,6},
                {6,6,6,6,6},
                {6,6,6,6,6},
            },
            {
                {0,0,0,0,0},
                {0,6,6,6,0},
                {0,6,4,6,0},
                {0,6,6,6,0},
                {0,0,0,0,0},
            },
            {
                {0,0,0,0,0},
                {0,0,0,0,0},
                {0,0,6,0,0},
                {0,0,0,0,0},
                {0,0,0,0,0},
            },
};

        //琲
        private readonly int[,] Bei = new int[,]
{
           {2,2,2,2,2,0,0,0,0,2,0,0,2,0,0,0, },
           {0,0,1,0,0,0,0,0,0,1,0,0,1,0,0,0, },
           {0,0,1,0,0,0,0,0,0,1,0,0,1,0,0,0, },
           {0,0,1,0,0,0,2,2,2,1,0,0,1,2,2,2, },
           {0,0,1,0,0,0,0,0,0,1,0,0,1,0,0,0, },
           {0,0,1,0,0,0,0,0,0,1,0,0,1,0,0,0, },
           {0,0,1,0,2,0,0,0,0,1,0,0,1,0,0,0, },
           {0,2,2,2,0,0,2,2,2,1,0,0,1,2,2,2, },
           {2,0,1,0,0,0,0,0,0,1,0,0,1,0,0,0, },
           {0,0,1,0,0,0,0,0,0,1,0,0,1,0,0,0, },
           {0,0,1,0,0,0,0,0,0,1,0,0,1,0,0,0, },
           {0,0,1,0,0,0,0,0,0,1,0,0,1,0,0,0, },
           {0,0,1,0,0,0,2,2,2,1,0,0,1,2,2,2, },
           {0,0,1,0,2,0,0,0,0,1,0,0,1,0,0,0, },
           {0,2,1,2,0,0,0,0,0,1,0,0,1,0,0,0, },
           {2,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0, },
};

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

        private void AddXZArrayToMap(int[,] anyArray, int arrayX, int arrayY, int x, int y, int height)
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

        private void AddXYZArrayToMap(int[,,] anyArray, int arrayX, int arrayY, int arrayZ, int x, int y, int height)
        {
            var worldData = mapData.WorldData;

            for (var i = 0; i < arrayX; i++)
            {
                for (var j = 0; j < arrayY; j++)
                {
                    for (var k = 0; k < arrayZ; k++)
                    {
                        if (anyArray[i, j, k] == 0)
                        {
                            continue;
                        }

                        worldData[height + i, x + j, y + k] = anyArray[i, j, k];
                    }
                }
            }
            mapData.WorldData = worldData;
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            mapData.mapName = EditorGUILayout.TextField("Name", mapData.mapName);
            mapData.isSaveable = EditorGUILayout.Toggle("Save", mapData.isSaveable);
            mapData.max_height = EditorGUILayout.IntField("Height", mapData.max_height);
            mapData.max_width = EditorGUILayout.IntField("Width", mapData.max_width);
            mapData.max_length = EditorGUILayout.IntField("Length", mapData.max_length);

            mapData.seed = EditorGUILayout.IntField("Seed", mapData.seed);

            if (GUILayout.Button("Generate World By Rule One"))
            {
                var data = new int[mapData.max_height, mapData.max_width, mapData.max_length];

                for (var heightIndex = 0; heightIndex < mapData.max_height; heightIndex++)
                {
                    for (var i = 0; i < mapData.max_width; i++)
                    {
                        for (var j = 0; j < mapData.max_length; j++)
                        {
                            //已经被设置
                            if (data[heightIndex, i, j] != 0)
                            {
                                continue;
                            }

                            var blockID = 0;

                            //基岩
                            if (heightIndex == 0)
                            {
                                blockID = 3;
                            }
                            //地表
                            else if (heightIndex < mapData.mineHeight)
                            {
                                var isMine = Random.value > 0.9;

                                if (isMine)
                                {
                                    var rv = Random.value;

                                    if (rv < 0.1f)
                                    {
                                        blockID = 1; //泥土
                                    }
                                    else if (rv < 0.25f)
                                    {
                                        blockID = 10; //煤炭
                                    }
                                    else if (rv < 0.35f)
                                    {
                                        blockID = 11; //铁矿
                                    }
                                    else
                                    {
                                        blockID = 1;
                                    }

                                    if (heightIndex > 1)
                                    {
                                        data[heightIndex - 1, i, j] = blockID;
                                    }

                                    if (i > 0)
                                    {
                                        data[heightIndex, i - 1, j] = blockID;
                                    }

                                    var randomT = Random.Range(2, 4);

                                    if (j > randomT)
                                    {
                                        for (var t = 1; t < randomT; t++)
                                        {
                                            data[heightIndex, i, j - randomT] = blockID;
                                        }
                                    }
                                }
                                else
                                {
                                    blockID = 9;
                                }
                            }

                            data[heightIndex, i, j] = blockID;
                        }
                    }
                }

                mapData.WorldData = data;
            }

            if (GUILayout.Button("Add Hills"))
            {
                var data = mapData.WorldData;

                for (var heightIndex = mapData.mineHeight; heightIndex < mapData.max_height; heightIndex++)
                {
                    for (var i = 0; i < mapData.max_width; i++)
                    {
                        for (var j = 0; j < mapData.max_length; j++)
                        {
                            Random.InitState(mapData.seed);
                            var seed = Random.value * 100;

                            var scale = 5.5f;

                            var maxHeight = Mathf.PerlinNoise(i * scale / mapData.max_width + seed, j * scale / mapData.max_length + seed) * mapData.grassHeightRange + mapData.grassHeight;

                            if (heightIndex < maxHeight)
                            {
                                data[heightIndex, i, j] = 1;
                            }
                        }
                    }
                }

                mapData.WorldData = data;
            }

            if (GUILayout.Button("Add Grass & Trees"))
            {
                var data = mapData.WorldData;

                var topBlocks = new List<Vector3>();

                for (var i = 0; i < mapData.max_width; i++)
                {
                    for (var j = 0; j < mapData.max_length; j++)
                    {
                        for (var heightIndex = mapData.max_height - 1; heightIndex > 0; heightIndex--)
                        {
                            if (data[heightIndex, i, j] != 0)
                            {
                                data[heightIndex, i, j] = 2;

                                if (i > 20 && i < mapData.max_width - 20 && j > 20 && j < mapData.max_length - 20)
                                {
                                    topBlocks.Add(new Vector3(heightIndex, i, j));
                                }

                                break;
                            }
                        }

                    }
                }

                mapData.WorldData = data;

                foreach (var topBlock in topBlocks)
                {
                    if (Random.value > 0.99f)
                    {
                        if (Random.value > 0.5f)
                        {
                            AddXYZArrayToMap(tree01, 7, 5, 5, (int)topBlock.y, (int)topBlock.z, (int)topBlock.x);
                        }
                        else
                        {
                            AddXYZArrayToMap(tree02, 7, 5, 5, (int)topBlock.y, (int)topBlock.z, (int)topBlock.x);
                        }
                    }
                }
            }

            if (GUILayout.Button("Add Happy Birthday"))
            {
                AddXZArrayToMap(Bei, 16, 16, 2, 58 + 32, 24);
                AddXZArrayToMap(Bei, 16, 16, 16, 62 + 32, 24);
                AddXZArrayToMap(Shen, 16, 16, 34, 58 + 32, 24);
                AddXZArrayToMap(Kuai, 16, 16, 50, 62 + 32, 24);
            }
            if (GUILayout.Button("Add Trees"))
            {
                AddXYZArrayToMap(tree01, 7, 5, 5, 2, 2, 16);
                AddXYZArrayToMap(tree01, 7, 5, 5, 12, 32, 16);
                AddXYZArrayToMap(tree01, 7, 5, 5, 18, 32, 16);
                AddXYZArrayToMap(tree01, 7, 5, 5, 19, 52, 16);
                AddXYZArrayToMap(tree01, 7, 5, 5, 35, 10, 16);
            }

            if (GUILayout.Button("Update Map Size"))
            {
                if (EditorUtility.DisplayDialog("Warning", "It will wipe all the map data", "I Understand it!"))
                {
                    mapData.UpdateMapSize();
                }
            }
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }

}
