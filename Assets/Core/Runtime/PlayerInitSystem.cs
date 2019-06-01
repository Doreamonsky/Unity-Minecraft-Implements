using System.Collections;
using System.Linq;
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
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("Achievements"), achievementData);

            Debug.Log(PlayerPrefs.GetString("Achievements"));

            player.inventorySystem.inventoryStorageList = achievementData.inventoryStorageList.Select(val => new InventoryStorage()
            {
                count = val.count,
                inventory = InventoryManager.Instance.GetInventoryByName(val.name),
                slotID = val.slotID
            }).ToList();

            player.inventorySystem.UpdateInvetoryUI();
        }

        public void SaveStorage()
        {
            achievementData.inventoryStorageList = player.inventorySystem.inventoryStorageList.Select(val => new AchievementData.InventorySaving()
            {
                count = val.count,
                name = val.inventory.inventoryName,
                slotID = val.slotID
            }
            ).ToList();

            Debug.Log(JsonUtility.ToJson(achievementData));

            PlayerPrefs.SetString("Achievements", JsonUtility.ToJson(achievementData));
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
