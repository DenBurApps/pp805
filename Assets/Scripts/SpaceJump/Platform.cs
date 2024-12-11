using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceJump
{
    public class Platform : MonoBehaviour
    {
        [SerializeField] private InteractableObject[] _interactableObjects;

        public event Action BombStepped;
        public event Action CrystalStepped;

        private void Start()
        {
            foreach (var obj in _interactableObjects)
            {
                if (obj is Bomb)
                {
                    obj.PlayerCatched += OnBombStepped;
                }
                else if (obj is Crystal)
                {
                    obj.PlayerCatched += OnCrystalStepped;
                }
            }
        }

        private void OnEnable()
        {
            if (_interactableObjects.Length > 0)
            {
                var randomObj = Random.Range(0, _interactableObjects.Length);
                _interactableObjects[randomObj].gameObject.SetActive(true);
            }
        }

        private void OnDisable()
        {
            if (_interactableObjects.Length > 0)
            {
                foreach (var obj in _interactableObjects)
                {
                    if (obj is Bomb)
                    {
                        obj.PlayerCatched -= OnBombStepped;
                    }
                    else if (obj is Crystal)
                    {
                        obj.PlayerCatched -= OnCrystalStepped;
                    }
                    
                    obj.gameObject.SetActive(false);
                }
            }
        }

        private void OnBombStepped(InteractableObject obj) 
        {
            obj.gameObject.SetActive(false);
            BombStepped?.Invoke();
        } 
            
        private void OnCrystalStepped(InteractableObject obj)
        {
            obj.gameObject.SetActive(false);
            CrystalStepped?.Invoke();
        }
    }
}