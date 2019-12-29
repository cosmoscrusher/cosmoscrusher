﻿using UnityEngine;

namespace Assets.Scripts.New.PilotFiring.Tier5PilotFiring
{
    //TODO: Better Name would help
    public class AiTier5PilotFiring : AiPilotFiring
    {
        public GameObject bulletPool;

        private bool firing;
        private float firingDelay;
        private int tier5Increment;

        void Update()
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

                var angle = 30 * tier5Increment;

                var bullet1 = GetNonActiveBullet(bulletPool);
                InitiateBullet(bulletPool, bullet1, angle);

                var bullet2 = GetNonActiveBullet(bulletPool);
                InitiateBullet(bulletPool, bullet2, -angle);

                if (tier5Increment == 12)
                {
                    tier5Increment = 0;
                }

                tier5Increment++;

                firing = false;
            }

            else
            {
                firingDelay += Time.deltaTime;

                if (firingDelay >= .08)
                {
                    firing = true;
                }
            }
        }
    }
}
