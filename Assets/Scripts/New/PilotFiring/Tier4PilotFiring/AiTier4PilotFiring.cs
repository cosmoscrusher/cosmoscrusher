using UnityEngine;

namespace Assets.Scripts.New.PilotFiring.Tier4PilotFiring
{
    //TODO: Better Name would help
    public class AiTier4PilotFiring : AiPilotFiring
    {
        public GameObject bulletPool;

        private bool firing;
        private float firingDelay;
        private int tier4Increment;

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
                var shipMaterial = GetComponent<Ship>().material;

                firingDelay = 0;

                Bullet theBullet = GetNonActiveBullet(bulletPool);
                theBullet.color = GetComponent<Ship>().color;
                theBullet.gameObject.transform.GetComponent<Renderer>().material = shipMaterial;
                theBullet.tier = GetComponent<Ship>().tier;
                theBullet.gameObject.layer = 11;
                theBullet.isEnemy = true;
                theBullet.transform.position = transform.position;
                theBullet.transform.rotation = transform.rotation;
                theBullet.transform.SetParent(bulletPool.transform);

                if (tier4Increment == 12)
                {
                    tier4Increment = 0;
                }

                float angle = 30 * tier4Increment;

                tier4Increment++;

                theBullet.gameObject.SetActive(true);
                theBullet.startLife();
                theBullet.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = shipMaterial.color;
                theBullet.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                Transform bulletTransform = theBullet.transform;
                bulletTransform.RotateAround(theBullet.transform.position, -theBullet.transform.forward, angle);

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

        private Bullet GetNonActiveBullet(GameObject bulletPool)
        {
            for (int i = 0; i < bulletPool.transform.childCount; ++i)
            {
                if (!bulletPool.transform.GetChild(i).gameObject.activeSelf)
                {
                    Bullet bill = bulletPool.transform.GetChild(i).GetComponent<Bullet>();
                    bill.transform.rotation = new Quaternion();
                    return bill;
                }
            }

            Debug.LogError("NOT Enough Bullets");
            return null;
        }
    }
}
