using System.Collections;

using Assets.Scripts.New.UnitAbilities;
using Assets.Scripts.New.UnitHealth;
using Assets.Scripts.New.UnitMovement;
using Assets.Scripts.New.UnitRotation;
using Assets.Scripts.New.UnitWeapons.BossWeapons;
using Assets.Scripts.Pilots;

using UnityEngine;

namespace Assets.Scripts
{
    public class BossFight : MonoBehaviour
    {
        public SoundManager soundManager;
        public GameObject theBoss;
        public GameObject bullet;

        public Material bulletMaterial;
        public Material greenMaterial;
        public Material blueMaterial;
        public Material playerHitMaterial;
        public Material playerMaterial;
        public Material secondaryMaterial;
        public Material secondaryHitMaterial;
        public Material purpleMaterial;
        public ParticleSystem bossDeath;

        public Ship theShip;
        public BossHUD theHud;
        public GameObject cam;
        public GameObject bulletPool;
        public GameObject gameOverLose;
        public GameObject gameOverWin;
        public GameObject pauseScreen;

        private bool flood = false;
        private float floodDelay = 0.0f;
        private bool floodSwitch = false;
        private float floodDelayTime = 0;
        private Ship playerShip;
        private bool gameOver = false;

        int counter = 0;
        int numBullets = 2000;
        bool paused = false;
        private bool shiftStarted = false;

        void Awake()
        {
            Application.targetFrameRate = 60;
        }

        void Start()
        {
            MakeShip();
            GenerateBullets();
            soundManager.PlayBossBackground();

            StartCoroutine(StartDelay());
        }

        private IEnumerator StartDelay()
        {
            yield return new WaitForSeconds(1.0f);
            AddBoss();
        }

        private void AddBoss()
        {
            AddStraightFiringBoss(45, 15, greenMaterial);
            AddStraightFiringBoss(135, 14, blueMaterial);
            AddStraightFiringBoss(225, 15, greenMaterial);
            AddStraightFiringBoss(315, 14, blueMaterial);

            AddRotatingFiringBoss(0, 55, 14, blueMaterial);
            AddRotatingFiringBoss(90, 55, 15, greenMaterial);
            AddRotatingFiringBoss(180, 55, 14, blueMaterial);
            AddRotatingFiringBoss(270, 55, 15, greenMaterial);
        }

        private void AddStraightFiringBoss(float angle, int layer, Material weaponMaterial)
        {
            var straightFiring1 = theBoss.AddComponent<StraightBossWeapons>();
            straightFiring1.bulletPool = bulletPool;
            straightFiring1.angle = angle;
            straightFiring1.layer = layer;
            straightFiring1.material = weaponMaterial;
        }

        private void AddRotatingFiringBoss(float startAngle, float angleRateOfChange, int layer, Material weaponMaterial)
        {
            var straightFiring1 = theBoss.AddComponent<RotatingBossWeapons>();
            straightFiring1.bulletPool = bulletPool;
            straightFiring1.angle = startAngle;
            straightFiring1.rateOfChangeOfAngles = angleRateOfChange;
            straightFiring1.layer = layer;
            straightFiring1.material = weaponMaterial;
        }

        private void AddBossPulseWeapon()
        {
            var pulseWeapon = theBoss.AddComponent<PulseBossWeapon>();
            pulseWeapon.bulletPool = bulletPool;

            var greenPulseData = new PulseBulletData
            {
                BulletMaterial = greenMaterial,
                BulletLayer = 15,
                BossLayer = 20
            };

            var bluePulseData = new PulseBulletData
            {
                BulletMaterial = blueMaterial,
                BulletLayer = 14,
                BossLayer = 19
            };

            pulseWeapon.pulseBulletData.Add(greenPulseData);
            pulseWeapon.pulseBulletData.Add(bluePulseData);
        }

        private void GenerateBullets()
        {
            //tier count * 20
            //20 * 20
            for (int i = 0; i < numBullets; ++i)
            {
                Bullet theBullet = Object.Instantiate(bullet.GetComponent<Bullet>()) as Bullet;
                theBullet.transform.SetParent(bulletPool.transform);
                theBullet.gameObject.SetActive(false);
            }
        }


