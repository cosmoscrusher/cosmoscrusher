using JetBrains.Annotations;

using UnityEngine;

namespace Assets.Scripts.New.UnitWeapons.BossWeapons
{
    public class FloodBossWeapon : UnitWeapons
    {
        public GameObject bulletPool;
        public Material material;

        public bool gameOver;
        public bool paused;

        private bool firing;
        private float firingDelay;

        private bool shooting;
        private float shotDelay;
        private float shotDuration;

        private bool floodSwitch;

        [UsedImplicitly]
        public void Update()
        {
            //TODO: Find a better way to handle this.  Maybe disabling components?
            //var gameOver = GetComponent<Ship>().gameOver;
            //var paused = GetComponent<Ship>().paused;

            if (gameOver || paused)
            {
                return;
            }

            Fire();
        }

        private void Fire()
        {
            if (firing)
            {
                firingDelay = 0;

                if (shooting)
                {
                    shotDelay = 0;

                    if (floodSwitch)
                    {
                        for (float x = -11; x < 12; x++)
                        {
                            var angle = 3.75f * x;
                            var bullet = GetNonActiveBullet(bulletPool);
                            InitiateBullet(bulletPool, bullet, angle);
                        }

                        for (float x = 37; x < 60; x++)
                        {
                            var angle = 3.75f * x;
                            var bullet = GetNonActiveBullet(bulletPool);
                            InitiateBullet(bulletPool, bullet, angle);
                        }
                    }

                    else
                    {
                        for (float x = 13; x < 36; x++)
                        {
                            var angle = 3.75f * x;
                            var bullet = GetNonActiveBullet(bulletPool);
                            InitiateBullet(bulletPool, bullet, angle);
                        }

                        for (float x = 61; x < 84; x++)
                        {
                            var angle = 3.75f * x;
                            var bullet = GetNonActiveBullet(bulletPool);
                            InitiateBullet(bulletPool, bullet, angle);
                        }
                    }

                    shooting = false;
                }
                else
                {
                    shotDelay += Time.deltaTime;
                    shotDuration += Time.deltaTime;
                    if (shotDelay >= .25f)
                    {
                        shooting = true;
                    }

                    if (shotDuration >= 2.5)
                    {
                        firing = false;
                    }
                }
            }
            else
            {
                firingDelay += Time.deltaTime;
                if (firingDelay >= 2.5f)
                {
                    firing = true;
                    shotDuration = 0;
                    floodSwitch = !floodSwitch;
                }
            }
        }

        public void InitiateBullet(GameObject bulletPool, Bullet bullet, float angle)
        {
            bullet.gameObject.transform.GetComponent<Renderer>().material = material;
            bullet.tier = 5;
            bullet.gameObject.layer = 11;
            bullet.isEnemy = true;
            bullet.isBoss = true;
            bullet.isPulse = false;
            bullet.isFlood = true;
            bullet.angle = angle;
            bullet.transform.position = transform.position;
            bullet.gameObject.SetActive(true);
            bullet.transform.SetParent(bulletPool.transform);

            var ps = bullet.transform.GetChild(0).GetComponent<ParticleSystem>().main;
            ps.startColor = material.color;

            bullet.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

            bullet.GetComponent<Bullet>().startLife();

            var currentTransform = bullet.transform;
            currentTransform.RotateAround(bullet.transform.position, -bullet.transform.forward, angle);
        }
    }
}
