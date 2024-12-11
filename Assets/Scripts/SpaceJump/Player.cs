using System;
using System.Collections;
using System.Collections.Generic;
using SpaceMission;
using UnityEngine;

namespace SpaceJump
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _movingSpeed = 10f;
        [SerializeField] private float _maxXposition = 8f;
        [SerializeField] private float _minXposition = -8f;
        [SerializeField] private float _bounceForce = 10f;
        
        private Transform _transform;
        private Coroutine _followTouchCoroutine;
        private Rigidbody2D _rigidbody2D;
        private Vector2 _startPosition;
        
        private void Awake()
        {
            _transform = transform;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _startPosition = _transform.position;
        }

        private void OnEnable()
        {
            StartFollowingTouch();
        }

        private void OnDisable()
        {
            StopFollowingTouch();
        }

        public void ResetPosition()
        {
            _transform.position = _startPosition;
        }

        public void StartFollowingTouch()
        {
            _rigidbody2D.isKinematic = false;
            
            if (_followTouchCoroutine == null)
            {
                _followTouchCoroutine = StartCoroutine(FollowTouch());
            }
        }

        public void StopFollowingTouch()
        {
            if (_followTouchCoroutine != null)
            {
                StopCoroutine(_followTouchCoroutine);
                _followTouchCoroutine = null;
            }

            _rigidbody2D.isKinematic = true;
            _rigidbody2D.velocity = Vector2.zero;
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Platform platform) || collision.gameObject.TryGetComponent(out StartPlatform startPlatformplatform))
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _bounceForce);
            }
        }
    }
}
