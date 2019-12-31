using UnityEngine;

namespace Assets.Scripts.New.UnitFiring
{
    public class UnitFiring : MonoBehaviour
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
    }
}
