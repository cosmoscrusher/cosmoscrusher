using UnityEngine;

namespace Assets.Scripts.Pilots
{
    public interface IPilot
    {
        void MoveShip(GameObject ship);
        void Fire(GameObject ship, GameObject bullet, GameObject bulletPool);
    }
}