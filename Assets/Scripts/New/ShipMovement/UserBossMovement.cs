using JetBrains.Annotations;

using UnityEngine;

namespace Assets.Scripts.New.ShipMovement
{
    public class UserBossMovement : MonoBehaviour
    {
        public float speed;

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
        }
    }
}