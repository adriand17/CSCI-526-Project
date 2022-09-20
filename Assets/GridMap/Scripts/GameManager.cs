using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    [Header("Level Loading")]
    [Tooltip("The following are used for level loading and wave progression.")]
    [SerializeField]public List<TextAsset> _textJson = new List<TextAsset>();
    [SerializeField] public JSONParser _parser;
    public List<Tile> _spawnTiles;
    public JSONParser.DropLocationsArray _dropLocations;
    public GridManager _gridManager;
    private int _wave = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        _spawnTiles = new List<Tile>();
        _dropLocations = new JSONParser.DropLocationsArray();
        _dropLocations.indicies = new List<JSONParser.DropLocations>();
        _parser.Parse(_textJson[0]);
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check if all particles are still
        
    }

    public void SpawnNextWave()
    {
        foreach (int index in _dropLocations.indicies[_wave].locations)
        {
            _gridManager.DrawParticle(BlockType.Water, new Vector3( index, _gridManager.getHeight() -1));
        }
        _wave++;
    }
}
