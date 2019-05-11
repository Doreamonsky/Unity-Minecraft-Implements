using System.Collections.Generic;
using UnityEngine;

namespace MC.Core
{
    public class WorldManager : MonoBehaviour
    {
        //id 0 empty
        //id 1 dirt
        public struct BlockMap
        {
            public int id;
            public BlockData blockData;
        }

        public List<BlockMap> blockMaps = new List<BlockMap>();

        //Height Width Length
        //Origin Point (0,0,0)
        public int[,,] worldData;

        private const int width = 256, length = 256, height = 16;

        private enum QuadStatus
        {
            Front = 0,
            Back = 1,
            Top = 2,
            Bottom = 3,
            Right = 4,
            Left = 5
        }

        private void Start()
        {

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
                        if (heightIndex == 0)
                        {
                            worldData[heightIndex, width, length] = 1;
                        }
                        else
                        {
                            worldData[heightIndex, width, length] = 0;
                        }
                    }
                }
            }
        }

        private void RenderMesh(int startHeight, int endHeight, int startWidth, int endWidth, int startLength, int endLength)
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
                                DrawQuad()
                            }
                        }

                    }
                }
            }
        }

        private void DrawQuad(int x, int y,int z, QuadStatus quadStatus)
        {

        }
    }

}
