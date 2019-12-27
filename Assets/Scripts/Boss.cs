using UnityEngine;

namespace Assets._scripts
{
    public class Boss : MonoBehaviour
    {
        public BossHUD bossHUD;

        public int Health;
        public int shield;

        void Start()
        {
            bossHUD.setBossStats(Health, shield);
        }

        public void takeDamage(int damage)
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