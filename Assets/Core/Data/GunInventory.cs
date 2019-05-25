using MC.Core.Interface;
using System.Collections;
using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName = "GunInventory", menuName = "GunInventory")]
    public class GunInventory : Inventory, IAttackable
    {
        [System.Serializable]
        public class Mag
        {
            public float ammoCount = 31;

            public float ammoMaxCount = 31;
        }


        [System.Serializable]
        public class SoundClips
        {
            public AudioClip shootSound;
            public AudioClip takeOutSound;
            public AudioClip holsterSound;
            public AudioClip reloadSoundOutOfAmmo;
            public AudioClip reloadSoundAmmoLeft;
            public AudioClip aimSound;
        }


        private static GameObject weaponModel;

        public GameObject weaponPrefab;

        public Vector3 slotPos, slotEulerAngle;

        public float fireRate = 0.1f;

        public Mag[] defaultMags = new Mag[3];

        public SoundClips soundClips;

        private float lastfireTime = 0;

        private int currentSelectedMug = 0;

        private Animator animator;

        private ParticleSystem sparkParticles;

        private ParticleSystem muzzleParticles;

        private AudioSource mainAudioSource;

        private bool isReloading = false;

        private bool HasBullet()
        {
            return defaultMags[currentSelectedMug].ammoCount > 0;
        }

        private void UseBullet()
        {
            defaultMags[currentSelectedMug].ammoCount -= 1;
        }

        private IEnumerator Reload()
        {
            if (isReloading)
            {
                yield break;
            }

            if (currentSelectedMug < defaultMags.Length - 1)
            {
                isReloading = true;

                currentSelectedMug++;

                animator.Play("Reload Ammo Left", 0, 0f);

                PlaySound(soundClips.reloadSoundAmmoLeft);

                yield return new WaitForSeconds(2.5f);

                isReloading = false;
            }
        }


        private void PlaySound(AudioClip clip)
        {
            mainAudioSource.clip = clip;
            mainAudioSource.Play();
        }

        public void Attack(Player attacker)
        {
            if (Time.time - lastfireTime > fireRate && !isReloading)
            {
                if (HasBullet())
                {
                    UseBullet();
                    lastfireTime = Time.time;

                    animator.Play("Fire", 0, 0f);

                    muzzleParticles.Emit(1);
                    sparkParticles.Emit(Random.Range(1, 7));

                    PlaySound(soundClips.shootSound);
                }
                else
                {
                    attacker.StartCoroutine(Reload());
                }

            }
        }


        public float GetDigBoost()
        {
            throw new System.NotImplementedException();
        }

        public float GetEndurance()
        {
            throw new System.NotImplementedException();
        }

        public float GetHPDamage()
        {
            throw new System.NotImplementedException();
        }

        public bool IsUseable()
        {
            throw new System.NotImplementedException();
        }

        public override void OnSelected(InventorySystem inventorySystem)
        {
            if (weaponModel == null)
            {
                weaponModel = Instantiate(weaponPrefab, inventorySystem.weaponSlot, true);
                weaponModel.transform.localPosition = slotPos;
                weaponModel.transform.localEulerAngles = slotEulerAngle;

                animator = weaponModel.GetComponent<Animator>();
                muzzleParticles = weaponModel.transform.Find("Armature/weapon/Components/Muzzleflash Particles").GetComponent<ParticleSystem>();
                sparkParticles = weaponModel.transform.Find("Armature/weapon/Components/SparkParticles").GetComponent<ParticleSystem>();

                mainAudioSource = weaponModel.AddComponent<AudioSource>();

                PlaySound(soundClips.takeOutSound);
            }

            weaponModel.SetActive(true);
        }

        public override void OnUnselected(InventorySystem inventorySystem)
        {
            if (weaponModel != null)
            {
                weaponModel.SetActive(false);
            }
        }

        public void UseEndurance(float usedEndurance)
        {
            throw new System.NotImplementedException();
        }
    }

}
