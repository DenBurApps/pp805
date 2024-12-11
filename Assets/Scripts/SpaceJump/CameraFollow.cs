using System;
using UnityEngine;

namespace SpaceJump
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float offsetY = 0f;
        [SerializeField] private float smoothSpeed = 0.125f;

        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        private void LateUpdate()
        {
            if (player != null)
            {
                Vector3 targetPosition = new Vector3(
                    _transform.position.x, 
                    player.position.y + offsetY, 
                    _transform.position.z
                );
                
                _transform.position = Vector3.Lerp(_transform.position, targetPosition, smoothSpeed);
            }
        }
    }
}