using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Despawner : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    { 
        if (GetComponentInChildren<Renderer>().isVisible)
        {
            //Visible code here
        }
        else
        {
            DestroyImmediate(gameObject);    
        }
    }
}
