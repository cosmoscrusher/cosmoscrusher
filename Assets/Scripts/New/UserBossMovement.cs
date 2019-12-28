using Assets.Scripts.Pilots;

using JetBrains.Annotations;

using UnityEngine;

namespace Assets.Scripts.New
{
    public class UserBossMovement : MonoBehaviour
    {
        public float speed;
        public GameObject camera;

        [UsedImplicitly]
        void Update()
        {
            //TODO: Find a better way to handle this.  Maybe disabling components?
            var gameOver = GetComponent<Ship>().gameOver;
            var paused = GetComponent<Ship>().paused;

            if (gameOver || paused)
            {
                return;
            }

            MoveShip();
        }

        public void MoveShip()
        {
            var currentTransform = gameObject.transform;
            if (Input.GetKey(KeyCode.W))
            {
                if (currentTransform.position.y < 55.0f)
                {
                    currentTransform.Translate(Vector3.up * speed * Time.deltaTime);
                }
            }

            if (Input.GetKey(KeyCode.A))
            {
                if (currentTransform.position.x > -105.0f)
                {
                    currentTransform.Translate(Vector3.left * speed * Time.deltaTime);
                }
            }

            if (Input.GetKey(KeyCode.S))
            {
                if (currentTransform.position.y > -55.0f)
                {
                    currentTransform.Translate(Vector3.down * speed * Time.deltaTime);
                }
            }

            if (Input.GetKey(KeyCode.D))
            {
                if (currentTransform.position.x < 105.0f)
                {
                    currentTransform.Translate(Vector3.right * speed * Time.deltaTime);
                }
            }

            if (GetComponent<Ship>().tier == 5)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    var playerShip = GetComponent<Ship>();
                    var second = playerShip.material;
                    var secondHit = playerShip.hitMaterial;

                    var pilot = playerShip.pilot as UserPilot;
                    pilot.bulletMaterial = playerShip.secondaryMaterial;
                    
                    playerShip.material = playerShip.secondaryMaterial;
                    playerShip.hitMaterial = playerShip.secondaryHitMaterial;
                    playerShip.secondaryMaterial = second;
                    playerShip.secondaryHitMaterial = secondHit;
                    transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = playerShip.material;
                    playerShip.bossHud.flipColor();
                    if (playerShip.gameObject.layer == 13)
                    {
                        playerShip.gameObject.layer = 12;
                    }
                    else
                    {
                        playerShip.gameObject.layer = 13;
                    }
                }
            }

            var mouse = Input.mousePosition;
            var cam = camera.GetComponent<Camera>();
            mouse.z = transform.position.z - camera.transform.position.z;
            mouse = cam.ScreenToWorldPoint(mouse);
            var angleLine = Vector3.Normalize(mouse - transform.position);
            var angle = Vector3.Angle(Vector3.up, angleLine);
            if (angleLine.x < 0)
            {
                angle = -angle;
            }

            currentTransform.GetChild(0).rotation = currentTransform.rotation;
            currentTransform.GetChild(0).RotateAround(currentTransform.transform.position,
                -currentTransform.transform.forward, angle);
        }
    }
}
