using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class behaviour : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject Vfx;
    public Vector2 x;
    public Vector2 y;   

    void Start()
    {
        Vfx.SetActive(false);   
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void vfxposition(Vector2 x)
    {
        Vfx.transform.position = new Vector2(x.x,x.y);
        Vfx.SetActive(true);
    }
}
