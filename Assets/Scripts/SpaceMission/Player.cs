using System;
using System.Collections;
using UnityEngine;

namespace SpaceMission
{
    [RequireComponent(typeof(Collider2D))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _movingSpeed = 10f;
        [SerializeField] private float _maxXposition = 8f;
        [SerializeField] private float _minXposition = -8f;

        private Transform _transform;
        private Coroutine _followTouchCoroutine;

        public event Action<MovingObject> BombCatched;
        public event Action<MovingObject> CrystalCatched;

        private void Awake()
        {
            _transform = transform;
        }

        private void OnEnable()
        {
            StartFollowingTouch();
        }

        private void OnDisable()
        {
            StopFollowingTouch();
        }

        private void StartFollowingTouch()
        {
            if (_followTouchCoroutine == null)
            {
                _followTouchCoroutine = StartCoroutine(FollowTouch());
            }
        }

        private void StopFollowingTouch()
        {
            if (_followTouchCoroutine != null)
            {
                StopCoroutine(_followTouchCoroutine);
                _followTouchCoroutine = null;
            }
        }

        private IEnumerator FollowTouch()
        {
            while (true)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    {
                        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                        
                        float targetX = Mathf.Clamp(touchPosition.x, _minXposition, _maxXposition);
                        
                        Vector3 newPosition = _transform.position;
                        newPosition.x = Mathf.Lerp(newPosition.x, targetX, _movingSpeed * Time.deltaTime);

                        _transform.position = newPosition;
                    }
                }

                yield return null;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IIntractable interactable))
            {
                if (interactable is Bomb)
                {
                    BombCatched?.Invoke((MovingObject)interactable);
                }
                else if (interactable is Crystal)
                {
                    CrystalCatched?.Invoke((MovingObject)interactable);
                }
            }
        }
    }
}
