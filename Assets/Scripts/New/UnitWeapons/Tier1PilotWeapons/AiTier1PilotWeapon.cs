﻿using JetBrains.Annotations;

using UnityEngine;

namespace Assets.Scripts.New.UnitWeapons.Tier1PilotWeapons
{
    //TODO: Better Name would help
    public class AiTier1PilotWeapon : AiPilotWeapon
    {
        public GameObject bulletPool;

        private bool firing;
        private float firingDelay;

        [UsedImplicitly]
        public void Update()
        {
            //TODO: Find a better way to handle this.  Maybe disabling components?
            var gameOver = GetComponent<Ship>().gameOver;
            var paused = GetComponent<Ship>().paused;

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

                var bullet = GetNonActiveBullet(bulletPool);
                var angle = 0;

                InitiateBullet(bulletPool, bullet, angle);

                firing = false;
            }

            else
            {
                firingDelay += Time.deltaTime;

                if (firingDelay >= 0.25)
                {
                    firing = true;
                }
            }
        }
    }
}
