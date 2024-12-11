using System;
using System.Collections;
using UnityEngine;

namespace BombEscape
{
    [RequireComponent(typeof(Collider2D))]
    public class ClickableObject : MonoBehaviour
    {
        [SerializeField] private int _disableInterval = 1;

        public event Action<ClickableObject> ReadyToDisable;

        public void StartCoroutine()
        {
            StartCoroutine(StartCountdown());
        }

        public void StopCoroutine()
        {
            StopCoroutine(StartCountdown());
        }

        private IEnumerator StartCountdown()
        {
            yield return new WaitForSeconds(_disableInterval);
            
            ReadyToDisable?.Invoke(this);
        }
    }
}