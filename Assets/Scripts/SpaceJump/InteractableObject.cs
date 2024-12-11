using System;
using System.Collections;
using System.Collections.Generic;
using SpaceJump;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public event Action<InteractableObject> PlayerCatched; 
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            PlayerCatched?.Invoke(this);
        }
    }
}
