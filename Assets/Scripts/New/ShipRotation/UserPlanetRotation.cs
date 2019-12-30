using JetBrains.Annotations;

using UnityEngine;

namespace Assets.Scripts.New.ShipRotation
{
    public class UserPlanetRotation : MonoBehaviour
    {
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

            var mouse = Input.mousePosition;
            var cam = camera.GetComponent<Camera>();
            mouse.x -= cam.pixelWidth / 2.0f;
            mouse.y -= cam.pixelHeight / 2.0f;
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