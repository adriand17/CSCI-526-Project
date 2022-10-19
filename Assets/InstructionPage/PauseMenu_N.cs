using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu_N : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    
     //Update is called once per frame
    void Update()
    {
        Debug.Log("Inside UPDATE");
        if(Input.GetKeyDown(KeyCode.P))
//      if(Input.GetKeyDown("escape"))
        {
            Debug.Log("PRESSED P");
            if(GameIsPaused){
                Debug.Log("InsIde CALL TO RESUME");
                Resume_N();
            }
            else{
                Pause_N();
            }
        }
    }
    public void Resume_N(){
        Debug.Log("InsIde RESUME");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    void Pause_N(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void LoadMenu_N()
    {
        GameIsPaused = false;
        Time.timeScale=1f;
        SceneManager.LoadScene("Title Screen 1");
        Debug.Log("Loading Game");
    }
    public void QuitGame_N()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
