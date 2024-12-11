using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceJump
{
    public class PlatformSpawner : ObjectPool<Platform>
    {
        [SerializeField] private Platform _prefab;
        [SerializeField] private PlatformPositionProvider _platformPositionProvider;
        [SerializeField] private int _poolCapacity;
        
        private List<Platform> _spawnedObjects = new List<Platform>();

        public event Action BombCatched;
        public event Action CrystalCatched;
        
        private void Awake()
        {
            for (int i = 0; i <= _poolCapacity; i++)
            {
                Initalize(_prefab);
            }
        }
        
        public void Spawn()
        {
            if (ActiveObjects.Count >= _poolCapacity)
                return;

            Platform prefabToSpawn = _prefab;

            if (TryGetObject(out Platform @object, prefabToSpawn))
            {
                @object.transform.position = _platformPositionProvider.GetNextPlatformPosition();
                @object.BombStepped += OnBombCatched;
                @object.CrystalStepped += OnCrystalCatched;
                _spawnedObjects.Add(@object);
            }
        }

        public void ReturnToPool(Platform @object)
        {
            if (@object == null)
                return;
            
            @object.BombStepped -= OnBombCatched;
            @object.CrystalStepped -= OnCrystalCatched;
            PutObject(@object);

            if (_spawnedObjects.Contains(@object))
                _spawnedObjects.Remove(@object);
        }

        public void ReturnAllObjectsToPool()
        {
            if (_spawnedObjects.Count <= 0)
                return;

            List<Platform> objectsToReturn = new List<Platform>(_spawnedObjects);
            foreach (var @object in objectsToReturn)
            {
                ReturnToPool(@object);
            }
        }

        private void OnCrystalCatched() => CrystalCatched?.Invoke();
        private void OnBombCatched() => BombCatched?.Invoke();
    }
}