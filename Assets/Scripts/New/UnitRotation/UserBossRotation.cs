using JetBrains.Annotations;

using UnityEngine;

namespace Assets.Scripts.New.UnitRotation
{
    public class UserBossRotation : MonoBehaviour
    {
        public GameObject cam;

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

            var mousePosition = Input.mousePosition;
            var cameraComponent = cam.GetComponent<Camera>();
            mousePosition.z = transform.position.z - cam.transform.position.z;
            mousePosition = cameraComponent.ScreenToWorldPoint(mousePosition);
            var angleLine = Vector3.Normalize(mousePosition - transform.position);

            var angle = Vector3.Angle(Vector3.up, angleLine);
            if (angleLine.x < 0)
            {
                angle = -angle;
            }

            currentTransform.GetChild(0).rotation = currentTransform.rotation;
            currentTransform.GetChild(0).RotateAround(currentTransform.transform.position, -currentTransform.transform.forward, angle);
        }
    }
}