        void Update()
        {
            if (gameOver)
            {
                return;
            }

            if (paused)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    unPause();
                }
                else
                {
                    return;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseGame();
            }

            if (!gameOver)
            {
                if (theBoss.GetComponent<BossHealth>().shield > 0)
                {
                    Flood();
                }

                else if (!shiftStarted)
                {
                    StartCoroutine(ChangePhase());
                }
            }

            if (theBoss != null && theBoss.GetComponent<BossHealth>().Health <= 0)
            {
                soundManager.PlayBossDestruction();
                StopAllCoroutines();
                gameOverWin.SetActive(true);
                Destroy(theBoss.gameObject);
                gameOver = true;
                playerShip.gameOver = true;

                var bossStraightFiringComponents = theBoss.GetComponents<StraightBossWeapons>();
                foreach (var bossStraightFiringComponent in bossStraightFiringComponents)
                {
                    bossStraightFiringComponent.gameOver = true;
                }
                var bossRotatingFiringComponents = theBoss.GetComponents<RotatingBossWeapons>();
                foreach (var bossRotatingFiringComponent in bossRotatingFiringComponents)
                {
                    bossRotatingFiringComponent.gameOver = true;
                }

                clearAllBullets();
                bossDeath.Play();
            }

            else if (playerShip != null && playerShip.health <= 0)
            {
                soundManager.PlayShipDestruction();
                StopAllCoroutines();
                gameOverLose.SetActive(true);
                Destroy(playerShip.gameObject);
                gameOver = true;
                var bossStraightFiringComponents = theBoss.GetComponents<StraightBossWeapons>();
                foreach (var bossStraightFiringComponent in bossStraightFiringComponents)
                {
                    bossStraightFiringComponent.gameOver = true;
                }
                var bossRotatingFiringComponents = theBoss.GetComponents<RotatingBossWeapons>();
                foreach (var bossRotatingFiringComponent in bossRotatingFiringComponents)
                {
                    bossRotatingFiringComponent.gameOver = true;
                }
                clearAllBullets();
            }
        }

        private void clearAllBullets()
        {
            foreach (var b in bulletPool.GetComponentsInChildren<Bullet>())
            {
                b.clearBullet();
            }
        }

        private void pauseGame()
        {
            pauseScreen.SetActive(true);
            playerShip.paused = true;
            paused = true;
            var bossStraightFiringComponents = theBoss.GetComponents<StraightBossWeapons>();
            foreach (var bossStraightFiringComponent in bossStraightFiringComponents)
            {
                bossStraightFiringComponent.paused = true;
            }
            var bossRotatingFiringComponents = theBoss.GetComponents<RotatingBossWeapons>();
            foreach (var bossRotatingFiringComponent in bossRotatingFiringComponents)
            {
                bossRotatingFiringComponent.paused = true;
            }
            foreach (Bullet b in bulletPool.GetComponentsInChildren<Bullet>())
            {
                b.paused = true;
            }

            soundManager.PauseSounds();
        }

        private void unPause()
        {
            pauseScreen.SetActive(false);
            playerShip.paused = false;
            var bossStraightFiringComponents = theBoss.GetComponents<StraightBossWeapons>();
            foreach (var bossStraightFiringComponent in bossStraightFiringComponents)
            {
                bossStraightFiringComponent.paused = false;
            }
            var bossRotatingFiringComponents = theBoss.GetComponents<RotatingBossWeapons>();
            foreach (var bossRotatingFiringComponent in bossRotatingFiringComponents)
            {
                bossRotatingFiringComponent.paused = false;
            }
            foreach (Bullet b in bulletPool.GetComponentsInChildren<Bullet>())
            {
                b.paused = false;
            }

            paused = false;
            soundManager.UnPauseSounds();
        }

