using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OPS.AntiCheat
{
    public class DoNotDestroyBehaviour : MonoBehaviour
    {
        private static DoNotDestroyBehaviour Singleton;
        private void Awake()
        {
            if (Singleton != null)
            {
                DestroyImmediate(this.gameObject);
                return;
            }

            Singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}