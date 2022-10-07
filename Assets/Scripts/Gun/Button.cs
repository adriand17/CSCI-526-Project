using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public UnityEvent click;
    
    private void Awake() {
        if (click == null) {
            click= new UnityEvent();
        }
    }
    
    private void OnMouseDown() {
        if (Time.timeScale == 0f) return;
         click.Invoke();    
    }
}
