using UnityEngine;

namespace MC.Core
{
    public class InfinitePlayerSpawn : MonoBehaviour
    {
        public Transform playerSpawnPoint;

        private void Start()
        {
            var initSystem = new GameObject("InitSystem", typeof(PlayerInitSystem)).GetComponent<PlayerInitSystem>();
            initSystem.transform.position = playerSpawnPoint.position;
            initSystem.transform.rotation = playerSpawnPoint.rotation;
            initSystem.useSaving = true;
            initSystem.Init();
        }
    }
}
