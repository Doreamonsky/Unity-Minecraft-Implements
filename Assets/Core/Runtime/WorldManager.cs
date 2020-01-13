using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MC.Core
{
    //可摆放配件
    public class RuntimePlaceableInventoryData
    {
        public Inventory inventory;

        public GameObject itemInstance;

        public InventoryPlaceData placeData;
    }

    //三角面的位置
    public enum QuadStatus
    {
        Front = 0,
        Back = 1,
        Top = 2,
        Bottom = 3,
        Right = 4,
        Left = 5
    }
    //用于移除渲染的面片
    public class RendererCache
    {
        // 采用坐标
        public Vector3 renderID;

        public int verticeIndex;
    }

    public class ColliderCache
    {
        // 采用坐标
        public Vector3 renderID;

        public BoxCollider collider;
    }


    //方块渲染数据储存【实时】
    public class RuntimeRendererData
    {
        public string matName;

        public List<Vector3> vertices = new List<Vector3>();

        public List<int> triangles = new List<int>(); //三角面要顺时针连线为法线方向

        public List<Vector2> uvs = new List<Vector2>();

        public List<RendererCache> rendererCaches = new List<RendererCache>();
        private Mesh mesh;

        private MeshRenderer m_MeshRenderer;
        private MeshFilter m_MeshFilter;
        //private MeshCollider m_meshCollider;


        public void Initialize(GameObject parent, Material material)
        {
            mesh = new Mesh();

            var gameObject = new GameObject($"{material.name}_Temp");
            gameObject.transform.SetParent(parent.transform);

            m_MeshRenderer = gameObject.AddComponent<MeshRenderer>();
            m_MeshFilter = gameObject.AddComponent<MeshFilter>();
            //m_meshCollider = gameObject.AddComponent<MeshCollider>();

            m_MeshFilter.mesh = mesh;
            //m_meshCollider.sharedMesh = mesh;
            m_MeshRenderer.material = material;
        }

        public void ApplyChanges()
        {
            if (mesh == null)
            {
                return;
            }

            //mesh.Clear();

            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles.ToArray(), 0);
            mesh.SetUVs(0, uvs);

            //mesh.RecalculateBounds();
            //mesh.RecalculateTangents();
            mesh.RecalculateNormals();
        }

    }
    /// <summary>
    /// 运行时候 Block的数据
    /// </summary>
    [System.Serializable]
    public class BlockMap
    {
        public BlockData blockData;

        public List<ColliderCache> colliderCacheList = new List<ColliderCache>();

        public Material GetRunTimeRendererData(QuadStatus quadStatus)
        {
            switch (quadStatus)
            {
                case QuadStatus.Front:
                    return blockData.frontTex;
                case QuadStatus.Back:
                    return blockData.backTex;
                case QuadStatus.Top:
                    return blockData.topTex;
                case QuadStatus.Bottom:
                    return blockData.bottomTex;
                case QuadStatus.Right:
                    return blockData.rightTex;
                case QuadStatus.Left:
                    return blockData.leftTex;
                default:
                    Debug.LogError("Runtime-Error Excepiton Type");
                    return null;
            }
        }
    }

    public class WorldManager : MonoBehaviour
    {
        public MapData mapData;

        public BlockStorageData blockStorageData;

        public bool InstancingRenderer = true;

        //引索与BlockStorageData中一样
        private List<BlockMap> blockMaps = new List<BlockMap>();

        private Transform colliderParent;

        //Block 信息 缓存在内存中，用于快速读取
        private int[,,] runtimeWorldData;

        private List<RuntimeRendererData> runtimeSharedRendererData = new List<RuntimeRendererData>();

        private List<RuntimePlaceableInventoryData> runtimePlaceableInventoryDataList = new List<RuntimePlaceableInventoryData>();

        private static int BatchingRendereringCount = 0;

        public static float scaleSize = 1f;

        public System.Action OnLoaded;

        //private readonly float scaleSize = 0.25f;

        public void Start()
        {
#if UNITY_EDITOR
            mapData = Instantiate(mapData);
#endif
            mapData.OnLoad();


            colliderParent = new GameObject("Collision").transform;
            colliderParent.parent = transform;

            runtimeWorldData = mapData.WorldData;

            for (int i = 0; i < blockStorageData.BlockMapping.Count; i++)
            {
                BlockStorageMapping mapping = blockStorageData.BlockMapping[i];

                blockMaps.Add(new BlockMap()
                {
                    blockData = mapping.blockData
                });
            }

            //foreach (var blockMap in blockMaps)
            //{
            //    blockMap.Initialize(gameObject);
            //}

            var mats = new List<Material>();

            foreach (var blockMap in blockMaps)
            {
                if (blockMap.blockData == null)
                {
                    continue;
                }

                mats.Add(blockMap.blockData.backTex);
                mats.Add(blockMap.blockData.frontTex);
                mats.Add(blockMap.blockData.leftTex);
                mats.Add(blockMap.blockData.rightTex);
                mats.Add(blockMap.blockData.topTex);
                mats.Add(blockMap.blockData.backTex);
            }

            mats = mats.Distinct().ToList();

            foreach (var mat in mats)
            {
                var sharedRenderer = new RuntimeRendererData()
                {
                    matName = mat.name
                };

                sharedRenderer.Initialize(gameObject, mat);

                runtimeSharedRendererData.Add(sharedRenderer);
            }

            GenerateWorld();

            OnLoaded?.Invoke();
        }

        public void SaveMapData()
        {
            if (mapData.isSaveable)
            {
                mapData.WorldData = runtimeWorldData;
                mapData.OnSave();
            }
        }

        private void GenerateWorld()
        {
            StartCoroutine(RenderBlocks(0, mapData.max_height - 1, 0, mapData.max_width - 1, 0, mapData.max_length - 1, InstancingRenderer));

            RenderPlaceableInventory();
        }

        //创建Block 需要外部判断Block的数组位置
        public void CreateBlock(int height, int x, int y, int layerID)
        {
            //x -= (int)mapData.startPos.x;
            //y -= (int)mapData.startPos.z;

            runtimeWorldData[height, x, y] = layerID;
            StartCoroutine(RenderBlocks(height, height, x, x, y, y, true));
        }

        //获取Block信息 需要外部判断Block的数组位置
        public BlockData GetBlockData(int height, int x, int y)
        {
            //x -= (int)mapData.startPos.x;
            //y -= (int)mapData.startPos.z;

            var blockID = runtimeWorldData[height, x, y];
            return blockMaps[blockID].blockData;
        }

        //与Block交互 需要外部判断Block的数组位置
        public void DeleteBlock(int height, int x, int y)
        {
            //x -= (int)mapData.startPos.x;
            //y -= (int)mapData.startPos.z;

            var blockID = runtimeWorldData[height, x, y];
            blockMaps[blockID].blockData.RemoveBlock(this, height, x, y);
        }

        public void RemoveBlock(int height, int x, int y)
        {
            //删除碰撞
            var blockID = runtimeWorldData[height, x, y];
            var colliderCache = blockMaps[blockID].colliderCacheList.Find(val => val.renderID == new Vector3(x, height, y));

            if (colliderCache != null)
            {
                Destroy(colliderCache.collider.gameObject);
                blockMaps[blockID].colliderCacheList.Remove(colliderCache);
            }

            runtimeWorldData[height, x, y] = 0;

            RemoveRenderBlock(height, x, y);

        }

        //渲染规定区域内的Block
        private IEnumerator RenderBlocks(int startHeight, int endHeight, int startWidth, int endWidth, int startLength, int endLength, bool IsInstancing)
        {
            var grouped = 0;

            for (var heightIndex = endHeight; heightIndex >= startHeight; heightIndex--)
            {
                for (var i = startWidth; i <= endWidth; i++)
                {
                    for (var j = startLength; j <= endLength; j++)
                    {
                        if (i >= mapData.max_width || i < 0 || j >= mapData.max_length || j < 0)
                        {
                            continue;
                        }

                        if (!IsInstancing && grouped >= 50)
                        {
                            grouped = 0;

                            while (BatchingRendereringCount > 4)
                            {
                                yield return new WaitForEndOfFrame();
                            }
                            BatchingRendereringCount++;

                            //面片渲染修改
                            foreach (var runtime in runtimeSharedRendererData)
                            {
                                runtime.ApplyChanges();
                            }

                            yield return new WaitForEndOfFrame();

                            BatchingRendereringCount--;
                        }


                        var blockID = runtimeWorldData[heightIndex, i, j];
                        var blockMap = blockMaps[blockID];

                        if (blockID != 0)
                        {
                            var isVisible = false;

                            //绘制顶部面片
                            if (heightIndex < mapData.max_height - 1)
                            {
                                var topBlockID = runtimeWorldData[heightIndex + 1, i, j];

                                if (topBlockID == 0 || blockMap.blockData.forceRenderer)
                                {
                                    isVisible = true;
                                    DrawQuad(heightIndex, i, j, QuadStatus.Top);
                                }
                            }

                            //渲染底部面片
                            if (heightIndex >= 1)
                            {
                                var bottomBlockID = runtimeWorldData[heightIndex - 1, i, j];

                                if (bottomBlockID == 0 || blockMap.blockData.forceRenderer)
                                {
                                    isVisible = true;
                                    DrawQuad(heightIndex, i, j, QuadStatus.Bottom);
                                }
                            }

                            //渲染右侧面片
                            if (i < mapData.max_width)
                            {
                                var rightBlockID = i == mapData.max_width - 1 ? 0 : runtimeWorldData[heightIndex, i + 1, j];

                                if (rightBlockID == 0 || blockMap.blockData.forceRenderer)
                                {
                                    isVisible = true;
                                    DrawQuad(heightIndex, i, j, QuadStatus.Right);
                                }
                            }

                            //渲染左侧面片
                            if (i >= 0)
                            {
                                var leftBlockID = i == 0 ? 0 : runtimeWorldData[heightIndex, i - 1, j];

                                if (leftBlockID == 0 || blockMap.blockData.forceRenderer)
                                {
                                    isVisible = true;
                                    DrawQuad(heightIndex, i, j, QuadStatus.Left);
                                }
                            }

                            //渲染前侧面片
                            if (j < mapData.max_length)
                            {
                                var frontBlockID = j == mapData.max_length - 1 ? 0 : runtimeWorldData[heightIndex, i, j + 1];

                                if (frontBlockID == 0 || blockMap.blockData.forceRenderer)
                                {
                                    isVisible = true;
                                    DrawQuad(heightIndex, i, j, QuadStatus.Front);
                                }
                            }

                            //渲染后侧面片
                            if (j >= 0)
                            {
                                var backBlockID = j == 0 ? 0 : runtimeWorldData[heightIndex, i, j - 1];

                                if (backBlockID == 0 || blockMap.blockData.forceRenderer)
                                {
                                    isVisible = true;
                                    DrawQuad(heightIndex, i, j, QuadStatus.Back);
                                }
                            }

                            if (isVisible)
                            {
                                grouped++;
                            }

                            //生成碰撞
                            if (blockMap.colliderCacheList.Find(val => val.renderID == new Vector3(i, heightIndex, j)) == null)
                            {
                                var collider = new GameObject("Collider", typeof(BoxCollider)).GetComponent<BoxCollider>();

                                collider.gameObject.layer = LayerMask.NameToLayer("Block");
                                collider.center = new Vector3(0.5f, 0.5f, 0.5f);

                                collider.transform.rotation = transform.rotation;
                                collider.transform.localScale = new Vector3(1, 1, 1) * scaleSize;
                                collider.transform.position = /*new Vector3(i*tra, heightIndex, j)*/(transform.forward * j + transform.up * heightIndex + transform.right * i) * scaleSize + transform.position;
                                collider.transform.SetParent(colliderParent);

                                blockMap.colliderCacheList.Add(new ColliderCache()
                                {
                                    renderID = new Vector3(i, heightIndex, j),
                                    collider = collider
                                });
                            }
                        }
                    }
                }
            }

            //面片渲染修改
            foreach (var runtime in runtimeSharedRendererData)
            {
                runtime.ApplyChanges();
            }

        }


        //清除渲染规定区域内的Block 
        private void RemoveRenderBlock(int height, int x, int y)
        {
            foreach (var blockMap in blockMaps)
            {
                foreach (var runtimeData in runtimeSharedRendererData)
                {
                    var caches = runtimeData.rendererCaches.FindAll(val => val.renderID == new Vector3(x, height, y));

                    //不删除runtimeData的数据是防止Index变化
                    for (int i = caches.Count - 1; i >= 0; i--)
                    {
                        RendererCache cache = caches[i];

                        for (var j = cache.verticeIndex; j < cache.verticeIndex + 4; j++)
                        {
                            runtimeData.vertices[j] = Vector3.zero;
                        }

                        runtimeData.rendererCaches.Remove(cache);
                    }
                }

            }

            StartCoroutine(RenderBlocks(height - 1, height + 1, x - 1, x + 1, y - 1, y + 1, true));
        }

        private void DrawQuad(int height, int x, int y, QuadStatus quadStatus)
        {
            var blockID = runtimeWorldData[height, x, y];
            var blockMap = blockMaps[blockID];

            var pivot = new Vector3(x, height, y) * scaleSize;

            var runtimeData = runtimeSharedRendererData.Find(val => val.matName == blockMap.GetRunTimeRendererData(quadStatus).name);

            var verIndex = runtimeData.vertices.Count;

            runtimeData.rendererCaches.Add(new RendererCache()
            {
                renderID = new Vector3(x, height, y),
                verticeIndex = verIndex
            });

            switch (quadStatus)
            {
                case QuadStatus.Top:
                    runtimeData.vertices.Add(pivot + new Vector3(0, 1, 1) * scaleSize + mapData.startPos * scaleSize);
                    runtimeData.vertices.Add(pivot + new Vector3(1, 1, 1) * scaleSize + mapData.startPos * scaleSize);
                    runtimeData.vertices.Add(pivot + new Vector3(0, 1, 0) * scaleSize + mapData.startPos * scaleSize);
                    runtimeData.vertices.Add(pivot + new Vector3(1, 1, 0) * scaleSize + mapData.startPos * scaleSize);

                    runtimeData.triangles.Add(verIndex);
                    runtimeData.triangles.Add(verIndex + 1);
                    runtimeData.triangles.Add(verIndex + 2);
                    runtimeData.triangles.Add(verIndex + 2);
                    runtimeData.triangles.Add(verIndex + 1);
                    runtimeData.triangles.Add(verIndex + 3);

                    break;
                case QuadStatus.Bottom:
                    runtimeData.vertices.Add(pivot + new Vector3(0, 0, 1) * scaleSize + mapData.startPos * scaleSize);
                    runtimeData.vertices.Add(pivot + new Vector3(1, 0, 1) * scaleSize + mapData.startPos * scaleSize);
                    runtimeData.vertices.Add(pivot + new Vector3(0, 0, 0) * scaleSize + mapData.startPos * scaleSize);
                    runtimeData.vertices.Add(pivot + new Vector3(1, 0, 0) * scaleSize + mapData.startPos * scaleSize);

                    runtimeData.triangles.Add(verIndex);
                    runtimeData.triangles.Add(verIndex + 2);
                    runtimeData.triangles.Add(verIndex + 1);
                    runtimeData.triangles.Add(verIndex + 1);
                    runtimeData.triangles.Add(verIndex + 2);
                    runtimeData.triangles.Add(verIndex + 3);
                    break;

                case QuadStatus.Right:
                    runtimeData.vertices.Add(pivot + new Vector3(1, 1, 0) * scaleSize + mapData.startPos * scaleSize);
                    runtimeData.vertices.Add(pivot + new Vector3(1, 1, 1) * scaleSize + mapData.startPos * scaleSize);
                    runtimeData.vertices.Add(pivot + new Vector3(1, 0, 0) * scaleSize + mapData.startPos * scaleSize);
                    runtimeData.vertices.Add(pivot + new Vector3(1, 0, 1) * scaleSize + mapData.startPos * scaleSize);

                    runtimeData.triangles.Add(verIndex);
                    runtimeData.triangles.Add(verIndex + 1);
                    runtimeData.triangles.Add(verIndex + 2);
                    runtimeData.triangles.Add(verIndex + 2);
                    runtimeData.triangles.Add(verIndex + 1);
                    runtimeData.triangles.Add(verIndex + 3);
                    break;
                case QuadStatus.Left:
                    runtimeData.vertices.Add(pivot + new Vector3(0, 1, 0) * scaleSize + mapData.startPos * scaleSize);
                    runtimeData.vertices.Add(pivot + new Vector3(0, 1, 1) * scaleSize + mapData.startPos * scaleSize);
                    runtimeData.vertices.Add(pivot + new Vector3(0, 0, 0) * scaleSize + mapData.startPos * scaleSize);
                    runtimeData.vertices.Add(pivot + new Vector3(0, 0, 1) * scaleSize + mapData.startPos * scaleSize);

                    runtimeData.triangles.Add(verIndex);
                    runtimeData.triangles.Add(verIndex + 2);
                    runtimeData.triangles.Add(verIndex + 1);
                    runtimeData.triangles.Add(verIndex + 1);
                    runtimeData.triangles.Add(verIndex + 2);
                    runtimeData.triangles.Add(verIndex + 3);
                    break;
                case QuadStatus.Front:
                    runtimeData.vertices.Add(pivot + new Vector3(0, 1, 1) * scaleSize + mapData.startPos * scaleSize);
                    runtimeData.vertices.Add(pivot + new Vector3(1, 1, 1) * scaleSize + mapData.startPos * scaleSize);
                    runtimeData.vertices.Add(pivot + new Vector3(0, 0, 1) * scaleSize + mapData.startPos * scaleSize);
                    runtimeData.vertices.Add(pivot + new Vector3(1, 0, 1) * scaleSize + mapData.startPos * scaleSize);

                    runtimeData.triangles.Add(verIndex);
                    runtimeData.triangles.Add(verIndex + 2);
                    runtimeData.triangles.Add(verIndex + 1);
                    runtimeData.triangles.Add(verIndex + 1);
                    runtimeData.triangles.Add(verIndex + 2);
                    runtimeData.triangles.Add(verIndex + 3);
                    break;
                case QuadStatus.Back:
                    runtimeData.vertices.Add(pivot + new Vector3(0, 1, 0) * scaleSize + mapData.startPos * scaleSize);
                    runtimeData.vertices.Add(pivot + new Vector3(1, 1, 0) * scaleSize + mapData.startPos * scaleSize);
                    runtimeData.vertices.Add(pivot + new Vector3(0, 0, 0) * scaleSize + mapData.startPos * scaleSize);
                    runtimeData.vertices.Add(pivot + new Vector3(1, 0, 0) * scaleSize + mapData.startPos * scaleSize);

                    runtimeData.triangles.Add(verIndex);
                    runtimeData.triangles.Add(verIndex + 1);
                    runtimeData.triangles.Add(verIndex + 2);
                    runtimeData.triangles.Add(verIndex + 2);
                    runtimeData.triangles.Add(verIndex + 1);
                    runtimeData.triangles.Add(verIndex + 3);

                    break;
            }

            runtimeData.uvs.Add(new Vector2(0, 1));
            runtimeData.uvs.Add(new Vector2(1, 1));
            runtimeData.uvs.Add(new Vector2(0, 0));
            runtimeData.uvs.Add(new Vector2(1, 0));
        }

        //渲染可放置物体
        private void RenderPlaceableInventory()
        {
            foreach (var placeData in mapData.inventoryPlaceDataList)
            {
                PlaceInventory(placeData);
            }
        }

        public void CreatePlaceableInventory(PlaceableInventory placeableInventory, Vector3Int pos, Vector3 dir)
        {
            var placeData = new InventoryPlaceData()
            {
                pos = pos,
                placeDir = dir,
                inventoryName = placeableInventory.inventoryName,
            };

            mapData.inventoryPlaceDataList.Add(placeData);
            PlaceInventory(placeData);

        }

        public RuntimePlaceableInventoryData GetItemData(Vector3Int pos)
        {
            return runtimePlaceableInventoryDataList.Find(val => val.placeData.pos == pos);
        }

        public void RemoveInventroy(RuntimePlaceableInventoryData runtimePlaceableInventoryData)
        {
            Destroy(runtimePlaceableInventoryData.itemInstance);

            runtimePlaceableInventoryDataList.Remove(runtimePlaceableInventoryData);
            mapData.inventoryPlaceDataList.Remove(runtimePlaceableInventoryData.placeData);
        }

        private void PlaceInventory(InventoryPlaceData placeData)
        {
            var inventory = InventoryManager.Instance.GetInventoryByName(placeData.inventoryName) as PlaceableInventory;
            var itemBlockPos = transform.position + (transform.right * 0.5f + transform.forward * 0.5f + transform.up * 0.5f) + (placeData.pos.x * transform.right + placeData.pos.y * transform.up + placeData.pos.z * transform.forward) * scaleSize;
            var itemInstance = Instantiate(inventory.itemModel, itemBlockPos, Quaternion.LookRotation(transform.TransformDirection(placeData.placeDir)));
            itemInstance.transform.parent = transform;

            runtimePlaceableInventoryDataList.Add(new RuntimePlaceableInventoryData()
            {
                inventory = inventory,
                itemInstance = itemInstance,
                placeData = placeData
            });
        }
    }

}
