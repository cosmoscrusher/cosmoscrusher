using JetBrains.Annotations;

using UnityEngine;

namespace Assets.Scripts.New.UnitFiring.Tier2PilotFiring
{
    //TODO: Better Name would help
    public class AiTier2PilotFiring : AiPilotFiring
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

                for (var x = 0; x < 12; x++)
                {

                    var bullet = GetNonActiveBullet(bulletPool);
                    var angle = 30 * x;

                    InitiateBullet(bulletPool, bullet, angle);
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
    }
}
