using MC.Core.Interface;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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


        private GameObject weaponModel;

        public GameObject weaponPrefab;

        public Vector3 slotPos, slotEulerAngle;

        public float fireRate = 0.1f;

        public Mag[] defaultMags = new Mag[3];

        public SoundClips soundClips;

        public BulletData bulletData;

        public bool clickToFire = false;

        private float lastfireTime = 0;

        private int currentSelectedMug = 0;

        private Animator animator;

        private ParticleSystem sparkParticles;

        private ParticleSystem muzzleParticles;

        private AudioSource mainAudioSource;

        private Transform ffPoint;

        private bool isReloading = false;

        private bool isZooming = false;

        private Player player;

        private Text WeaponName, SlotCount;

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

                ToggleZooming(false);

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

        public void Fire()
        {
            if (Time.time - lastfireTime > fireRate && !isReloading)
            {
                if (HasBullet())
                {
                    UseBullet();
                    lastfireTime = Time.time;

                    if (isZooming)
                    {
                        animator.Play("Aim Fire", 0, 0f);
                    }
                    else
                    {
                        animator.Play("Fire", 0, 0f);
                    }

                    var bullet = new GameObject("Bullet", typeof(Bullet));
                    bullet.transform.position = ffPoint.position;
                    bullet.transform.rotation = ffPoint.rotation;
                    bullet.GetComponent<Bullet>().bulletData = bulletData;

                    muzzleParticles.Emit(1);
                    sparkParticles.Emit(Random.Range(1, 7));

                    PlaySound(soundClips.shootSound);

                    SlotCount.text = $"Ammos:{defaultMags[currentSelectedMug].ammoCount}/{defaultMags[currentSelectedMug].ammoMaxCount}";
                }
                else
                {
                    player.StartCoroutine(Reload());
                }

            }
        }

        public float GetEndurance()
        {
            return 100;
        }

        public float GetHPDamage()
        {
            return 0;
        }

        public bool IsUseable()
        {
            return false;
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
                ffPoint = weaponModel.transform.Find("Armature/weapon/Components/Bullet Spawn Point");

                mainAudioSource = weaponModel.AddComponent<AudioSource>();

                player = inventorySystem.player;

                WeaponName = player.weaponBar.transform.Find("WeaponName/Text").GetComponent<Text>();
                SlotCount = player.weaponBar.transform.Find("SlotCount/Text").GetComponent<Text>();
            }

            PlaySound(soundClips.takeOutSound);

            weaponModel.SetActive(true);
            player.weaponBar.SetActive(true);

            player.gunFireBtn.gameObject.SetActive(true);
            player.gunFireBtn.onClick.AddListener(Fire);

            WeaponName.text = inventoryName;
            player.weaponBar.transform.Find("WeaponIcon").GetComponent<Image>().sprite = inventoryIcon;

            inventorySystem.player.OnUpdated += OnUpdated;

            ControlEvents.OnGunFire += Fire;
        }

        public override void OnUnselected(InventorySystem inventorySystem)
        {
            if (weaponModel != null)
            {
                weaponModel.SetActive(false);

                player.weaponBar.SetActive(false);

                player.gunFireBtn.gameObject.SetActive(false);
                player.gunFireBtn.onClick.RemoveListener(Fire);

                inventorySystem.player.OnUpdated -= OnUpdated;
                ControlEvents.OnGunFire -= Fire;
            }
        }

        public void UseEndurance(float usedEndurance)
        {

        }

        private void OnUpdated()
        {
            if (!Util.IsMobile())
            {
                if (!isReloading)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        ToggleZooming(true);
                    }
                    if (Input.GetKeyUp(KeyCode.Mouse1))
                    {
                        ToggleZooming(false);
                    }

                    if (clickToFire)
                    {
                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            Fire();
                        }
                    }
                    else
                    {
                        if (Input.GetKey(KeyCode.Mouse0))
                        {
                            Fire();
                        }
                    }

                }

            }
        }

        private void ToggleZooming(bool state)
        {
            isZooming = state;

            animator.SetBool("Aim", isZooming);

            PlaySound(soundClips.aimSound);
        }

        //步枪系统 采用其他的工具方式
        public void Attack(Player attacker)
        {
            return;
        }
    }

}
