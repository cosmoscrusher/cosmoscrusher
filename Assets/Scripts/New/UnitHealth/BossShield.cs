using JetBrains.Annotations;

using UnityEngine;

namespace Assets.Scripts.New.UnitHealth
{
    public class BossShield : MonoBehaviour
    {
        public BossHUD bossHUD;

        public int shield;

        [UsedImplicitly]
        public void Start()
        {
            bossHUD.setBossShieldStats(shield);
        }

        public void TakeDamage(int damage)
        {
            shield -= damage;
            shield = Mathf.Clamp(shield, 0, int.MaxValue);

            bossHUD.hitEnemy(damage);
        }
    }
}
