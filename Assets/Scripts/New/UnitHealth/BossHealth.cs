using UnityEngine;

namespace Assets.Scripts.New.UnitHealth
{
    public class BossHealth : MonoBehaviour
    {
        public BossHUD bossHUD;

        public int Health;
        public int shield;

        void Start()
        {
            bossHUD.setBossStats(Health, shield);
        }

        public void TakeDamage(int damage)
        {
            if (shield > 0)
            {
                shield -= damage;
                shield = Mathf.Clamp(shield, 0, int.MaxValue);
            }
            else
            {
                Health -= damage;
            }

            bossHUD.hitEnemy(damage);
        }
    }
}
