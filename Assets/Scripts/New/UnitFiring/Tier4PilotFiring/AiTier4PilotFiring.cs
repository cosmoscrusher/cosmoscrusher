using JetBrains.Annotations;

using UnityEngine;

namespace Assets.Scripts.New.UnitFiring.Tier4PilotFiring
{
    //TODO: Better Name would help
    public class AiTier4PilotFiring : AiPilotFiring
    {
        public GameObject bulletPool;

        private bool firing;
        private float firingDelay;
        private int tier4Increment;

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
                var angle = 30 * tier4Increment;

                if (tier4Increment == 12)
                {
                    tier4Increment = 0;
                }

                tier4Increment++;

                InitiateBullet(bulletPool, bullet, angle);

                firing = false;
            }
            else
            {
                firingDelay += Time.deltaTime;

                if (firingDelay >= .1)
                {
                    firing = true;
                }
            }
        }
    }
}
