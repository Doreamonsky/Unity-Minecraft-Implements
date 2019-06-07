using System.Collections;
using UnityEngine;

namespace MC.Core
{
    public class PlayerInitSystem : MonoBehaviour
    {
        public GameObject runtime;

        public Player player;

        public AchievementData achievementData;

        public bool useSaving = false;

        public void Init()
        {
            runtime = Instantiate(Resources.Load<GameObject>("Runtime"));

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
            yield return new WaitForEndOfFrame();

            achievementData = ScriptableObject.CreateInstance<AchievementData>();
            GeneralStorageSystem.LoadFile(achievementData, "Achievements");

            player.inventorySystem.inventoryStorageList = achievementData.inventoryStorageList;
            player.inventorySystem.UpdateInvetoryUI();
        }

        public void SaveStorage()
        {
            achievementData.inventoryStorageList = player.inventorySystem.inventoryStorageList;
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
