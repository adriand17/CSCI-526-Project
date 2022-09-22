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
    public JSONParser.DropLocationsArray _dropLocations;
    public GridManager _gridManager;
    private int _wave = 0;
    
    void Awake() {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _spawnTiles = new List<Tile>();
        _dropLocations = new JSONParser.DropLocationsArray();
        _dropLocations.indicies = new List<JSONParser.DropLocations>();
        _parser.Parse(_textJson[0]);
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
        GridManager.Instance.onStart();
    }

    void handleGun(){
        GunManager.Instance.handleGunPosition();
    }
    
    void UpdateGameState() {
        //TODO: Add game state listener
        handleGrid();
        handleGun();

    }
}
