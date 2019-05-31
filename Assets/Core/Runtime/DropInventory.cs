using System.Collections;
using UnityEngine;

namespace MC.Core
{
    public class DropInventory : MonoBehaviour
    {
        public System.Action OnPlayerEnter;

        private bool isCollected = false;

        private Vector3 touchPos;

        private void OnTriggerEnter(Collider other)
        {
            if (isCollected)
            {
                return;
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                OnPlayerEnter?.Invoke();

                touchPos = other.transform.position;
            }
        }
        private void Update()
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 35);
        }

        public void Collect()
        {
            isCollected = true;

            StartCoroutine(PlayColletAim(touchPos));
        }
        private IEnumerator PlayColletAim(Vector3 pos)
        {
            var t = 0f;

            while (true)
            {
                t += Time.deltaTime;

                transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 2);

                if (t > 2f)
                {
                    break;
                }

                yield return new WaitForEndOfFrame();
            }

            Destroy(gameObject);
        }
    }
}
