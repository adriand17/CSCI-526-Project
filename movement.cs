using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float movespeed =5f;
    public Rigidbody2D rb;
    Vector2 mousePos;
    public Camera cam;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        mousePos=cam.ScreenToWorldPoint(Input.mousePosition);


    }
    void FixedUpdate()
    {
        Vector2 lookdir = mousePos-rb.position;
        float angle =Mathf.Atan2(lookdir.y,lookdir.x)*Mathf.Rad2Deg-90f;
        rb.rotation=angle;



    }
}
