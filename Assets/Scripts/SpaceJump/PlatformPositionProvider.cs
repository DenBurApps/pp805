using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceJump
{
    public class PlatformPositionProvider : MonoBehaviour
    {
        [SerializeField] private float _spawnRangeX = 5f;
        [SerializeField] private float _maxSpawnY = 2.5f;
        [SerializeField] private Player _player;

        private float _minSpawnY = 1f;
        private float _highestPlatformY = 0f;

        private void Start()
        {
            _highestPlatformY = _player.transform.position.y - _minSpawnY;
        }

        public Vector2 GetNextPlatformPosition()
        {
            float randomX = Random.Range(-_spawnRangeX, _spawnRangeX);

            Vector2 nextPosition = new Vector2(randomX, _highestPlatformY + _maxSpawnY);

            _highestPlatformY = nextPosition.y;

            return nextPosition;
        }
        
        public void ResetProvider(float startY)
        {
            _highestPlatformY = startY - _minSpawnY;
        }
    }
}