        private Bullet GetNonActiveBullet()
        {
            int j = 0;
            for (int i = counter; j < bulletPool.transform.childCount; ++i)
            {
                if (!bulletPool.transform.GetChild(i).gameObject.activeSelf)
                {
                    Bullet bill = bulletPool.transform.GetChild(i).GetComponent<Bullet>() as Bullet;
                    bill.transform.rotation = new Quaternion();
                    return bill;
                }

                ++counter;
                if (counter >= numBullets)
                {
                    counter -= numBullets;
                    i -= numBullets;
                }

                ++j;
            }

            Debug.LogError("NOT Enough Bullets");
            return null;
        }

        void Flood()
        {
            if (flood)
            {
                int bulletLayer = 11;

                if (floodDelayTime >= .25f)
                {
                    floodDelayTime = 0;
                    if (floodSwitch)
                    {
                        for (float x = -11; x < 12; x++)
                        {
                            Bullet theBullet = GetNonActiveBullet();
                            theBullet.gameObject.transform.GetComponent<Renderer>().material = bulletMaterial;
                            theBullet.tier = 5;
                            theBullet.gameObject.layer = bulletLayer;
                            theBullet.isEnemy = true;
                            theBullet.isBoss = true;
                            theBullet.isPulse = false;
                            theBullet.isFlood = true;
                            theBullet.angle = 3.75f * x;
                            theBullet.transform.position = theBoss.transform.position;
                            theBullet.gameObject.SetActive(true);

                            var ps = theBullet.transform.GetChild(0).GetComponent<ParticleSystem>().main;
                            ps.startColor = bulletMaterial.color;

                            theBullet.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                            theBullet.GetComponent<Bullet>().startLife();
                            if (paused)
                            {
                                theBullet.paused = true;
                            }

                            Transform currentTransform = theBullet.transform;
                            currentTransform.RotateAround(theBullet.transform.position, -theBullet.transform.forward,
                                theBullet.angle);
                        }

                        for (float x = 37; x < 60; x++)
                        {
                            Bullet theBullet = GetNonActiveBullet();
                            theBullet.gameObject.transform.GetComponent<Renderer>().material = bulletMaterial;
                            theBullet.tier = 5;
                            theBullet.gameObject.layer = bulletLayer;
                            theBullet.isEnemy = true;
                            theBullet.isBoss = true;
                            theBullet.isPulse = false;
                            theBullet.isFlood = true;
                            theBullet.angle = 3.75f * x;
                            theBullet.transform.position = theBoss.transform.position;
                            theBullet.gameObject.SetActive(true);

                            var ps = theBullet.transform.GetChild(0).GetComponent<ParticleSystem>().main;
                            ps.startColor = bulletMaterial.color;

                            theBullet.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                            theBullet.GetComponent<Bullet>().startLife();
                            if (paused)
                            {
                                theBullet.paused = true;
                            }

                            Transform currentTransform = theBullet.transform;
                            currentTransform.RotateAround(theBullet.transform.position, -theBullet.transform.forward,
                                theBullet.angle);
                        }
                    }

                    else
                    {
                        for (float x = 13; x < 36; x++)
                        {
                            Bullet theBullet = GetNonActiveBullet();
                            theBullet.gameObject.transform.GetComponent<Renderer>().material = bulletMaterial;
                            theBullet.tier = 5;
                            theBullet.gameObject.layer = bulletLayer;
                            theBullet.isEnemy = true;
                            theBullet.isBoss = true;
                            theBullet.isPulse = false;
                            theBullet.isFlood = true;
                            theBullet.angle = 3.75f * x;
                            theBullet.transform.position = theBoss.transform.position;
                            theBullet.gameObject.SetActive(true);

                            var ps = theBullet.transform.GetChild(0).GetComponent<ParticleSystem>().main;
                            ps.startColor = bulletMaterial.color;

                            theBullet.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                            theBullet.GetComponent<Bullet>().startLife();
                            if (paused)
                            {
                                theBullet.paused = true;
                            }

                            Transform currentTransform = theBullet.transform;
                            currentTransform.RotateAround(theBullet.transform.position, -theBullet.transform.forward,
                                theBullet.angle);
                        }

                        for (float x = 61; x < 84; x++)
                        {
                            Bullet theBullet = GetNonActiveBullet();
                            theBullet.gameObject.transform.GetComponent<Renderer>().material = bulletMaterial;
                            theBullet.tier = 5;
                            theBullet.gameObject.layer = bulletLayer;
                            theBullet.isEnemy = true;
                            theBullet.isBoss = true;
                            theBullet.isPulse = false;
                            theBullet.isFlood = true;
                            theBullet.angle = 3.75f * x;
                            theBullet.transform.position = theBoss.transform.position;
                            theBullet.gameObject.SetActive(true);

                            var ps = theBullet.transform.GetChild(0).GetComponent<ParticleSystem>().main;
                            ps.startColor = bulletMaterial.color;

                            theBullet.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                            theBullet.GetComponent<Bullet>().startLife();
                            if (paused)
                            {
                                theBullet.paused = true;
                            }

                            Transform currentTransform = theBullet.transform;
                            currentTransform.RotateAround(theBullet.transform.position, -theBullet.transform.forward,
                                theBullet.angle);
                        }
                    }
                }

                else
                {
                    floodDelayTime += Time.deltaTime;
                }

                if (floodDelay < 2.5f)
                {
                    floodDelay += Time.deltaTime;
                }

                else
                {
                    flood = false;
                    floodDelay = 0;
                }
            }

            else
            {
                if (floodDelay < 2.5f)
                {
                    floodDelay += Time.deltaTime;
                }
                else
                {
                    flood = true;
                    floodDelay = 0.0f;
                    floodSwitch = !floodSwitch;
                }
            }
        }

