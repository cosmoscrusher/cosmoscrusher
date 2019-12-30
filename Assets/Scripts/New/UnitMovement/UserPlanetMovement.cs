using JetBrains.Annotations;

using UnityEngine;

namespace Assets.Scripts.New.UnitMovement
{
    public class UserPlanetMovement : MonoBehaviour
    {
        public float speed;

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
                Vector3 leftDirection = Vector3.Normalize(-currentTransform.transform.right) * speed * Time.deltaTime;

                if (Input.GetKey(KeyCode.W))
                {
                    currentTransform.RotateAround(Vector3.zero, leftDirection, -speed * Time.deltaTime);
                }

                if (Input.GetKey(KeyCode.A))
                {
                    currentTransform.RotateAround(Vector3.zero, frontDirection, speed * Time.deltaTime);
                }

                if (Input.GetKey(KeyCode.S))
                {
                    currentTransform.RotateAround(Vector3.zero, leftDirection, speed * Time.deltaTime);
                }

                if (Input.GetKey(KeyCode.D))
                {
                    currentTransform.RotateAround(Vector3.zero, frontDirection, -speed * Time.deltaTime);
                }
            }
        }
    }
}
