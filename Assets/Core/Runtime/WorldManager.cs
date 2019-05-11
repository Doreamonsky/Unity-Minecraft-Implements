using System.Collections.Generic;
using UnityEngine;

namespace MC.Core
{
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
        public Vector3 pos;

        public int verticeIndex;
    }

    public class ColliderCache
    {
        public Vector3 pos;

        public BoxCollider collider;
    }
    //方块渲染数据储存【实时】
    public class RuntimeRendererData
    {
        public List<Vector3> vertices = new List<Vector3>();

        public List<int> triangles = new List<int>(); //三角面要顺时针连线为法线方向

        public List<Vector2> uvs = new List<Vector2>();

        public List<RendererCache> rendererCaches = new List<RendererCache>();
        private Mesh mesh;

        private MeshRenderer m_MeshRenderer;
        private MeshFilter m_MeshFilter;

        public void Initialize(GameObject parent, Material material)
        {
            mesh = new Mesh();

            var gameObject = new GameObject($"{material.name}_Temp");
            gameObject.transform.SetParent(parent.transform);

            m_MeshRenderer = gameObject.AddComponent<MeshRenderer>();
            m_MeshFilter = gameObject.AddComponent<MeshFilter>();

            m_MeshFilter.mesh = mesh;
            m_MeshRenderer.material = material;
        }

        public void ApplyChanges()
        {
            if (mesh == null)
            {
                return;
            }

            mesh.Clear();

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();

            mesh.uv = uvs.ToArray();

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }

    }

    [System.Serializable]
    public class BlockMap
    {
        public BlockData blockData;

        public RuntimeRendererData frontData = new RuntimeRendererData(), backData = new RuntimeRendererData(), topData = new RuntimeRendererData(), bottomData = new RuntimeRendererData(), leftData = new RuntimeRendererData(), rightData = new RuntimeRendererData();

        public List<RuntimeRendererData> runtimeRendererDataList = new List<RuntimeRendererData>();

        public List<ColliderCache> colliderCacheList = new List<ColliderCache>();

        public void Initialize(GameObject parent)
        {
            if (blockData == null)
            {
                return;
            }

            frontData.Initialize(parent, blockData.frontTex);
            backData.Initialize(parent, blockData.backTex);

            topData.Initialize(parent, blockData.topTex);
            bottomData.Initialize(parent, blockData.bottomTex);

            leftData.Initialize(parent, blockData.leftTex);
            rightData.Initialize(parent, blockData.rightTex);

            runtimeRendererDataList = new List<RuntimeRendererData>()
            {
                frontData,
                backData,
                topData,
                bottomData,
                leftData,
                rightData
            };

        }

        public void Apply()
        {
            foreach (var runtimeData in runtimeRendererDataList)
            {
                runtimeData.ApplyChanges();
            }
        }

        public RuntimeRendererData GetRunTimeRendererData(QuadStatus quadStatus)
        {
            switch (quadStatus)
            {
                case QuadStatus.Front:
                    return frontData;
                case QuadStatus.Back:
                    return backData;
                case QuadStatus.Top:
                    return topData;
                case QuadStatus.Bottom:
                    return bottomData;
                case QuadStatus.Right:
                    return rightData;
                case QuadStatus.Left:
                    return leftData;
                default:
                    Debug.LogError("Runtime-Error Excepiton Type");
                    return null;
            }
        }
    }

    public class WorldManager : MonoBehaviour
    {
        public static WorldManager Instance;

        //id 0 empty
        //id 1 dirt
        public List<BlockMap> blockMaps = new List<BlockMap>();

        public MapData mapData;

        private Transform colliderParent;


        private void Start()
        {
            colliderParent = new GameObject("Collision").transform;

            foreach (var blockMap in blockMaps)
            {
                blockMap.Initialize(gameObject);
            }

            GenerateWorld();

            Instance = this;
        }

        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.A))
        //    {
        //        CreateBlock(14, 9, 9, 1);
        //    }
        //    if (Input.GetKeyDown(KeyCode.D))
        //    {
        //        RemoveBlock(2, 5, 8);
        //    }
        //}
        private void GenerateWorld()
        {
            RenderBlocks(0, mapData.max_height - 1, 0, mapData.max_width - 1, 0, mapData.max_length - 1);
        }

        public void CreateBlock(int height, int x, int y, int type)
        {
            mapData.worldData[height, x, y] = type;
            RenderBlocks(height, height, x, x, y, y);
        }

        public void RemoveBlock(int height, int x, int y)
        {
            //删除碰撞
            var blockID = mapData.worldData[height, x, y];
            var colliderCache = blockMaps[blockID].colliderCacheList.Find(val => val.pos == new Vector3(x, height, y));

            if (colliderCache != null)
            {
                Destroy(colliderCache.collider.gameObject);
                blockMaps[blockID].colliderCacheList.Remove(colliderCache);
            }

            mapData.worldData[height, x, y] = 0;

            RemoveRenderBlock(height, x, y);
        }

        //渲染规定区域内的Block
        private void RenderBlocks(int startHeight, int endHeight, int startWidth, int endWidth, int startLength, int endLength)
        {
            for (var heightIndex = startHeight; heightIndex <= endHeight; heightIndex++)
            {
                for (var i = startWidth; i <= endWidth; i++)
                {
                    for (var j = startLength; j <= endLength; j++)
                    {
                        var blockID = mapData.worldData[heightIndex, i, j];

                        if (blockID != 0)
                        {
                            var isVisible = false;

                            //绘制顶部面片
                            if (heightIndex < mapData.max_height - 1)
                            {
                                var topBlockID = mapData.worldData[heightIndex + 1, i, j];

                                if (topBlockID == 0)
                                {
                                    isVisible = true;
                                    DrawQuad(heightIndex, i, j, QuadStatus.Top);
                                }
                            }

                            //渲染底部面片
                            if (heightIndex >= 1)
                            {
                                var bottomBlockID = mapData.worldData[heightIndex - 1, i, j];

                                if (bottomBlockID == 0)
                                {
                                    isVisible = true;
                                    DrawQuad(heightIndex, i, j, QuadStatus.Bottom);
                                }
                            }

                            //渲染右侧面片
                            if (i < mapData.max_width - 1)
                            {
                                var rightBlockID = mapData.worldData[heightIndex, i + 1, j];

                                if (rightBlockID == 0)
                                {
                                    isVisible = true;
                                    DrawQuad(heightIndex, i, j, QuadStatus.Right);
                                }
                            }

                            //渲染左侧面片
                            if (i > 0)
                            {
                                var leftBlockID = mapData.worldData[heightIndex, i - 1, j];

                                if (leftBlockID == 0)
                                {
                                    isVisible = true;
                                    DrawQuad(heightIndex, i, j, QuadStatus.Left);
                                }
                            }

                            //渲染前侧面片
                            if (j < mapData.max_length - 1)
                            {
                                var frontBlockID = mapData.worldData[heightIndex, i, j + 1];

                                if (frontBlockID == 0)
                                {
                                    isVisible = true;
                                    DrawQuad(heightIndex, i, j, QuadStatus.Front);
                                }
                            }

                            //渲染后侧面片
                            if (j > 0)
                            {
                                var backBlockID = mapData.worldData[heightIndex, i, j - 1];

                                if (backBlockID == 0)
                                {
                                    isVisible = true;
                                    DrawQuad(heightIndex, i, j, QuadStatus.Back);
                                }
                            }

                            //生成碰撞
                            if (isVisible)
                            {
                                var blockMap = blockMaps[blockID];

                                if (blockMap.colliderCacheList.Find(val => val.pos == new Vector3(i, heightIndex, j)) == null)
                                {
                                    var collider = new GameObject("Collider", typeof(BoxCollider)).GetComponent<BoxCollider>();

                                    collider.gameObject.layer = LayerMask.NameToLayer("Block");
                                    collider.center = new Vector3(0.5f, 0.5f, 0.5f);
                                    collider.transform.position = new Vector3(i, heightIndex, j);
                                    collider.transform.SetParent(colliderParent);

                                    blockMap.colliderCacheList.Add(new ColliderCache()
                                    {
                                        pos = new Vector3(i, heightIndex, j),
                                        collider = collider
                                    });
                                }
                            }
                        }
                    }
                }
            }

            //面片渲染修改
            foreach (var blockMap in blockMaps)
            {
                blockMap.Apply();
            }
        }


        //清除渲染规定区域内的Block 
        private void RemoveRenderBlock(int height, int x, int y)
        {
            foreach (var blockMap in blockMaps)
            {
                foreach (var runtimeData in blockMap.runtimeRendererDataList)
                {
                    var caches = runtimeData.rendererCaches.FindAll(val => val.pos == new Vector3(x, height, y));

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

            RenderBlocks(height - 1, height + 1, x - 1, x + 1, y - 1, y + 1);
        }

        private void DrawQuad(int height, int x, int y, QuadStatus quadStatus)
        {
            var blockID = mapData.worldData[height, x, y];
            var blockMap = blockMaps[blockID];

            var pivot = new Vector3(x, height, y);

            var runtimeData = blockMap.GetRunTimeRendererData(quadStatus);

            var verIndex = runtimeData.vertices.Count;

            runtimeData.rendererCaches.Add(new RendererCache()
            {
                pos = new Vector3(x, height, y),
                verticeIndex = verIndex
            });

            switch (quadStatus)
            {
                case QuadStatus.Top:
                    runtimeData.vertices.Add(pivot + new Vector3(0, 1, 1));
                    runtimeData.vertices.Add(pivot + new Vector3(1, 1, 1));
                    runtimeData.vertices.Add(pivot + new Vector3(0, 1, 0));
                    runtimeData.vertices.Add(pivot + new Vector3(1, 1, 0));

                    runtimeData.triangles.Add(verIndex);
                    runtimeData.triangles.Add(verIndex + 1);
                    runtimeData.triangles.Add(verIndex + 2);
                    runtimeData.triangles.Add(verIndex + 2);
                    runtimeData.triangles.Add(verIndex + 1);
                    runtimeData.triangles.Add(verIndex + 3);

                    break;
                case QuadStatus.Bottom:
                    runtimeData.vertices.Add(pivot + new Vector3(0, 0, 1));
                    runtimeData.vertices.Add(pivot + new Vector3(1, 0, 1));
                    runtimeData.vertices.Add(pivot + new Vector3(0, 0, 0));
                    runtimeData.vertices.Add(pivot + new Vector3(1, 0, 0));

                    runtimeData.triangles.Add(verIndex);
                    runtimeData.triangles.Add(verIndex + 2);
                    runtimeData.triangles.Add(verIndex + 1);
                    runtimeData.triangles.Add(verIndex + 1);
                    runtimeData.triangles.Add(verIndex + 2);
                    runtimeData.triangles.Add(verIndex + 3);
                    break;

                case QuadStatus.Right:
                    runtimeData.vertices.Add(pivot + new Vector3(1, 1, 0));
                    runtimeData.vertices.Add(pivot + new Vector3(1, 1, 1));
                    runtimeData.vertices.Add(pivot + new Vector3(1, 0, 0));
                    runtimeData.vertices.Add(pivot + new Vector3(1, 0, 1));

                    runtimeData.triangles.Add(verIndex);
                    runtimeData.triangles.Add(verIndex + 1);
                    runtimeData.triangles.Add(verIndex + 2);
                    runtimeData.triangles.Add(verIndex + 2);
                    runtimeData.triangles.Add(verIndex + 1);
                    runtimeData.triangles.Add(verIndex + 3);
                    break;
                case QuadStatus.Left:
                    runtimeData.vertices.Add(pivot + new Vector3(0, 1, 0));
                    runtimeData.vertices.Add(pivot + new Vector3(0, 1, 1));
                    runtimeData.vertices.Add(pivot + new Vector3(0, 0, 0));
                    runtimeData.vertices.Add(pivot + new Vector3(0, 0, 1));

                    runtimeData.triangles.Add(verIndex);
                    runtimeData.triangles.Add(verIndex + 2);
                    runtimeData.triangles.Add(verIndex + 1);
                    runtimeData.triangles.Add(verIndex + 1);
                    runtimeData.triangles.Add(verIndex + 2);
                    runtimeData.triangles.Add(verIndex + 3);
                    break;
                case QuadStatus.Front:
                    runtimeData.vertices.Add(pivot + new Vector3(0, 1, 1));
                    runtimeData.vertices.Add(pivot + new Vector3(1, 1, 1));
                    runtimeData.vertices.Add(pivot + new Vector3(0, 0, 1));
                    runtimeData.vertices.Add(pivot + new Vector3(1, 0, 1));

                    runtimeData.triangles.Add(verIndex);
                    runtimeData.triangles.Add(verIndex + 2);
                    runtimeData.triangles.Add(verIndex + 1);
                    runtimeData.triangles.Add(verIndex + 1);
                    runtimeData.triangles.Add(verIndex + 2);
                    runtimeData.triangles.Add(verIndex + 3);
                    break;
                case QuadStatus.Back:
                    runtimeData.vertices.Add(pivot + new Vector3(0, 1, 0));
                    runtimeData.vertices.Add(pivot + new Vector3(1, 1, 0));
                    runtimeData.vertices.Add(pivot + new Vector3(0, 0, 0));
                    runtimeData.vertices.Add(pivot + new Vector3(1, 0, 0));

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
    }

}
