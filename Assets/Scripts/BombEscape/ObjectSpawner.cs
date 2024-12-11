using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BombEscape
{
    public class ObjectSpawner : ObjectPool<ClickableObject>
    {
        [SerializeField] private ClickableObject[] _prefabs;
        [SerializeField] private SpawnArea _spawnArea;
        [SerializeField] private int _poolCapacity;

        private List<ClickableObject> _clickableObjects = new List<ClickableObject>();

        private void Awake()
        {
            for (int i = 0; i <= _poolCapacity; i++)
            {
                ShuffleArray();

                foreach (var prefab in _prefabs)
                {
                    Initalize(prefab);
                }
            }
        }
        
        public void Spawn()
        {
            if (ActiveObjects.Count >= _poolCapacity)
                return;

            int randomIndex = Random.Range(0, _prefabs.Length);
            ClickableObject prefabToSpawn = _prefabs[randomIndex];

            if (TryGetObject(out ClickableObject @object, prefabToSpawn))
            {
                @object.transform.position = _spawnArea.GetPositionToSpawn();
                @object.ReadyToDisable += ReturnToPool;
                @object.StartCoroutine();
                _clickableObjects.Add(@object);
            }
        }
        
        public void ReturnToPool(ClickableObject @object)
        {
            if (@object == null)
                return;
            
            @object.ReadyToDisable -= ReturnToPool;
            @object.StopCoroutine();
            PutObject(@object);

            if (_clickableObjects.Contains(@object))
                _clickableObjects.Remove(@object);
        }

        public void ReturnAllObjectsToPool()
        {
            if (_clickableObjects.Count <= 0)
                return;

            List<ClickableObject> objectsToReturn = new List<ClickableObject>(_clickableObjects);
            foreach (var @object in objectsToReturn)
            {
                ReturnToPool(@object);
            }
        }

        private void ShuffleArray()
        {
            for (int i = 0; i < _prefabs.Length - 1; i++)
            {
                ClickableObject temp = _prefabs[i];
                int randomIndex = Random.Range(0, _prefabs.Length);
                _prefabs[i] = _prefabs[randomIndex];
                _prefabs[randomIndex] = temp;
            }
        }
    }
}
