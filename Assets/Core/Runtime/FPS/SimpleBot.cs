using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace MC.Core
{
    public class SimpleBot : Monster, IHittableCharacter
    {
        //public Transform destination;

        public string currentWeapon;

        public CharacterData characterData;

        public WeaponData weaponData;

        public BotLogic botLogic;

        private NavMeshAgent meshAgent;

        private TPSAnimationController animationController;

        // 武器模型
        private GameObject weapon;

        // 武器参数
        private bool isReloading = false;

        private float lastfireTime = 0;

        private int currentSelectedMug = 0;

        private AudioSource mainWeaponAudioSource;

        public override void Start()
        {
            //base.Start();

            //Init Bot Model
            var bot = Instantiate(characterData.character);
            bot.transform.SetParent(transform);

            bot.transform.localPosition = Vector3.zero;
            bot.transform.localRotation = Quaternion.identity;

            var slotData = characterData.weaponIKData.slotDataList.Find(val => val.weapon.name == currentWeapon);

            weapon = Instantiate(slotData.weapon);
            weapon.transform.SetParent(bot.transform.Find("metarig/hips"));
            weapon.transform.localPosition = slotData.weaponPos;
            weapon.transform.localRotation = slotData.weaponRot;

            var leftHand = new GameObject("Left Hand");
            leftHand.transform.SetParent(weapon.transform);
            leftHand.transform.localPosition = slotData.leftHandPos;
            leftHand.transform.localRotation = slotData.leftHandRot;

            var rightHand = new GameObject("Right Hand");
            rightHand.transform.SetParent(weapon.transform);
            rightHand.transform.localPosition = slotData.rightHandPos;
            rightHand.transform.localRotation = slotData.rightHandRot;

            animationController = bot.AddComponent<TPSAnimationController>();
            animationController.weaponSlot = weapon.transform;

            meshAgent = gameObject.AddComponent<NavMeshAgent>();
            meshAgent.speed = 5;
            meshAgent.angularSpeed = 360;
            meshAgent.acceleration = 4;

            botLogic.OnInitialized(new LogicData()
            {
                simpleBot = this,
                navMeshAgent = meshAgent,
                animationController = animationController
            });

            // 武器设置
            mainWeaponAudioSource = weapon.AddComponent<AudioSource>();
        }



        public override void ApplyDamage(float damage)
        {
            base.ApplyDamage(damage);
        }
        public override void Update()
        {
            //meshAgent.SetDestination(destination.position);
            animationController.velocity = meshAgent.velocity.magnitude / meshAgent.speed;
            //base.Update();
        }

        public Vector3 GetSpotPoint()
        {
            return transform.position + transform.up * 1.6f;
        }

        public Transform GetAimTransform()
        {
            return transform;
        }
        // 武器控制
        public void WeaponFire()
        {
            if (Time.time - lastfireTime > weaponData.fireRate && !isReloading)
            {
                if (HasBullet())
                {
                    UseBullet();
                    lastfireTime = Time.time;

                    var bullet = new GameObject("Bullet", typeof(Bullet));
                    bullet.transform.position = weapon.transform.position;
                    bullet.transform.rotation = weapon.transform.rotation;
                    bullet.GetComponent<Bullet>().bulletData = weaponData.bulletData;

                    //muzzleParticles.Emit(1);
                    //sparkParticles.Emit(Random.Range(1, 7));

                    PlaySound(weaponData.soundClips.shootSound);
                }
                else
                {
                    Reload();
                }

            }
        }

        private bool HasBullet()
        {
            return weaponData.defaultMags[currentSelectedMug].ammoCount > 0;
        }

        private void UseBullet()
        {
            weaponData.defaultMags[currentSelectedMug].ammoCount -= 1;
        }

        private void PlaySound(AudioClip clip)
        {
            mainWeaponAudioSource.clip = clip;
            mainWeaponAudioSource.Play();
        }

        private IEnumerator Reload()
        {
            if (isReloading)
            {
                yield break;
            }

            if (currentSelectedMug < weaponData.defaultMags.Length - 1)
            {
                isReloading = true;

                currentSelectedMug++;

                PlaySound(weaponData.soundClips.reloadSoundAmmoLeft);

                yield return new WaitForSeconds(2.5f);

                isReloading = false;
            }
        }
    }
}
