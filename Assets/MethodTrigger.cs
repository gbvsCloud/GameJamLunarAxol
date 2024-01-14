using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MethodTrigger : MonoBehaviour
{
    public UnityEvent evento;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
            evento.Invoke();
    }


}

