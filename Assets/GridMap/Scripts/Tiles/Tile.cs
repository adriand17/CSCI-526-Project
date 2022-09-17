using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public enum BlockID 
    { 
        Air,     // Absence of a block.
        Water,   // Water block.
        Bedrock, // Uneditable level structure block.
        Dirt,    // Player placable block.
    }

    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private bool _isBuildable;
    [SerializeField] public GameObject _filledWater;
        
    
    [SerializeField] public BaseTower OccupiedTower;
    [SerializeField] private BaseTower _towerPrefab;

    //public BaseTower OccupiedTower;

    private GridManager gridManager;
    private bool Occupied = false;

    /// Tile's valid neighbors.
    /// Null indicates the world border.
    public Tile leftTile = null;
    public Tile rightTile = null;
    public Tile underTile = null;
    
    public BlockID blockID = BlockID.Air;

    public bool isPassable() { return blockID == BlockID.Air; }
    public void SetTileUnpassable()
    {
        _renderer.color = Color.black;
        blockID = BlockID.Dirt;
    }


    public bool _hasWater = false;
    public Color baseColor = Color.gray;
    public Vector3 location;

    //public bool Buildable => _isBuildable && OccupiedTower == null;
    public bool Buildable => _isBuildable && Occupied == false;

    // Start is called before the first frame update

    public void Init(bool isOffset, BaseTower towerPrefab, Vector3 location, GridManager gridManager)
    {
        _isBuildable = false;
        //buildingManager = bm;
        this._towerPrefab = towerPrefab;
        
        _renderer.color = isOffset ? _offsetColor : _baseColor;
        baseColor = isOffset ? _offsetColor : _baseColor;

        this.location = location;
        this.gridManager = gridManager;
    }

    private void Update()
    {
       if (Buildable)
        {
            _renderer.color = Color.green;
            _isBuildable = true;
        }
        else
        {

        }
    }

    
    private void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        _highlight.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (blockID == BlockID.Air) 
        {
            blockID = BlockID.Dirt;
            _renderer.color = Color.black;
        } else if (blockID == BlockID.Dirt)
        {
            blockID = BlockID.Air;
            _renderer.color = baseColor;
        }
    }

    /// Lets manager inform tile of its valid neighbors.
    public void setAdjacentTiles(Tile underTile, Tile leftTile, Tile rightTile)
    {
        this.underTile = underTile;
        this.rightTile = rightTile;
        this.leftTile = leftTile;
    }


    public void SetBuilding()
    {

        Debug.Log("setting building");
        var tower = Instantiate(_towerPrefab);
        if (tower.OccupiedTile != null) tower.OccupiedTile.OccupiedTower = null;
        Vector3 tPostion = new  Vector3(transform.position.x, transform.position.y, -1);
        tower.transform.position = tPostion;
        this.OccupiedTower = tower;
        tower.OccupiedTile = this;


    }

}
