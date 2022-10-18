using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public void loadLevel(int scene_id)
    {
        SceneManager.LoadScene(scene_id);
    }
    
}
