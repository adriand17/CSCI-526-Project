using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //[SerializeField] private Rigidbody2D rigidBody;
    private Vector2 mousePos;
    
    void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    
    void FixedUpdate() {
        // Rotate towards mouse position.
        Rigidbody2D rigidBody = gameObject.GetComponent<Rigidbody2D>();
        Vector2 lookdir = mousePos - rigidBody.position;
        float angle = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg - 90f;
        rigidBody.rotation = angle;
    }
}
