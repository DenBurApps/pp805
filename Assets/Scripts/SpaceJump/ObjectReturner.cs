using System;
using UnityEngine;

namespace SpaceJump
{
    public class ObjectReturner : MonoBehaviour
    {
        [SerializeField] private PlatformSpawner _platformSpawner;
        
        public event Action PlayerCatched;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player player))
            {
                PlayerCatched?.Invoke();
            }
            else if(other.TryGetComponent(out Platform platform))
            {
                _platformSpawner.ReturnToPool(platform);
            }
        }
    }
}
