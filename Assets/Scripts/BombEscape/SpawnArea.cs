using UnityEngine;

namespace BombEscape
{
    public class SpawnArea : MonoBehaviour
    {
        [SerializeField] private float _minX;
        [SerializeField] private float _maxX;
        [SerializeField] private float _minY;
        [SerializeField] private float _maxY;

        public Vector2 GetPositionToSpawn()
        {
            float randomX = Random.Range(_minX, _maxX);
            float randomY = Random.Range(_minY, _maxY);

            return new Vector2(randomX, randomY);
        }
    }
}
