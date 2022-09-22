using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{

    private Text _bulidingText;  
    private float timeToAppear = 2f;
    private float timeWhenDisappear;
    private GridManager gridManager;

    void Start()
    {
        
    }
    public void Init(bool isEnabled, GridManager gridManager)
    {
        this.gridManager = gridManager;
    }

    // Update is called once per frame
    void Update()
    {
        if (_bulidingText.enabled && (Time.time >= timeWhenDisappear))
        {
            _bulidingText.enabled = false;
        }
    }
}
