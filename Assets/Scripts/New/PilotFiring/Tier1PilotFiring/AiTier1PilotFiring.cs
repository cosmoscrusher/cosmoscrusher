using UnityEngine;

namespace Assets.Scripts.New.PilotFiring.Tier1PilotFiring
{
    //TODO: Better Name would help
    public class AiTier1PilotFiring : MonoBehaviour
    {
        public GameObject bulletPool;

        private bool firing = false;
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
                var shipMaterial = GetComponent<Ship>().material;

                firingDelay = 0;

                var theBullet = GetNonActiveBullet(bulletPool);
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

                var angle = 0;
                var bulletTransform = theBullet.transform;
                bulletTransform.RotateAround(theBullet.transform.position, -theBullet.transform.forward, angle);

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
