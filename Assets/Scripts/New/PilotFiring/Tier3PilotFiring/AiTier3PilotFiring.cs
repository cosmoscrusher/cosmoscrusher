﻿using UnityEngine;

namespace Assets.Scripts.New.PilotFiring.Tier3PilotFiring
{
    //TODO: Better Name would help
    public class AiTier3PilotFiring : AiPilotFiring
    {
        public GameObject bulletPool;

        private bool firing;
        private float firingDelay;

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

                for (var x = -1; x < 2; x++)
                {

                    var bullet = GetNonActiveBullet(bulletPool);
                    var angle = 120 * x;

                    InitiateBullet(bulletPool, bullet, angle);
                }

                firing = false;
            }

            else
            {
                firingDelay += Time.deltaTime;

                if (firingDelay >= .25)
                {
                    firing = true;
                }
            }
        }
    }
}
