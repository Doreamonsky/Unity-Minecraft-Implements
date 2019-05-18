using UnityEngine;

namespace MC.Core
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;

        public AudioSource digSource;

        private float lastDigTime = 0;

        private void Start()
        {
            Instance = this;
        }

        public void PlayDigSound(AudioClip audioClip)
        {
            if (Time.time - lastDigTime > 0.25f)
            {
                digSource.clip = audioClip;
                digSource.Play();

                lastDigTime = Time.time;
            }
        }
    }
}
