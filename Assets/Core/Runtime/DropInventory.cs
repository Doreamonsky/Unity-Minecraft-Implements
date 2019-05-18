using UnityEngine;


namespace MC.Core
{
    public class DropInventory : MonoBehaviour
    {
        public System.Action OnPlayerEnter;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                OnPlayerEnter?.Invoke();
            }
        }
        private void Update()
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 35);
        }
    }
}
