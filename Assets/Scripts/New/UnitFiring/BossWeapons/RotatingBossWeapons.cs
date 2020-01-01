using JetBrains.Annotations;

using UnityEngine;

namespace Assets.Scripts.New.UnitFiring.BossWeapons
{
    public class RotatingBossWeapons : UnitFiring
    {
        public GameObject bulletPool;
        public Material material;
        public float angle;
        public float rateOfChangeOfAngles;
        public int layer;

        public bool gameOver;
        public bool paused;

        private bool firing;
        private float firingDelay;

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

                angle %= 360;

                if (paused)
                {
                    return;
                }

                angle += rateOfChangeOfAngles * Time.deltaTime;


                var bullet = GetNonActiveBullet(bulletPool);
                InitiateBullet(bulletPool, bullet, angle);

                firing = false;
            }
            else
            {
                firingDelay += Time.deltaTime;

                if (firingDelay >= .01)
                {
                    firing = true;
                }
            }
        }

        public void InitiateBullet(GameObject bulletPool, Bullet bullet, float angle)
        {
            bullet.gameObject.transform.GetComponent<Renderer>().material = material;
            bullet.tier = 5;
            bullet.gameObject.layer = layer;
            bullet.isEnemy = true;
            bullet.isBoss = true;
            bullet.isPulse = false;
            bullet.isFlood = false;
            bullet.angle = angle;
            bullet.transform.position = transform.position;
            bullet.gameObject.SetActive(true);
            //bullet.transform.SetParent(bulletPool.transform);

            var ps = bullet.transform.GetChild(0).GetComponent<ParticleSystem>().main;
            ps.startColor = material.color;

            bullet.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

            bullet.GetComponent<Bullet>().startLife();

            var currentTransform = bullet.transform;
            currentTransform.RotateAround(bullet.transform.position, -bullet.transform.forward, angle);
        }
    }
}
