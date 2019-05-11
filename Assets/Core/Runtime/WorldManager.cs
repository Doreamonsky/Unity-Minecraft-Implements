using System.Collections.Generic;
using UnityEngine;

namespace MC.Core
{
    public enum QuadStatus
    {
        Front = 0,
        Back = 1,
        Top = 2,
        Bottom = 3,
        Right = 4,
        Left = 5
    }

    [System.Serializable]
    public class BlockMap
    {
        public class RuntimeRendererData
        {
            public List<Vector3> vertices = new List<Vector3>();

            public List<int> triangles = new List<int>(); //三角面要顺时针连线为法线方向

            public List<Vector2> uvs = new List<Vector2>();

            private Mesh mesh;

            private MeshRenderer m_MeshRenderer;
            private MeshFilter m_MeshFilter;

            public void Initialize(GameObject parent, Material material)
            {
                mesh = new Mesh();

                var gameObject = new GameObject("_Temp");
                gameObject.transform.SetParent(parent.transform);

                m_MeshRenderer = gameObject.AddComponent<MeshRenderer>();
                m_MeshFilter = gameObject.AddComponent<MeshFilter>();

                m_MeshFilter.mesh = mesh;
                m_MeshRenderer.material = material;
            }

            public void Apply()
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
            }

        }

        public BlockData blockData;

