using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstrScreen : MonoBehaviour
{    
    public void LoadInstrcuction()
    {
        SceneManager.LoadScene("Level Selector Screen");
        Debug.Log("Loading Game");
    }
}
