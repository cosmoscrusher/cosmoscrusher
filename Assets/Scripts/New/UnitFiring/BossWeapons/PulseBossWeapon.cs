using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts.New.UnitFiring.BossWeapons
{
    public class PulseBossWeapon : UnitFiring
    {
        public GameObject bulletPool;
        public List<PulseBulletData> pulseBulletData = new List<PulseBulletData>();

        public bool gameOver;
        public bool paused;

        private bool firing;
        private float firingDelay;

        void Update()
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

                var pulseType = Random.Range(0, pulseBulletData.Count);

                var pulseBullet = this.pulseBulletData[pulseType];

                var bulletMaterial = pulseBullet.BulletMaterial;
                var bulletLayer = pulseBullet.BulletLayer;
                var bossLayer = pulseBullet.BossLayer;

                for (float x = 0; x < 96; x++)
                {
                    var bullet = GetNonActiveBullet(bulletPool);
                    bullet.gameObject.transform.GetComponent<Renderer>().material = bulletMaterial;
                    bullet.tier = 5;
                    bullet.gameObject.layer = bulletLayer;
                    bullet.isEnemy = true;
                    bullet.isBoss = true;
                    bullet.isPulse = true;
                    bullet.isFlood = false;
                    bullet.angle = 3.75f * x;
                    bullet.transform.position = transform.position;
                    bullet.gameObject.SetActive(true);

                    var ps = bullet.transform.GetChild(0).GetComponent<ParticleSystem>().main;
                    ps.startColor = bulletMaterial.color;

                    bullet.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                    gameObject.GetComponent<Renderer>().material = bulletMaterial;
                    gameObject.layer = bossLayer;
                    bullet.GetComponent<Bullet>().startLife();

                    var currentTransform = bullet.transform;
                    currentTransform.RotateAround(bullet.transform.position, -bullet.transform.forward, bullet.angle);
                }

                firing = false;
            }
            else
            {
                firingDelay += Time.deltaTime;
                if (firingDelay >= 2.5f)
                {
                    firing = true;
                }
            }
        }

        //public void InitiateBullet(GameObject bulletPool, Bullet bullet, float angle)
        //{
        //    bullet.gameObject.transform.GetComponent<Renderer>().material = material;
        //    bullet.tier = 5;
        //    bullet.gameObject.layer = layer;
        //    bullet.isEnemy = true;
        //    bullet.isBoss = true;
        //    bullet.isPulse = false;
        //    bullet.isFlood = false;
        //    bullet.angle = angle;
        //    bullet.transform.position = transform.position;
        //    bullet.gameObject.SetActive(true);
        //    //bullet.transform.SetParent(bulletPool.transform);

        //    var ps = bullet.transform.GetChild(0).GetComponent<ParticleSystem>().main;
        //    ps.startColor = material.color;

        //    bullet.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        //    bullet.GetComponent<Bullet>().startLife();

        //    var currentTransform = bullet.transform;
        //    currentTransform.RotateAround(bullet.transform.position, -bullet.transform.forward, angle);
        //}
    }

    public class PulseBulletData
    {
        public Material BulletMaterial { get; set; }
        public int BulletLayer { get; set; }
        public int BossLayer { get; set; }
    }
}