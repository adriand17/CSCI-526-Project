using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] public Tower t;
    public void Setup(float energy)
    {

    }
   
    // Update is called once per frame
    void Update()
    {
        transform.Find("Container").localScale = new Vector3(t.GetEnergyPercentage(), 1);
    }
}
