using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Vector2 mousePos;
    [SerializeField] private Camera cam;
    
    void Update() {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }
    
    void FixedUpdate() {
        Vector2 lookdir = mousePos - rigidBody.position;
        float angle = Mathf.Atan2(lookdir.y,lookdir.x)*Mathf.Rad2Deg-90f;
        rigidBody.rotation = angle;
    }
}
