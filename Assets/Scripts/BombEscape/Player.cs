using System;
using System.Collections;
using UnityEngine;

namespace BombEscape
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerMask;

        public event Action<ClickableObject> CrystalCatched;
        public event Action<ClickableObject> BombCatched;

        private Coroutine _touchDetectingCoroutine;

        public void StartDetectingTouch()
        {
            if (_touchDetectingCoroutine == null)
            {
                _touchDetectingCoroutine = StartCoroutine(DetectTouch());
            }
        }

        public void StopDetectingTouch()
        {
            if (_touchDetectingCoroutine != null)
            {
                StopCoroutine(DetectTouch());
                _touchDetectingCoroutine = null;
            }
        }

        private IEnumerator DetectTouch()
        {
            while (true)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    
                    RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity, _layerMask);
                    
                    if (hit.collider != null)
                    {
                        if (hit.collider.TryGetComponent(out ClickableObject obj))
                        {
                            if (obj is Bomb)
                            {
                                BombCatched?.Invoke(obj);
                                Debug.Log("bomb");
                            }
                            else if (obj is Crystal)
                            {
                                CrystalCatched?.Invoke(obj);
                                Debug.Log("crystal");
                            }
                        }
                    }
                }

                yield return null;
            }
        }
    }
}