        public RuntimeRendererData frontData = new RuntimeRendererData(), backData = new RuntimeRendererData(), topData = new RuntimeRendererData(), bottomData = new RuntimeRendererData(), leftData = new RuntimeRendererData(), rightData = new RuntimeRendererData();

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
        }

        public void Apply()
        {
            frontData.Apply();
            backData.Apply();

            topData.Apply();
            bottomData.Apply();

            leftData.Apply();
            rightData.Apply();
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
                    return null;
            }
        }
    }

    public class WorldManager : MonoBehaviour
    {
        //id 0 empty
        //id 1 dirt
        public List<BlockMap> blockMaps = new List<BlockMap>();

        //Height Width Length
        //Origin Point (0,0,0)
        public int[,,] worldData;

        private const int width = 32, length = 32, height = 16;

        private void Start()
        {
            foreach (var blockMap in blockMaps)
            {
                blockMap.Initialize(gameObject);
            }

            GenerateWorld();
        }

        private void GenerateWorld()
        {
            worldData = new int[height, width, length];

            for (var heightIndex = 0; heightIndex < height; heightIndex++)
            {
                for (var i = 0; i < width; i++)
                {
                    for (var j = 0; j < length; j++)
                    {
                        if (heightIndex == 1)
                        {
                            worldData[heightIndex, i, j] = 1;
                        }
                        else if (heightIndex == 4 && i == 8 && j == 8)
                        {
                            worldData[heightIndex, i, j] = 1;
                        }
                        else if (i == 5 && heightIndex == 2)
                        {
                            worldData[heightIndex, i, j] = 1;
                        }
                        else
                        {
                            worldData[heightIndex, i, j] = 0;
                        }

                    }
                }
            }

            RenderBlocks(0, height, 0, width, 0, length);
        }

        //渲染规定区域内的Block
        private void RenderBlocks(int startHeight, int endHeight, int startWidth, int endWidth, int startLength, int endLength)
        {
            for (var heightIndex = startHeight; heightIndex < endHeight; heightIndex++)
            {
                for (var i = startWidth; i < endWidth; i++)
                {
                    for (var j = startLength; j < endLength; j++)
                    {
                        var blockID = worldData[heightIndex, i, j];

                        if (blockID == 0)
                        {
                            continue;
                        }

                        //绘制顶部面片
                        if (heightIndex < height - 1)
                        {
                            var topBlockID = worldData[heightIndex + 1, i, j];

                            if (topBlockID == 0)
                            {
                                DrawQuad(heightIndex, i, j, QuadStatus.Top);
                            }
                        }

                        //渲染底部面片
                        if (heightIndex >= 1)
                        {
                            var bottomBlockID = worldData[heightIndex - 1, i, j];

                            if (bottomBlockID == 0)
                            {
                                DrawQuad(heightIndex, i, j, QuadStatus.Bottom);
                            }
                        }

                        //渲染右侧面片
                        if (i < width - 1)
                        {
                            var rightBlockID = worldData[heightIndex, i + 1, j];

                            if (rightBlockID == 0)
                            {
                                DrawQuad(heightIndex, i, j, QuadStatus.Right);
                            }
                        }

                        //渲染左侧面片
                        if (i > 0)
                        {
                            var leftBlockID = worldData[heightIndex, i - 1, j];

                            if (leftBlockID == 0)
                            {
                                DrawQuad(heightIndex, i, j, QuadStatus.Left);
                            }
                        }

                        //渲染前侧面片
                        if (j < length - 1)
                        {
                            var frontBlockID = worldData[heightIndex, i, j + 1];

                            if (frontBlockID == 0)
                            {
                                DrawQuad(heightIndex, i, j, QuadStatus.Front);
                            }
                        }

                        //渲染后侧面片
                        if (j > 0)
                        {
                            var backBlockID = worldData[heightIndex, i, j - 1];

                            if (backBlockID == 0)
                            {
                                DrawQuad(heightIndex, i, j, QuadStatus.Back);
                            }
                        }
                    }
                }
            }

            foreach (var blockMap in blockMaps)
            {
                blockMap.Apply();
            }
        }

        private void DrawQuad(int height, int x, int y, QuadStatus quadStatus)
        {
            var blockID = worldData[height, x, y];
            var blockMap = blockMaps[blockID];

            var pivot = new Vector3(x, height, y);

            var runtimeData = blockMap.GetRunTimeRendererData(quadStatus);

            var startIndex = runtimeData.vertices.Count;

            switch (quadStatus)
            {
                case QuadStatus.Top:
                    runtimeData.vertices.Add(pivot + new Vector3(0, 1, 1));
                    runtimeData.vertices.Add(pivot + new Vector3(1, 1, 1));
                    runtimeData.vertices.Add(pivot + new Vector3(0, 1, 0));
                    runtimeData.vertices.Add(pivot + new Vector3(1, 1, 0));

                    runtimeData.triangles.Add(startIndex);
                    runtimeData.triangles.Add(startIndex + 1);
                    runtimeData.triangles.Add(startIndex + 2);
                    runtimeData.triangles.Add(startIndex + 2);
                    runtimeData.triangles.Add(startIndex + 1);
                    runtimeData.triangles.Add(startIndex + 3);

                    break;
                case QuadStatus.Bottom:
                    runtimeData.vertices.Add(pivot + new Vector3(0, 0, 1));
                    runtimeData.vertices.Add(pivot + new Vector3(1, 0, 1));
                    runtimeData.vertices.Add(pivot + new Vector3(0, 0, 0));
                    runtimeData.vertices.Add(pivot + new Vector3(1, 0, 0));

                    runtimeData.triangles.Add(startIndex);
                    runtimeData.triangles.Add(startIndex + 2);
                    runtimeData.triangles.Add(startIndex + 1);
                    runtimeData.triangles.Add(startIndex + 1);
                    runtimeData.triangles.Add(startIndex + 2);
                    runtimeData.triangles.Add(startIndex + 3);
                    break;

                case QuadStatus.Right:
                    runtimeData.vertices.Add(pivot + new Vector3(1, 1, 0));
                    runtimeData.vertices.Add(pivot + new Vector3(1, 1, 1));
                    runtimeData.vertices.Add(pivot + new Vector3(1, 0, 0));
                    runtimeData.vertices.Add(pivot + new Vector3(1, 0, 1));

                    runtimeData.triangles.Add(startIndex);
                    runtimeData.triangles.Add(startIndex + 1);
                    runtimeData.triangles.Add(startIndex + 2);
                    runtimeData.triangles.Add(startIndex + 2);
                    runtimeData.triangles.Add(startIndex + 1);
                    runtimeData.triangles.Add(startIndex + 3);
                    break;
                case QuadStatus.Left:
                    runtimeData.vertices.Add(pivot + new Vector3(0, 1, 0));
                    runtimeData.vertices.Add(pivot + new Vector3(0, 1, 1));
                    runtimeData.vertices.Add(pivot + new Vector3(0, 0, 0));
                    runtimeData.vertices.Add(pivot + new Vector3(0, 0, 1));

                    runtimeData.triangles.Add(startIndex);
                    runtimeData.triangles.Add(startIndex + 2);
                    runtimeData.triangles.Add(startIndex + 1);
                    runtimeData.triangles.Add(startIndex + 1);
                    runtimeData.triangles.Add(startIndex + 2);
                    runtimeData.triangles.Add(startIndex + 3);
                    break;
                case QuadStatus.Front:
                    runtimeData.vertices.Add(pivot + new Vector3(0, 1, 0));
                    runtimeData.vertices.Add(pivot + new Vector3(1, 1, 0));
                    runtimeData.vertices.Add(pivot + new Vector3(0, 0, 0));
                    runtimeData.vertices.Add(pivot + new Vector3(1, 0, 0));

                    runtimeData.triangles.Add(startIndex);
                    runtimeData.triangles.Add(startIndex + 1);
                    runtimeData.triangles.Add(startIndex + 2);
                    runtimeData.triangles.Add(startIndex + 2);
                    runtimeData.triangles.Add(startIndex + 1);
                    runtimeData.triangles.Add(startIndex + 3);
                    break;
                case QuadStatus.Back:
                    runtimeData.vertices.Add(pivot + new Vector3(0, 1, 1));
                    runtimeData.vertices.Add(pivot + new Vector3(1, 1, 1));
                    runtimeData.vertices.Add(pivot + new Vector3(0, 0, 1));
                    runtimeData.vertices.Add(pivot + new Vector3(1, 0, 1));

                    runtimeData.triangles.Add(startIndex);
                    runtimeData.triangles.Add(startIndex + 2);
                    runtimeData.triangles.Add(startIndex + 1);
                    runtimeData.triangles.Add(startIndex + 1);
                    runtimeData.triangles.Add(startIndex + 2);
                    runtimeData.triangles.Add(startIndex + 3);
                    break;
            }

            runtimeData.uvs.Add(new Vector2(0, 1));
            runtimeData.uvs.Add(new Vector2(1, 1));
            runtimeData.uvs.Add(new Vector2(0, 0));
            runtimeData.uvs.Add(new Vector2(1, 0));
        }
    }

}
