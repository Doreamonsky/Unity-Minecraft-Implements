
using System.Collections;
using UnityEngine;

namespace MC.Core
{
    public class FPSPlayerSpawn : MonoBehaviour
    {
        public Transform playerSpawnPoint;

        private void Start()
        {
            var initSystem = new GameObject("InitSystem", typeof(PlayerInitSystem)).GetComponent<PlayerInitSystem>();
            initSystem.transform.position = playerSpawnPoint.position;
            initSystem.transform.rotation = playerSpawnPoint.rotation;
            initSystem.useSaving = false;

            initSystem.onSpawned += (Player player) =>
            {
                StartCoroutine(AssignWeapon(player, "AK 47", 0));
                StartCoroutine(AssignWeapon(player, "Hand Gun", 1));

                //player.inventorySystem.UpdateInvetoryUI();
            };

            initSystem.Init();
        }

        private IEnumerator AssignWeapon(Player player, string weaponName, int slotID)
        {
            yield return new WaitForEndOfFrame();

            player.inventorySystem.AddStorage(new InventoryStorage()
            {
                count = 1,
                inventory = InventoryManager.Instance.GetInventoryByName(weaponName),
                inventoryName = weaponName,
                slotID = slotID
            }
            );
        }

    }
}
