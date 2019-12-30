using System.Collections;

using Assets.Scripts.New;
using Assets.Scripts.New.PilotFiring;
using Assets.Scripts.Pilots;

using UnityEngine;

namespace Assets.Scripts
{
    public class Ship : MonoBehaviour
    {
        public int tier;
        public int health;
        public GameObject collidedShip;
        public GameObject bullet;
        public GameObject bulletPool;
        public IPilot pilot;
        public bool fireBullets = true;
        public bool gameOver = false;
        public HUDManager hud;
        public BossHUD bossHud;
        public bool invulnerable = false;
        public Material material;
        public Material hitMaterial;
        public Material secondaryMaterial;
        public Material secondaryHitMaterial;
        public int color = 0;
        public bool bossFight;
        public bool paused = false;

        void Update()
        {
            if (!gameOver)
            {
                if (paused)
                {
                    return;
                }

                if (pilot is UserPilot && Input.GetMouseButton(0))
                {
                    if (fireBullets)
                    {
                        pilot.Fire(gameObject, bulletPool);

                        fireBullets = false;
                        StartCoroutine(Countdown());
                    }
                }

                if (pilot is AIPilot)
                {
                    //pilot.Fire(gameObject, bullet, bulletPool);
                }
            }
        }

        public void HitAnimation(int damage)
        {
            health -= damage;
            if (pilot is UserPilot && bossFight)
            {
                bossHud.lostHealth(damage);
            }

            StartCoroutine(Hit());
        }

        public IEnumerator Hit()
        {
            gameObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = hitMaterial;
            yield return new WaitForSeconds(.1f);
            gameObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = material;
            invulnerable = false;
        }

        public IEnumerator Countdown()
        {
            WaitForSeconds delay = new WaitForSeconds(.125f);
            yield return delay;
            fireBullets = true;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (pilot is UserPilot || pilot is HowToPilot)
            {
                if (collision.gameObject.layer == 9)
                {
                    if (collision.gameObject.GetComponent<Ship>().tier == tier + 1)
                    {
                        collidedShip = collision.gameObject;
                        Destroy(collidedShip.GetComponent<AiPilotMovement>());
                        var aiComponents = collidedShip.GetComponents<AiPilotFiring>();
                        foreach (var aiComponent in aiComponents)
                        {
                            Destroy(aiComponent);
                        }

                        var currentMovement = GetComponent<UserPlanetMovement>();

                        var movement = collidedShip.AddComponent<UserPlanetMovement>();
                        movement.speed = currentMovement.speed;
                        movement.camera = currentMovement.camera;

                        if (hud != null)
                        {
                            hud.enemyDestroyed();
                        }

                        tier++;

                        //put health to full and increase size                    
                    }

                    else
                    {
                        //take away health
                        health -= 1;
                    }
                }
            }

            else
            {
                if (collision.gameObject.layer == 8)
                {
                }
            }
        }

        public bool CollidedWithValidEnemy()
        {
            if (collidedShip != null)
            {
                return true;
            }

            return false;
        }
    }
}