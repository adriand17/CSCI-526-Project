using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    [Header("Gun Initializations")]
    [SerializeField] private GameObject _gunObject;
    [SerializeField] private GameObject _camera;
    
    [Header("Level Loading")]
    [Tooltip("The following are used for level loading and wave progression.")]
    [SerializeField]public List<TextAsset> _textJson = new List<TextAsset>();
    [SerializeField] public JSONParser _parser;
    [SerializeField] public GameObject _WinScreenText;
    private List<Tile> _spawnTiles;
    public JSONParser.GridLocationsArray _dropLocations;
    public JSONParser.GridLocationsArray _gridLocations;
    public JSONParser.WaveArray _wavesArray;
    public JSONParser.Wave _waveLocations;
 
    
    [Header("Reference Managers")]
    public GridManager _gridManager;
    public GunManager _gunManager;
    public TextMeshProUGUI _WaveText;


    [Header("Block Selection Buttons")] [SerializeField]
    public List<GameObject> _blockSelectionButtons;
    private int _wave = 0; //current wave index
    private int _totalWaves; //total amount of waves
    private int _subWave = 0; //current subwave index
    private int _totalSubWaves; //total amount of subwaves in a wave
    public bool waveSpawning = false;
    public float subwaveTimerMax = 2f; //time between subwaves
    public float subwaveTimer;

    void Awake() {
        _instance = this;
        foreach (GameObject button in _blockSelectionButtons)
        {
            button.GetComponent<ButtonScript>().InitializeButton();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _spawnTiles = new List<Tile>();
        _wavesArray = new JSONParser.WaveArray();
        _wavesArray.waves = new List<JSONParser.Wave>();
        _dropLocations = new JSONParser.GridLocationsArray();
        _dropLocations.indicies = new List<JSONParser.GridLocations>();
        _gridLocations = new JSONParser.GridLocationsArray();
        _gridLocations.indicies = new List<JSONParser.GridLocations>();
        _parser.Parse(_textJson[0]);
        _parser.ParseLevel(_textJson[1]);
        _parser.ParseWaves(_textJson[2]);

        _waveLocations = _wavesArray.waves[0]; //set to first wave
        subwaveTimer = subwaveTimerMax;

        //this._totalWaves = _dropLocations.indicies.Count;
       
        this._totalWaves = _wavesArray.waves.Count;
        _WaveText.text = (_wave + 1).ToString() + "/" + _totalWaves.ToString();
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

        //times

        if (waveSpawning)
        {
            Debug.Log("waveSpawing is true");
            subwaveTimer -= Time.deltaTime;
            Debug.Log(_totalSubWaves);
            if (subwaveTimer <= 0f)
            {
                
                Debug.Log("wave is spawning");
                subwaveTimer = subwaveTimerMax;
                SpawnNextSubWave();
                
            }
            if (_subWave >= _totalSubWaves)
            {
                //finished a wave
                Debug.Log("wave is done");
                waveSpawning = false;
                _wave++;
            }
        }
        
    }

    public void SpawnNextWave()
    {
        /* if (_wave < _totalWaves)
         {
             if (_wave >= _dropLocations.indicies.Count)
             {
                 _wave = 0;
             }

             foreach (int index in _dropLocations.indicies[_wave].locations)
             {
                 _gridManager.DrawParticle(BlockType.Water, new Vector3(index, _gridManager.getHeight() - 1));
             } 

             _wave++;
             _WaveText.text = _wave.ToString() + "/" + _totalWaves.ToString();
         }*/
        if (_wave < _totalWaves)
        {
            if (_wave >= _wavesArray.waves.Count)
            {
                _wave = 0;
            }

            waveSpawning = true;
            _waveLocations = _wavesArray.waves[_wave];
            _subWave = 0;
            _totalSubWaves = _waveLocations.wavelocations.Count;
            _WaveText.text = (_wave + 1).ToString() + "/" + _totalWaves.ToString();
        }

    }


    public void SpawnNextSubWave()
    {
        //timer here to pause between subwaves
        Debug.Log("current subwave:" + _subWave + " of wave: "  + _wave);
        foreach (int index in _waveLocations.wavelocations[_subWave].locations)
        {
            _gridManager.DrawParticle(BlockType.Water, new Vector3(index, _gridManager.getHeight() - 1));
        }
        _subWave++;
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

        //else start short time after last wave and no water is coming down, it end of timer then win
    }
    
    public void loadNextScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        switch (currentScene.name)
        {
            case "LVL0 BLOCK":
                Debug.Log("block scene");
                SceneManager.LoadScene("LVL0 SHOOT");
                break;
            case "LVL0 SHOOT":
                Debug.Log("shooting scene");
                SceneManager.LoadScene("LVL1");
                break;
            case "LVL1":
                Debug.Log("level 1 scene");
                SceneManager.LoadScene("LVL2");
                break;
            case "LVL2":
                Debug.Log("level 2 scene");
                SceneManager.LoadScene("Main Game Scene");
                break;
            default:
                SceneManager.LoadScene("InstructionOverlay");
                break;

        }

    }
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Instance Is Null");
            }
            return _instance;
        }
    }
}



