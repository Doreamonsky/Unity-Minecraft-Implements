using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MC.Core
{
    public class PlayerInitSystem : MonoBehaviour
    {
        public GameObject runtime;

        public Player player;

        public AchievementData achievementData;

        public bool useSaving = false;

        private TOD_Sky skyDome;

        public void Init()
        {
            runtime = Instantiate(Resources.Load<GameObject>("Runtime"));
            skyDome = FindObjectOfType<TOD_Sky>();

            player = runtime.transform.Find("Player").GetComponent<Player>();

            player.gameObject.SetActive(false);

            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;

            if (useSaving)
            {
                StartCoroutine(LoadStorage());
            }

            player.gameObject.SetActive(true);
        }

        private IEnumerator LoadStorage()
        {
            achievementData = ScriptableObject.CreateInstance<AchievementData>();
            GeneralStorageSystem.LoadFile(achievementData, "Achievements");

            if (isInfiniteScene())
            {
                if (achievementData.playerPos != Vector3.zero)
                {
                    player.gameObject.SetActive(false);
                    player.transform.position = achievementData.playerPos;
                    player.gameObject.SetActive(true);
                    Player.OnPlayerMove(achievementData.playerPos);
                }

                skyDome.Cycle = achievementData.timeCycle;

                var infiniteWorld = FindObjectOfType<InfiniteWorld>();
                infiniteWorld.StartCoroutine(infiniteWorld.UpdateWorld());
            }

            yield return new WaitForEndOfFrame();

            player.inventorySystem.inventoryStorageList = achievementData.inventoryStorageList;
            player.inventorySystem.UpdateInvetoryUI();
        }

        private static bool isInfiniteScene()
        {
            return SceneManager.GetActiveScene().name == "InfiniteScene";
        }

        public void SaveStorage()
        {
            achievementData.playerPos = player.transform.position;
            achievementData.inventoryStorageList = player.inventorySystem.inventoryStorageList;
            achievementData.timeCycle = skyDome.Cycle;
            GeneralStorageSystem.SaveFile(achievementData, "Achievements");
        }

        public void OnDestroy()
        {
            if (useSaving)
            {
                SaveStorage();
            }
        }
    }

}
