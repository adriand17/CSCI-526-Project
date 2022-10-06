using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    private int popUpIndex;
    
    void update(){
        for (int i = 0; i < popUps.Length; i++){
            if(i == popUpIndex){
                popUps[popUpIndex].SetActive(true);
            }
            else{
                popUps[popUpIndex].SetActive(false);
            }
        }
        if(popUpIndex == 0){//Tutorial has just started
            if(Input.GetKeyDown(KeyCode.Space)){
                popUpIndex++;
            }
        }
        if(popUpIndex == 1){//Tutorial has just started
            if(Input.GetKeyDown(KeyCode.Mouse0)){
                popUpIndex++;
            }
        }
    }
}
