using UnityEngine;

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
                theBullet.gameObject.SetActive(true);
                theBullet.startLife();
                theBullet.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = shipMaterial.color;
                theBullet.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

                Bullet theBullet2 = GetNonActiveBullet(bulletPool);
                theBullet2.color = GetComponent<Ship>().color;
                theBullet2.gameObject.transform.GetComponent<Renderer>().material = shipMaterial;
                theBullet2.tier = GetComponent<Ship>().tier;
                theBullet2.gameObject.layer = 11;
                theBullet2.isEnemy = true;
                theBullet2.transform.position = transform.position;
                theBullet2.transform.rotation = transform.rotation;
                theBullet2.transform.SetParent(bulletPool.transform);
                theBullet2.gameObject.SetActive(true);
                theBullet2.startLife();
                theBullet2.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = shipMaterial.color;
                theBullet2.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

                if (tier5Increment == 12)
                {
                    tier5Increment = 0;
                }

                float angle = 30 * tier5Increment;

                tier5Increment++;

                Transform bulletTransform = theBullet.transform;
                bulletTransform.RotateAround(theBullet.transform.position, -theBullet.transform.forward, angle);

                bulletTransform = theBullet2.transform;
                bulletTransform.RotateAround(theBullet2.transform.position, -theBullet2.transform.forward, -angle);

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