        public void MakeShip()
        {
            var position = new Vector3(0, -50.6f, 94.1f);
            var ship = Instantiate(theShip) as Ship;
            ship.transform.position = position;
            ship.gameObject.layer = 12;
            ship.health = 5;
            ship.bossHud = theHud;
            ship.bossFight = true;
            ship.bullet = bullet;
            ship.tier = 5;
            ship.pilot = new UserPilot(30, cam, playerMaterial, true, soundManager);
            ship.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = playerMaterial;
            ship.bullet = bullet;
            ship.material = playerMaterial;
            ship.hitMaterial = playerHitMaterial;
            ship.GetComponent<Ship>().bulletPool = bulletPool;
            ship.secondaryMaterial = secondaryMaterial;
            ship.secondaryHitMaterial = secondaryHitMaterial;

            var movement = ship.gameObject.AddComponent<UserBossMovement>();
            movement.speed = 30;
            
            var rotation = ship.gameObject.AddComponent<UserBossRotation>();
            rotation.camera = cam;

            ship.gameObject.AddComponent<UserPowerSwitch>();

            playerShip = ship;
        }

        private IEnumerator ChangePhase()
        {
            shiftStarted = true;

            theBoss.gameObject.layer = 21;
            theBoss.gameObject.transform.GetComponent<Renderer>().material = purpleMaterial;

            var bossRotatingFiringComponents = theBoss.GetComponents<RotatingBossWeapons>();
            foreach (var bossRotatingFiringComponent in bossRotatingFiringComponents)
            {
                Destroy(bossRotatingFiringComponent);
            }

            yield return new WaitForSeconds(1);

            AddStraightFiringBoss(0, 11, bulletMaterial);
            AddStraightFiringBoss(90, 11, bulletMaterial);
            AddStraightFiringBoss(180, 11, bulletMaterial);
            AddStraightFiringBoss(270, 11, bulletMaterial);

            yield return new WaitForSeconds(1);

            var bossStraightWeaponComponents = theBoss.GetComponents<StraightBossWeapons>();
            foreach (var bossStraightWeaponComponent in bossStraightWeaponComponents)
            {
                if (bossStraightWeaponComponent.material == bulletMaterial)
                {
                    Destroy(bossStraightWeaponComponent);
                }
            }

            AddRotatingFiringBoss(0, 55, 11, bulletMaterial);
            AddRotatingFiringBoss(90, 55, 11, bulletMaterial);
            AddRotatingFiringBoss(180, 55, 11, bulletMaterial);
            AddRotatingFiringBoss(270, 55, 11, bulletMaterial);

            AddBossPulseWeapon();
        }
    }
}