using JetBrains.Annotations;

using UnityEngine;

namespace Assets.Scripts.New
{
    public class UserPlanetMovement : MonoBehaviour
    {
        public float speed;
        public GameObject camera;

        [UsedImplicitly]
        public void Update()
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
            var currentTransform = transform;
            if (Input.anyKey)
            {
                Vector3 frontDirection = Vector3.Normalize(currentTransform.transform.up) * speed * Time.deltaTime;
                //(currentTransform.position - frontTransform.position).normalized * speed;
                Vector3 leftDirection = Vector3.Normalize(-currentTransform.transform.right) * speed * Time.deltaTime;
                //(currentTransform.position - leftTransform.position).normalized * speed;

                if (Input.GetKey(KeyCode.W))
                {
                    currentTransform.RotateAround(Vector3.zero, leftDirection, -speed * Time.deltaTime);
                    //currentTransform.position += frontDirection;
                }

                if (Input.GetKey(KeyCode.A))
                {
                    currentTransform.RotateAround(Vector3.zero, frontDirection, speed * Time.deltaTime);
                    //currentTransform.position += leftDirection;
                }

                if (Input.GetKey(KeyCode.S))
                {
                    currentTransform.RotateAround(Vector3.zero, leftDirection, speed * Time.deltaTime);
                    //currentTransform.position += -frontDirection;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    currentTransform.RotateAround(Vector3.zero, frontDirection, -speed * Time.deltaTime);
                    //currentTransform.position += -leftDirection;
                }
            }

            var mouse = Input.mousePosition;
            var cam = camera.GetComponent<Camera>();
            mouse.x -= cam.pixelWidth / 2;
            mouse.y -= cam.pixelHeight / 2;
            mouse = Vector3.Normalize(mouse);
            var angle = Vector3.Angle(Vector3.up, mouse);
            if (mouse.x < 0)
            {
                angle = -angle;
            }

            currentTransform.GetChild(0).rotation = currentTransform.rotation;
            currentTransform.GetChild(0).RotateAround(currentTransform.position, -currentTransform.forward, angle);
        }
    }
}
