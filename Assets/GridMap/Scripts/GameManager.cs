using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private GameObject _gunObject;
    [SerializeField] private GameObject _camera;
    
    [Header("Level Loading")]
    [Tooltip("The following are used for level loading and wave progression.")]
    [SerializeField]public List<TextAsset> _textJson = new List<TextAsset>();
    [SerializeField] public JSONParser _parser;
    public List<Tile> _spawnTiles;
    public JSONParser.GridLocationsArray _dropLocations;
    public JSONParser.GridLocationsArray _gridLocations;
    public GridManager _gridManager;
    public GunManager _gunManager;
    private int _wave = 0;
    
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
        UpdateGameState();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if all particles are still
        
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
    }
    
    void handleGrid() {
        _gridManager.onStart();
    }

    void handleGun(){
        _gunManager.handleGunPosition();
    }
    
    void UpdateGameState() {
        //TODO: Add game state listener
        handleGrid();
        handleGun();
    }
}
