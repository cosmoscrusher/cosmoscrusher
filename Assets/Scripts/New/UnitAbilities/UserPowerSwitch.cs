using Assets.Scripts.Pilots;

using JetBrains.Annotations;

using UnityEngine;

namespace Assets.Scripts.New.UnitAbilities
{
    //TODO: Better Name would help
    public class UserPowerSwitch : MonoBehaviour
    {
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

            ChangePowers();
        }

        private void ChangePowers()
        {
            if (Input.GetMouseButtonDown(1))
            {
                var playerShip = GetComponent<Ship>();

                var oldPrimaryMaterial = playerShip.material;
                var oldPrimaryHitMaterial = playerShip.hitMaterial;

                var newPrimaryMaterial = playerShip.secondaryMaterial;
                var newPrimaryHitMaterial = playerShip.secondaryHitMaterial;

                playerShip.material = newPrimaryMaterial;
                playerShip.hitMaterial = newPrimaryHitMaterial;
                playerShip.secondaryMaterial = oldPrimaryMaterial;
                playerShip.secondaryHitMaterial = oldPrimaryHitMaterial;

                var pilot = playerShip.pilot as UserPilot;
                pilot.bulletMaterial = newPrimaryMaterial;

                transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = newPrimaryMaterial;
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
    }
}
