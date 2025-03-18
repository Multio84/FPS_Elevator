using System;
using UnityEngine;


public class PlayerDeath : MonoBehaviour
{
    public Action OnDeath;

    public void Death()
    {
        OnDeath?.Invoke();
    }
}
