using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;

    static private int popUpIndex = 0;

    void Update(){
        if (popUpIndex >= popUps.Length)
        {
            return;
        }
        
        if(popUpIndex == 0) {//Tutorial has just started
            popUps[popUpIndex].SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space)){
                popUps[popUpIndex].SetActive(false);
                popUpIndex++;
            }
        } else if(popUpIndex == 1) {
            popUps[popUpIndex].SetActive(true);
            if (Input.GetKeyDown(KeyCode.Mouse0)){
                popUps[popUpIndex].SetActive(false);
                popUpIndex++;
            }
        } else if (popUpIndex == 2)
        {//Tutorial has just started
            popUps[popUpIndex].SetActive(true);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                popUps[popUpIndex].SetActive(false);
                popUpIndex++;
            }
        }
        else if (popUpIndex == 3)
        {//Tutorial has just started
            popUps[popUpIndex].SetActive(true);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                popUps[popUpIndex].SetActive(false);
                popUpIndex++;
            }
        }
        else if (popUpIndex == 4)
        {//Tutorial has just started
            popUps[popUpIndex].SetActive(true);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                popUps[popUpIndex].SetActive(false);
                popUpIndex++;
                SceneManager.LoadScene("MENU");
            }
        }


        
    }
}
