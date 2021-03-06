﻿using JetBrains.Annotations;

using UnityEngine;

namespace Assets.Scripts.New.UnitHealth
{
    public class BossHealth : MonoBehaviour
    {
        public BossHUD bossHUD;

        public int health;

        [UsedImplicitly]
        public void Start()
        {
            bossHUD.setBossHealthStats(health);
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            health = Mathf.Clamp(health, 0, int.MaxValue);

            bossHUD.hitEnemy(damage);
        }
    }
}
