using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    void Awake() {
        Instance = this;
    }

    void Start() {
        UpdateGameState();
        
    }

    void UpdateGameState() {
        //TODO: Add game state listener
        handleGrid();
        handleGun();

    }

    void handleGrid() {
        GridManager.Instance.onStart();
    }

    void handleGun(){
        GunManager.Instance.handleGunPosition();
    }


}