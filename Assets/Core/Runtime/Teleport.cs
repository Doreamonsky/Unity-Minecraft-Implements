using UnityEngine;
using UnityEngine.SceneManagement;

namespace MC.Core
{
    public class Teleport : MonoBehaviour
    {
        private float lastingTime = 0;

        private Player player;

        private void Update()
        {
            if (player == null)
            {
                player = GameObject.FindObjectOfType<Player>();
                return;
            }

            if (Vector3.ProjectOnPlane(player.transform.position - transform.position, Vector3.up).magnitude < 5)
            {
                lastingTime += Time.deltaTime;
            }
            else
            {
                lastingTime = 0;
            }

            if (lastingTime > 3)
            {
                Util.OnRequireSave?.Invoke();
                SceneManager.LoadScene("InfiniteScene");
            }
        }
    }
}
