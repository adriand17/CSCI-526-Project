using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;

    static private int popUpIndex = 0;


    //[SerializeField] GameObject nextWaveButton;

    //[SerializeField] GameObject dirtButton;

    //[SerializeField] GameObject glassButton;

    //[SerializeField] GameObject mirrorButton;

    private static bool showTutorial = true;

    void Update(){
        Debug.Log("Tutorial Manager was called!" + popUpIndex + " length-  " + popUps.Length);
        if (!showTutorial) return;
        for (int i = 0; i < popUps.Length; i++){
            popUps[popUpIndex].SetActive(i == popUpIndex);
        }
        Time.timeScale = 0f;
        if(popUpIndex == 0){//Tutorial has just started
            Debug.Log("in here - !" + popUpIndex);
            if (Input.GetKeyDown(KeyCode.Space)){
                popUpIndex++;
            }
        }
        if(popUpIndex == 1){//Tutorial has just started
            if(Input.GetKeyDown(KeyCode.Mouse0)){
                popUpIndex++;
            }
        }

        if (popUpIndex == 2)
        {//Tutorial has just started
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                popUpIndex++;
            }
        }


        // add logic if on final index -> reset the
        if (popUpIndex >= popUps.Length) {
            showTutorial = false;
        }
    }
}
