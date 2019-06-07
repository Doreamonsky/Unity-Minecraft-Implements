using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC.Core
{
    public class WorldGenerator
    {
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

        private readonly int chunckSize;

        public float perlinScale = 0.05f;

        private readonly int currentSeed;

        public WorldGenerator(int size, int seed)
        {
            Random.InitState(seed);
            chunckSize = size;
            currentSeed = seed;
        }


        public IEnumerator GetBlockMap(Vector3 startPos, bool isInstancingRender, System.Action<MapData> OnMapped)
        {
            var mapData = ScriptableObject.CreateInstance<MapData>();

            mapData.mapName = $"Chunck_{startPos.x}_{startPos.y}_{startPos.z}";
            mapData.isSaveable = true;

            //如果没有生成过，使用生成算法！
            if (!mapData.HasSaving())
            {
                mapData.max_length = chunckSize;
                mapData.max_width = chunckSize;
                mapData.max_height = 64;
                mapData.startPos = startPos;
                mapData.seed = currentSeed;

                var data = new int[mapData.max_height, mapData.max_width, mapData.max_length];
                //平地
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

                            data[heightIndex, i, j] = blockID;
                        }
                    }
                }
                //山丘
                for (var heightIndex = 15; heightIndex < mapData.max_height; heightIndex++)
                {
                    for (var i = 0; i < mapData.max_width; i++)
                    {
                        for (var j = 0; j < mapData.max_length; j++)
                        {
                            var maxHeight = GetNoise(i + startPos.x, j + startPos.z) * 8 + 15;

                            if (heightIndex < maxHeight)
                            {
                                data[heightIndex, i, j] = 1;
                            }
                        }
                    }
                }
                //草地
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

                                if (i > 4 && i < mapData.max_width - 4 && j > 4 && j < mapData.max_length - 4)
                                {
                                    topBlocks.Add(new Vector3(heightIndex, i, j));
                                }

                                if (Random.value > 0.9f)
                                {
                                    mapData.inventoryPlaceDataList.Add(new InventoryPlaceData()
                                    {
                                        pos = new Vector3(i, heightIndex + 1, j) + startPos + new Vector3(0.5f, 0, 0.5f),
                                        eulerAngle = Vector3.zero,
                                        inventoryName = "Plant Grass"
                                    });
                                }

                                break;
                            }
                        }

                    }
                }

                //数目
                foreach (var topBlock in topBlocks)
                {
                    if (Random.value > 0.94f)
                    {
                        if (!isInstancingRender)
                        {
                            yield return new WaitForEndOfFrame();
                        }

                        if (Random.value > 0.5f)
                        {
                            AddXYZArrayToMap(tree01, 7, 5, 5, data, (int)topBlock.y, (int)topBlock.z, (int)topBlock.x);
                        }
                        else
                        {
                            AddXYZArrayToMap(tree02, 7, 5, 5, data, (int)topBlock.y, (int)topBlock.z, (int)topBlock.x);
                        }
                    }
                }

                mapData.WorldData = data;
            }


            OnMapped(mapData);
        }

        private void AddXYZArrayToMap(int[,,] anyArray, int arrayX, int arrayY, int arrayZ, int[,,] worldData, int x, int y, int height)
        {
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
        }


        private float GetNoise(float i, float j)
        {
            return Mathf.PerlinNoise(i * perlinScale, j * perlinScale);
        }
    }

    public class InfiniteWorld : MonoBehaviour
    {
        public class Chunck
        {
            public Vector3 startPos;

            public MapData mapData;
        }

        public BlockStorageData storageData;

        public List<Chunck> chuncks = new List<Chunck>();

        public bool autoStart = true;

        private const int chunckSize = 24;

        private Vector3 playerPos;

        private void Start()
        {
            Player.OnPlayerMove += pos =>
            {
                playerPos = pos;
            };

            if (autoStart)
            {
                StartCoroutine(UpdateWorld());
            }
        }


        private void CreateWorldManager(WorldGenerator worldGenerator, Vector3 pos)
        {
            var isInstancingRenderer = false;

            if (chuncks.Count <= 9)
            {
                isInstancingRenderer = true;
            }
            else
            {
                isInstancingRenderer = false;
            }

            var chunck = new Chunck()
            {
                startPos = pos,
                mapData = null
            };

            chuncks.Add(chunck);

            StartCoroutine(worldGenerator.GetBlockMap(pos, isInstancingRenderer, mapData =>
            {
                var worldManager = new GameObject("WorldManager", typeof(WorldManager)).GetComponent<WorldManager>();

                worldManager.mapData = mapData;
                worldManager.blockStorageData = storageData;
                worldManager.InstancingRenderer = isInstancingRenderer;

                chunck.mapData = mapData;
            }));


        }

        public IEnumerator UpdateWorld()
        {
            var worldGenerator = new WorldGenerator(chunckSize, 12);

            while (true)
            {
                var planePos = new Vector3((int)(playerPos.x - chunckSize * 0.5f) / chunckSize, 0, (int)(playerPos.z - chunckSize * 0.5f) / chunckSize) * chunckSize;

                var posList = new List<Vector3>()
                {
                   new Vector3(chunckSize*0,0,chunckSize*0) + planePos,
                   new Vector3(chunckSize*1,0,chunckSize*0)+ planePos,
                   new Vector3(chunckSize*0,0,chunckSize*1)+ planePos,
                   new Vector3(chunckSize*1,0,chunckSize*1)+ planePos,
                   new Vector3(chunckSize*-1,0,chunckSize*0)+ planePos,
                   new Vector3(chunckSize*0,0,chunckSize*-1)+ planePos,
                   new Vector3(chunckSize*-1,0,chunckSize*-1)+ planePos,
                   new Vector3(chunckSize*1,0,chunckSize*-1)+ planePos,
                   new Vector3(chunckSize*-1,0,chunckSize*1)+ planePos
                };

                foreach (var pos in posList)
                {
                    var chunck = chuncks.Find(val => val.startPos == pos);

                    if (chunck == null)
                    {
                        CreateWorldManager(worldGenerator, pos);
                    }
                }

                yield return new WaitForSeconds(1);
            }
        }
    }
}
