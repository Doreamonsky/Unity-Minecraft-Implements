using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MC.Core
{
    public class PlayerInitSystem : MonoBehaviour
    {
        public System.Action<Player> onSpawned;

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

            Util.OnRequireSave += () =>
            {
                if (useSaving)
                {
                    SaveStorage();
                }
            };

            onSpawned?.Invoke(player);
        }

        private IEnumerator LoadStorage()
        {
            achievementData = ScriptableObject.CreateInstance<AchievementData>();
            GeneralStorageSystem.LoadFile(achievementData, "Achievements");

            if (isInfiniteScene())
            {
                if (achievementData.infinitePlayerPos != Vector3.zero)
                {
                    player.gameObject.SetActive(false);
                    player.transform.position = achievementData.infinitePlayerPos;
                    player.gameObject.SetActive(true);
                    Player.OnPlayerMove(achievementData.infinitePlayerPos);
                }

                skyDome.Cycle = achievementData.timeCycle;

                var infiniteWorld = FindObjectOfType<InfiniteWorld>();
                infiniteWorld.StartCoroutine(infiniteWorld.UpdateWorld());
            }

            yield return new WaitForEndOfFrame();

            player.inventorySystem.inventoryStorageList = achievementData.inventoryStorageList;
            player.inventorySystem.UpdateInvetoryUI();

            CraftSystem.Instance.craftInventoryList = achievementData.craftStorageList;
            CraftSystem.Instance.UpdateInvetoryUI();
        }

        private static bool isInfiniteScene()
        {
            return SceneManager.GetActiveScene().name == "InfiniteScene";
        }

        public void SaveStorage()
        {
            if (isInfiniteScene())
            {
                achievementData.infinitePlayerPos = player.transform.position;
                achievementData.timeCycle = skyDome.Cycle;
            }

            achievementData.inventoryStorageList = player.inventorySystem.inventoryStorageList;
            achievementData.craftStorageList = CraftSystem.Instance.craftInventoryList;
            GeneralStorageSystem.SaveFile(achievementData, "Achievements");
        }


    }

}
