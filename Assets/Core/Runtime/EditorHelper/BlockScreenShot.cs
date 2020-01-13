using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MC.Core.Helper
{

    public class BlockScreenShot : MonoBehaviour
    {
        public List<BlockData> blockDatas = new List<BlockData>();

        public BlockStorageData blockStorageData;

        public List<MapData> mapDatas = new List<MapData>();

        public List<GameObject> prefabs = new List<GameObject>();

        [Header("选择Camera对应Render相应内容")]
        public Camera blockCamera, mapCamera, prefabCamera;

        // Start is called before the first frame update
        void Start()
        {
            if (blockCamera != null)
            {
                StartCoroutine(RenderBlocks());
            }

            if (mapCamera != null)
            {
                StartCoroutine(RenderMapData());
            }
            if (prefabCamera != null)
            {
                StartCoroutine(RenderPrefab());
            }
        }

        private IEnumerator RenderMapData()
        {
            mapCamera.gameObject.SetActive(true);

            foreach (var mapData in mapDatas)
            {
                var worldManager = new GameObject("World Manager", typeof(WorldManager)).GetComponent<WorldManager>();
                worldManager.blockStorageData = blockStorageData;
                worldManager.mapData = mapData;

                yield return new WaitForSeconds(1);

                worldManager.transform.position -= worldManager.transform.forward * mapData.max_width * 0.5f;
                worldManager.transform.position -= worldManager.transform.right * mapData.max_length * 0.5f;


                Rect lRect = new Rect(0f, 0f, Screen.width, Screen.height);

                yield return new WaitForEndOfFrame();

                Texture2D capturedImage = zzTransparencyCapture.capture(lRect);

                byte[] byt = capturedImage.EncodeToPNG();

                var dic = new DirectoryInfo("Others/Renderering/MapData/");

                if (!dic.Exists)
                {
                    dic.Create();
                }

                File.WriteAllBytes("Others/Renderering/MapData/" + mapData.name + ".png", byt);

                yield return new WaitForSeconds(1);

                Destroy(worldManager.gameObject);
            }
        }
        private IEnumerator RenderBlocks()
        {
            blockCamera.gameObject.SetActive(true);

            foreach (var block in blockDatas)
            {
                var worldManager = new GameObject("World Manager", typeof(WorldManager)).GetComponent<WorldManager>();
                worldManager.blockStorageData = blockStorageData;

                var worldData = new int[3, 3, 3];
                worldData[1, 1, 1] = blockStorageData.BlockMapping.Find(x => x.blockData == block).layerID;

                var mapData = ScriptableObject.CreateInstance<MapData>();
                mapData.max_height = 3;
                mapData.max_length = 3;
                mapData.max_width = 3;
                mapData.WorldData = worldData;

                mapData.isSaveable = false;
                worldManager.mapData = mapData;

                yield return new WaitForSeconds(1);

                Rect lRect = new Rect(0f, 0f, Screen.width, Screen.height);

                yield return new WaitForEndOfFrame();

                Texture2D capturedImage = zzTransparencyCapture.capture(lRect);

                byte[] byt = capturedImage.EncodeToPNG();

                var dic = new DirectoryInfo("Others/Renderering/Blocks/");

                if (!dic.Exists)
                {
                    dic.Create();
                }

                File.WriteAllBytes("Others/Renderering/Blocks/" + block.name + ".png", byt);

                yield return new WaitForSeconds(1);

                Destroy(worldManager.gameObject);
            }
        }

        private IEnumerator RenderPrefab()
        {
            prefabCamera.gameObject.SetActive(true);

            foreach (var prefab in prefabs)
            {
                var instance = Instantiate(prefab);

                yield return new WaitForSeconds(1);

                Rect lRect = new Rect(0f, 0f, Screen.width, Screen.height);

                yield return new WaitForEndOfFrame();

                Texture2D capturedImage = zzTransparencyCapture.capture(lRect);

                byte[] byt = capturedImage.EncodeToPNG();

                var dic = new DirectoryInfo("Others/Renderering/Prefab/");

                if (!dic.Exists)
                {
                    dic.Create();
                }

                File.WriteAllBytes("Others/Renderering/Prefab/" + prefab.name + ".png", byt);

                yield return new WaitForSeconds(1);

                Destroy(instance.gameObject);
            }
        }
    }

}
