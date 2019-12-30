using UnityEngine;

namespace Assets.Scripts.Pilots
{
    public interface IPilot
    {
        void Fire(GameObject ship, GameObject bulletPool);
    }
}