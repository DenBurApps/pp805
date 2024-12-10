using SpaceMission;
using UnityEngine;

namespace SpaceMission
{
    public class PoolReturner : MonoBehaviour
    {
        [SerializeField] private InteractableObjectSpawner _spawner;
    
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out MovingObject @object))
            {
                _spawner.ReturnToPool(@object);
            }
        }
    }
}
