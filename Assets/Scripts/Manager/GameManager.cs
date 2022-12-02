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
    [SerializeField] private SpriteRenderer _background;
    
    [Header("Level Loading")]
    [Tooltip("The following are used for level loading and wave progression.")]
    [SerializeField]public List<TextAsset> _textJson = new List<TextAsset>();
    [SerializeField] public JSONParser _parser;
    [SerializeField] public GameObject _WinScreenText;
    private bool didWin = false;

    public JSONParser.GridLocationsArray _dropLocations;
    public JSONParser.GridLocationsArray _gridLocations;
    public JSONParser.WaveArray _wavesArray;
    public JSONParser.Wave _waveLocations;
 
    
    [Header("Reference Managers")]
    public GridManager _gridManager;
    public GunManager _gunManager;
    public AudioSource GMaudioSource;

    [Header("Block Selection Buttons")]
    [SerializeField] public List<BlockType> _blockSelectionButtonTypes;
    [SerializeField] public List<TextMeshProUGUI> textLimitBoxes;
    [SerializeField] public List<TextMeshProUGUI> textPlaceBoxes;
    [SerializeField] public List<int> _blocksGiven;
    [SerializeField] public TextMeshProUGUI waterCount;
    [SerializeField] public TextMeshProUGUI levelName;
    [SerializeField] public Texture2D pickaxeCursor;
    public List<int> blocksPlaced;
    
    [SerializeField] public List<GameObject> _blockSelectionButtons;
    
    private int _wave = 0; //current wave index
    private int _totalWaves; //total amount of waves
    private int _subWave = 0; //current subwave index
    private int _totalSubWaves; //total amount of subwaves in a wave
    public bool waveSpawning = false;
    public float subwaveTimerMax = 2f; //time between subwaves
    public float subwaveTimer;
    private bool reset = false;
    private Scene currentScene;
    void Awake() {
        _instance = this;
        int counter = 0;
        currentScene = SceneManager.GetActiveScene();
        Debug.Log("Started level: " + currentScene.name);
        foreach (GameObject button in _blockSelectionButtons)
        {
            button.GetComponent<ButtonScript>().InitializeButton(_blockSelectionButtonTypes[counter]);
            textLimitBoxes[counter].text = _blocksGiven[counter].ToString();
            counter++;
        }

        // Telemetry: level start
        string level = currentScene.name;
        levelName.text = level;
        string uri = $"https://docs.google.com/forms/d/e/1FAIpQLSfZ8JI_YZx-d-JoHURLWdkNi7IuAWH_X7hsVfNPRcfTK4bxUQ/formResponse?usp=pp_url&entry.1841436937=Start&entry.177939367={level}&submit";
        _gridManager.MakeGetRequest(uri);
    }

    // Start is called before the first frame update
    void Start()
    {
        GMaudioSource = gameObject.AddComponent<AudioSource>();
        _wavesArray = new JSONParser.WaveArray();
        _wavesArray.waves = new List<JSONParser.Wave>();
        _dropLocations = new JSONParser.GridLocationsArray();
        _dropLocations.indicies = new List<JSONParser.GridLocations>();
        _gridLocations = new JSONParser.GridLocationsArray();
        _gridLocations.indicies = new List<JSONParser.GridLocations>();
        _parser.Parse(_textJson[0]);
        _parser.ParseLevel(_textJson[1]);
        _parser.ParseWaves(_textJson[2]);
        blocksPlaced = new List<int>();
        blocksPlaced.Add(_blocksGiven[0]);
        blocksPlaced.Add(_blocksGiven[1]);
        blocksPlaced.Add(_blocksGiven[2]);
        _waveLocations = _wavesArray.waves[0]; //set to first wave
        subwaveTimer = subwaveTimerMax;
        //this._totalWaves = _dropLocations.indicies.Count;
       
        this._totalWaves = _wavesArray.waves.Count;
        UpdateGameState();
        loadBackground();
    }


    // Update is called once per frame
    void Update()
    {
        //Check if all particles are still
        setRemainingWater();
        if (!reset)
        {
            DetermineWinState();
        }
    }

     public void ResetInventory()
    {
        blocksPlaced[0] = _blocksGiven[0];
        blocksPlaced[1] = _blocksGiven[1];
        blocksPlaced[2] = _blocksGiven[2];
        Debug.Log(_blocksGiven[0]);
        Debug.Log(_blocksGiven[1]);
        Debug.Log(_blocksGiven[2]);

        textPlaceBoxes[0].text = _blocksGiven[0].ToString();
        textPlaceBoxes[1].text = _blocksGiven[1].ToString();
        textPlaceBoxes[2].text = _blocksGiven[2].ToString();
    }

    public void SpawnNextWave()
    {
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
        _gridManager.setGridHeight(_gridLocations.rows);
        _gridManager.setGridWidth(_gridLocations.cols);
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
        reset = true;
        _WinScreenText.SetActive(false);
        _wave = 0;
        ResetInventory();
        _gridManager.ResetGrid();
        logReset();
        reset = false;
        didWin = false;
    }


    public void ReturnToLevelSelection()
    {
        SceneManager.LoadScene("Level Selection");
    }

    private void logReset() { 
        string level = currentScene.name;
        string uri = $"https://docs.google.com/forms/d/e/1FAIpQLSfviyNFA3yDEbnJlsuuWuY3di26yUYloZ_K5yuGg-wdc46SUw/formResponse?usp=pp_url&entry.55836778=Yes&entry.594947595={level}&submit";
        _gridManager.MakeGetRequest(uri);
    }


    void DetermineWinState()
    {

        //check is water is present on map if so then bring up win screen
        if(_gridManager.GetWaterCount() == 0 && !didWin)
        {
            _WinScreenText.SetActive(true);
            didWin = true;
            
            // Telemetry: level finish
            string level = SceneManager.GetActiveScene().name;
            string uri = $"https://docs.google.com/forms/d/e/1FAIpQLSfZ8JI_YZx-d-JoHURLWdkNi7IuAWH_X7hsVfNPRcfTK4bxUQ/formResponse?usp=pp_url&entry.1841436937=Finish&entry.177939367={level}&submit";
            _gridManager.MakeGetRequest(uri);
        }

        //else start short time after last wave and no water is coming down, it end of timer then win
    }

    void setRemainingWater()
    {
        waterCount.text = _gridManager.waterCount.ToString();
    }
    
    public void loadNextScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        switch (currentScene.name)
        {
            case "1 Glass Level":
                Debug.Log("1 Glass Level");
                SceneManager.LoadScene("2 Mirror Level");
                break;
            case "2 Mirror Level":
                Debug.Log("2 Mirror Level");
                SceneManager.LoadScene("3 New Dirt Level");
                break;
            case "3 New Dirt Level":
                Debug.Log("3 New Dirt Level");
                SceneManager.LoadScene("4 Magma_TNT");
                break;
            case "4 Magma_TNT":
                Debug.Log("4 Magma_TNT");
                SceneManager.LoadScene("5 Vapor");
                break;
            case "5 Vapor":
                SceneManager.LoadScene("6 Ice Intro");
                break;
            case "6 Ice Intro":
                SceneManager.LoadScene("7 TNT");
                break;
            case "7 TNT":
                SceneManager.LoadScene("8 TNT Mirror");
                break;
            case "8 TNT Mirror":
                SceneManager.LoadScene("9 Mirror 2");
                break;
            case "9 Mirror 2":
                SceneManager.LoadScene("10 Mixed Level 1");
                break;
            case "10 Mixed Level 1":
                SceneManager.LoadScene("11 Mixed Level 2");
                break;
            case "11 Mixed Level 2":
                SceneManager.LoadScene("12 Advance Level 1");
                break;
            case "12 Advance Level 1":
                SceneManager.LoadScene("13 Advance Level 2");
                break;
            case "13 Advance Level 2":
                SceneManager.LoadScene("14 Mixed Level 3");
                break;
            case "14 Mixed Level 3":
                SceneManager.LoadScene("15 Mirror 3 Hard");
                break;
            case "15 Mirror 3 Hard":
                SceneManager.LoadScene("16 TNT 2");
                break;
            case "16 TNT 2":
                SceneManager.LoadScene("17 Vapor 2");
                break;
            case "17 Vapor 2":
                SceneManager.LoadScene("18 Vapor 3");
                break;
            case "18 Vapor 3":
                SceneManager.LoadScene("19 Mixed Level 3");
                break;
            case "19 Mixed Level 3":
                SceneManager.LoadScene("20 Vapor 4");
                break;
            case "20 Vapor 4":
                SceneManager.LoadScene("21 Mixed Level 4");
                break;
            case "21 Mixed Level 4":
                SceneManager.LoadScene("22 Portal");
                break;
            case "22 Portal":
                SceneManager.LoadScene("23 Portal 2");
                break;
            case "23 Portal 2":
                SceneManager.LoadScene("24 Portal 3");
                break;
            case "24 Portal 3":
                SceneManager.LoadScene("25 Portal 4");
                break;
            case "25 Portal 4":
                SceneManager.LoadScene("26 Portal 5");
                break;
            case "26 Portal 5":
                SceneManager.LoadScene("27 Portal 6");
                break;
            case "27 Portal 6":
                SceneManager.LoadScene("28 RainMaker");
                break;
            case "28 RainMaker":
                SceneManager.LoadScene("29 RainMaker 2");
                break;
            case "29 RainMaker 2":
                SceneManager.LoadScene("30 Rain Maker 3");
                break;
            case "30 Rain Maker 3":
                SceneManager.LoadScene("31 Rain Maker 4");
                break;
            case "31 Rain Maker 4":
                SceneManager.LoadScene("32 Portal Rain Maker");
                break;
            case "32 Portal Rain Maker":
                SceneManager.LoadScene("33 Portal Glass Box");
                break;
            case "33 Portal Glass Box":
                SceneManager.LoadScene("34 Rain Maker 5");
                break;
            case "34 Rain Maker 5":
                SceneManager.LoadScene("35 Rain Maker 6");
                break;
            case "35 Rain Maker 6":
                SceneManager.LoadScene("36 Rain Maker 7");
                break;
            case "36 Rain Maker 7":
                SceneManager.LoadScene("Title Screen");
                break;
            default:
                Debug.Log("Scene not found");
                SceneManager.LoadScene("Title Screen");
                break;

        }
    }



    public void loadBackground()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        switch (currentScene.name)
        {
            case "1 Glass Level":
                Debug.Log("1 Glass Level");
                _background.sprite = Resources.Load<Sprite>("5");
                levelName.text = "Level 1";
                break;
            case "2 Mirror Level":
                Debug.Log("2 Mirror Level");
                _background.sprite = Resources.Load<Sprite>("5");
                levelName.text = "Level 2";
                break;
            case "3 New Dirt Level":
                Debug.Log("3 New Dirt Level");
                _background.sprite = Resources.Load<Sprite>("5");
                levelName.text = "Level 3";
                break;
            case "4 Magma_TNT":
                Debug.Log("4 Magma_TNT");
                _background.sprite = Resources.Load<Sprite>("5");
                levelName.text = "Level 4";
                break;
            case "5 Vapor":
                _background.sprite = Resources.Load<Sprite>("5");
                levelName.text = "Level 5";
                break;
            case "6 Ice Intro":
                _background.sprite = Resources.Load<Sprite>("7");
                levelName.text = "Level 6";
                break;
            case "7 TNT":
                _background.sprite = Resources.Load<Sprite>("7");
                levelName.text = "Level 7";
                break;
            case "8 TNT Mirror":
                _background.sprite = Resources.Load<Sprite>("7");
                levelName.text = "Level 8";
                break;
                
            case "9 Mirror 2":
                _background.sprite = Resources.Load<Sprite>("7");
                levelName.text = "Level 9";
                break;
            case "10 Mixed Level 1":
                _background.sprite = Resources.Load<Sprite>("8");
                levelName.text = "Level 10";
                break;
            case "11 Mixed Level 2":
                _background.sprite = Resources.Load<Sprite>("8");
                levelName.text = "Level 11";
                break;
            case "12 Advance Level 1":
                _background.sprite = Resources.Load<Sprite>("7");
                levelName.text = "Level 12";
                break;
            case "13 Advance Level 2":
                _background.sprite = Resources.Load<Sprite>("8");
                levelName.text = "Level 13";
                break;
            case "14 Mixed Level 3":
                _background.sprite = Resources.Load<Sprite>("8");
                levelName.text = "Level 14";
                break;
            case "15 Mirror 3 Hard":
                _background.sprite = Resources.Load<Sprite>("8");
                levelName.text = "Level 15";
                break;
            case "16 TNT 2":
                _background.sprite = Resources.Load<Sprite>("8");
                levelName.text = "Level 16";
                break;
            case "17 Vapor 2":
                _background.sprite = Resources.Load<Sprite>("8");
                levelName.text = "Level 17";
                break;
            case "18 Vapor 3":
                _background.sprite = Resources.Load<Sprite>("14");
                levelName.text = "Level 18";
                break;
            case "19 Mixed Level 3":
                _background.sprite = Resources.Load<Sprite>("14");
                levelName.text = "Level 19";
                break;
            case "20 Vapor 4":
                _background.sprite = Resources.Load<Sprite>("14");
                levelName.text = "Level 20";
                break;
            case "21 Mixed Level 4":
                _background.sprite = Resources.Load<Sprite>("14");
                levelName.text = "Level 21";
                break;
            case "22 Portal":
                _background.sprite = Resources.Load<Sprite>("11");
                levelName.text = "Level 22";
                break;
            case "23 Portal 2":
                _background.sprite = Resources.Load<Sprite>("11");
                levelName.text = "Level 23";
                break;
            case "24 Portal 3":
                _background.sprite = Resources.Load<Sprite>("11");
                levelName.text = "Level 24";
                break;
            case "25 Portal 4":
                _background.sprite = Resources.Load<Sprite>("11");
                levelName.text = "Level 25";
                break;
            case "26 Portal 5":
                _background.sprite = Resources.Load<Sprite>("11");
                levelName.text = "Level 26";
                break;
            case "27 Portal 6":
                _background.sprite = Resources.Load<Sprite>("11");
                levelName.text = "Level 27";
                break;
            case "28 RainMaker":
                _background.sprite = Resources.Load<Sprite>("11");
                levelName.text = "Level 28";
                break;
            case "29 RainMaker 2":
                _background.sprite = Resources.Load<Sprite>("11");
                levelName.text = "Level 29";
                break;
            case "30 Rain Maker 3":
                _background.sprite = Resources.Load<Sprite>("11");
                levelName.text = "Level 30";
                break;
            case "31 Rain Maker 4":
                _background.sprite = Resources.Load<Sprite>("11");
                levelName.text = "Level 31";
                break;
            case "32 Portal Rain Maker":
                _background.sprite = Resources.Load<Sprite>("12");
                levelName.text = "Level 32";
                break;
            case "33 Portal Glass Box":
                _background.sprite = Resources.Load<Sprite>("12");
                levelName.text = "Level 33";
                break;
            case "34 Rain Maker 5":
                _background.sprite = Resources.Load<Sprite>("12");
                levelName.text = "Level 34";
                break;
            case "35 Rain Maker 6":
                _background.sprite = Resources.Load<Sprite>("123");
                levelName.text = "Level 35";
                break;
            case "36 Rain Maker 7":
                _background.sprite = Resources.Load<Sprite>("12");
                levelName.text = "Level 36";
                break;
            default:
                Debug.Log("Scene not found");
                _background.sprite = Resources.Load<Sprite>("12");
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

    public int whichButtonPressed(BlockType type)
    {
        for(int i = 0; i < 3; i++)
        {
            if (_blockSelectionButtonTypes[i] == type)
            {
                return i;
            }
        }
        return -1;
    }
}



