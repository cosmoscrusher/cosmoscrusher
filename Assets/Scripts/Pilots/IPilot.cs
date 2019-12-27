using UnityEngine;

namespace Assets._scripts.Pilots
{
    public interface IPilot
    {
        void MoveShip(GameObject ship);
        void Fire(GameObject ship, GameObject bullet, GameObject bulletPool);
    }
}