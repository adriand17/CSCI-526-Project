using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstrScreen : MonoBehaviour
{    
    public void LoadInstrcuction()
    {
        SceneManager.LoadScene("Level Selection");
        Debug.Log("Loading Game");
    }
}
