using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTrigger2DUtil : MonoBehaviour
{
    public string targetTag = "Player";
    public UnityEvent OnTriggerEnterEvent, OnTriggerExitEvent;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(targetTag))
        {
            OnTriggerEnterEvent?.Invoke();
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(targetTag))
        {
            OnTriggerExitEvent?.Invoke();
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
