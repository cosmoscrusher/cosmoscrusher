using UnityEngine;

namespace Assets.Scripts.New.PilotFiring
{
    public class AiPilotFiring : MonoBehaviour
    {
        public Bullet GetNonActiveBullet(GameObject bulletPool)
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

        public void InitiateBullet(GameObject bulletPool, Bullet bullet, int angle)
        {
            var shipMaterial = GetComponent<Ship>().material;

            bullet.color = GetComponent<Ship>().color;
            bullet.gameObject.transform.GetComponent<Renderer>().material = shipMaterial;
            bullet.tier = GetComponent<Ship>().tier;
            bullet.gameObject.layer = 11;
            bullet.isEnemy = true;

            bullet.gameObject.SetActive(true);
            bullet.startLife();

            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.transform.SetParent(bulletPool.transform);

            var ps = bullet.transform.GetChild(0).GetComponent<ParticleSystem>().main;
            ps.startColor = shipMaterial.color;
            bullet.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

            bullet.transform.RotateAround(bullet.transform.position, -bullet.transform.forward, angle);
        }
    }
}
