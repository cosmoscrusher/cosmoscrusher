using UnityEngine;

namespace Assets.Scripts
{
    public class ParticleKiller : MonoBehaviour
    {
        void Update()
        {
            if (!gameObject.transform.GetComponentInChildren<ParticleSystem>().IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}