using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting.FullSerializer;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private GameObject _gunObject;
    [SerializeField] private GameObject _camera;
    
    [Header("Level Loading")]
    [Tooltip("The following are used for level loading and wave progression.")]
    [SerializeField]public List<TextAsset> _textJson = new List<TextAsset>();
    [SerializeField] public JSONParser _parser;
    [SerializeField] public GameObject _WinScreenText;
    [SerializeField] private TextMeshProUGUI _WaveText;

    public List<Tile> _spawnTiles;
    public JSONParser.GridLocationsArray _dropLocations;
    public JSONParser.GridLocationsArray _gridLocations;
    public GridManager _gridManager;
    public GunManager _gunManager;


    private int _wave = 0;
    private int _totalWaves;
    public float timeToWin = 10.0f;
    
    void Awake() {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _spawnTiles = new List<Tile>();
        _dropLocations = new JSONParser.GridLocationsArray();
        _dropLocations.indicies = new List<JSONParser.GridLocations>();
        _gridLocations = new JSONParser.GridLocationsArray();
        _gridLocations.indicies = new List<JSONParser.GridLocations>();
        _parser.Parse(_textJson[0]);
        _parser.ParseLevel(_textJson[1]);
        this._totalWaves = _dropLocations.indicies.Count;
        _WaveText.text = _wave.ToString() + "/" + _totalWaves.ToString();
        UpdateGameState();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if all particles are still
        if (_wave == _totalWaves)
        {
            DetermineWinState();
        }
        else
        {
        }
    }

    public void SpawnNextWave()
    {

        if (_wave >= _dropLocations.indicies.Count)
        {
            _wave = 0;
        }
        foreach (int index in _dropLocations.indicies[_wave].locations)
        {
            _gridManager.DrawParticle(BlockType.Water, new Vector3( index, _gridManager.getHeight() -1));
        }
        _wave++;
        _WaveText.text = _wave.ToString() + "/" + _totalWaves.ToString();
    }
    
    void handleGrid() {
        _gridManager.onStart();
    }

    void handleGun(){
        _gunManager.buildGun();
        _gunManager.handleGunPosition();
    }
    
    void UpdateGameState() {
        //TODO: Add game state listener
        handleGrid();
        handleGun();
    }

    public void resetLevel()
    {
        _WinScreenText.SetActive(false);
        _wave = 0;
        _gridManager.ResetGrid();
    }


    void DetermineWinState()
    {
        //check is water is present on map if so then bring up win screen
        if(_gridManager.GetCurrentHealth() > 0 &&  _gridManager.GetWaterCount() == 0)
        {
            _WinScreenText.SetActive(true);
        }
        else if (_gridManager.GetCurrentHealth() > 0 && _gridManager.GetWaterCount() > 0)
        {
            timeToWin -= Time.deltaTime;
            if(timeToWin == 0.0f)
            {
                _WinScreenText.SetActive(true);
            }
        }

        //else start short time after last wave and no water is coming down, it end of timer then win
    }


    public void loadNextScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        switch (currentScene.name)
        {
            case "Block Placing Tutorial":
                Debug.Log("block scene");
                SceneManager.LoadScene("Level 1");
                break;
            case "Shooting Tutorial":
                Debug.Log("shooting scene");
                SceneManager.LoadScene("Block Placing Tutorial");
                break;
            case "Level 1":
                Debug.Log("level 1 scene");
                SceneManager.LoadScene("Level 2");
                break;
            case "Level 2":
                Debug.Log("level 2 scene");
                SceneManager.LoadScene("Main Game Scene");
                break;
            case "Main Game Scene":
                Debug.Log("main scene");
                SceneManager.LoadScene("InstructionOverlay");
                break;
            default:
                SceneManager.LoadScene("InstructionOverlay");
                break;

        }

    }


    public void returnToMainMenu()
    {
        SceneManager.LoadScene("InstructionOverlay");
    }

}



