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

        private readonly float random;

        public float perlinScale = 0.05f;

        public WorldGenerator(int size, int seed)
        {
            Random.InitState(seed);
            random = Random.value * 1000;
            chunckSize = size;
        }


        public MapData GetBlockMap(Vector3 startPos)
        {
            var mapData = ScriptableObject.CreateInstance<MapData>();

            mapData.mapName = $"Chunck_{startPos.x}_{startPos.y}_{startPos.z}";
            mapData.isSaveable = true;

            mapData.max_length = chunckSize;
            mapData.max_width = chunckSize;
            mapData.max_height = 64;
            mapData.startPos = startPos;

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
                        Random.InitState(mapData.seed);
                        var seed = Random.value * 100;

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

                            if (i > 3 && i < mapData.max_width - 3 && j > 3 && j < mapData.max_length - 3)
                            {
                                topBlocks.Add(new Vector3(heightIndex, i, j));
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

            return mapData;
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

        private const int chunckSize = 18;

        private Vector3 playerPos;

        private void Start()
        {
            StartCoroutine(UpdateWorld());

            Player.OnPlayerMove += pos =>
            {
                playerPos = pos;
            };
        }


        private void CreateWorldManager(WorldGenerator worldGenerator, Vector3 pos)
        {
            var mapData = worldGenerator.GetBlockMap(pos);
            var worldManager = new GameObject("WorldManager", typeof(WorldManager)).GetComponent<WorldManager>();

            worldManager.mapData = mapData;
            worldManager.blockStorageData = storageData;

            if (chuncks.Count <= 9)
            {
                worldManager.InstancingRenderer = true;
            }
            else
            {
                worldManager.InstancingRenderer = false;
            }
            chuncks.Add(new Chunck()
            {
                startPos = pos,
                mapData = mapData
            });
        }

        private IEnumerator UpdateWorld()
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
