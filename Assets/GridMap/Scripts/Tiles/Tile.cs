using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private bool _isBuildable;
    
    [SerializeField] public BaseTower OccupiedTower;
    [SerializeField] private BaseTower _towerPrefab;

    //public BaseTower OccupiedTower;

    private BuildingManager buildingManager;
    private bool Occupied = false;
    public Tile leftTile = null;
    public Tile rightTile = null;
    public Tile underTile = null;
    public bool _isPassable = true;

    //public bool Buildable => _isBuildable && OccupiedTower == null;
    public bool Buildable => _isBuildable && Occupied == false;

    // Start is called before the first frame update

    public void Init(bool isOffset, BaseTower towerPrefab, bool _isPassable)
    {
        _isBuildable = false;
        //buildingManager = bm;
        this._towerPrefab = towerPrefab;
        this._isPassable = _isPassable;
        
        _renderer.color = isOffset ? _offsetColor : _baseColor;
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

    public void SetTileUnpassable()
    {
        _renderer.color = Color.black;
        _isPassable = false;
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

       if (Buildable)
       {

            SetBuilding();
            
            Occupied = true;
       }
        
    }

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
