using UnityEngine;

namespace Assets.Scripts.New
{
    public class AiPilotMovement : MonoBehaviour
    {
        public float secondsToChange = 2.0f;
        public float speed;
        
        private float secondsPassed;
        private int randomDirection;

        void Start()
        {
        }

        void LateUpdate()
        {
        }

        void Update()
        {
            //if (!gameOver)
            //{
            //    if (paused)
            //    {
            //        return;
            //    }

            MoveShip();
            //}
        }


        public void MoveShip()
        {
            var theTransform = gameObject.transform;
            if (secondsPassed >= secondsToChange)
            {
                secondsPassed = 0;
                randomDirection = Random.Range(0, 8);
            }
            else
            {
                secondsPassed += Time.deltaTime;
            }

            var frontDirection = Vector3.Normalize(theTransform.transform.up) * speed * Time.deltaTime;
            var leftDirection = Vector3.Normalize(-theTransform.transform.right) * speed * Time.deltaTime;
            switch (randomDirection)
            {
                case 0:
                    theTransform.RotateAround(Vector3.zero, leftDirection, -speed * Time.deltaTime);
                    break;
                case 1:
                    theTransform.RotateAround(Vector3.zero, frontDirection, speed * Time.deltaTime);
                    break;
                case 2:
                    theTransform.RotateAround(Vector3.zero, frontDirection, -speed * Time.deltaTime);
                    break;
                case 3:
                    theTransform.RotateAround(Vector3.zero, leftDirection, speed * Time.deltaTime);
                    break;
                case 4:
                    theTransform.RotateAround(Vector3.zero,
                        (leftDirection.normalized + frontDirection.normalized) * speed * Time.deltaTime,
                        speed * Time.deltaTime);
                    break;
                case 5:
                    theTransform.RotateAround(Vector3.zero,
                        (leftDirection.normalized - frontDirection.normalized) * speed * Time.deltaTime,
                        speed * Time.deltaTime);
                    break;
                case 6:
                    theTransform.RotateAround(Vector3.zero,
                        -(leftDirection.normalized + frontDirection.normalized) * speed * Time.deltaTime,
                        speed * Time.deltaTime);
                    break;
                case 7:
                    theTransform.RotateAround(Vector3.zero,
                        -(leftDirection.normalized - frontDirection.normalized) * speed * Time.deltaTime,
                        speed * Time.deltaTime);
                    break;
                default:
                    Debug.LogError("The random number generator did not produce a number from 0 - 7");
                    break;
            }
        }
    }
}
