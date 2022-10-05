using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private bool _isBuildable;

    public bool Occupied = false;
    private bool changeFlage = true;
    public bool _isPassable = true;

    /// Tile's valid neighbors.
    /// Null indicates the world border.
    public Tile upTile = null;
    public Tile downTile = null;
    public Tile leftTile = null;
    public Tile rightTile = null;
    
    private GridManager _gridManager;
    

    public Particle particle;
    public Tower tower;
    public void SetParticle(Particle p)
    {
        this.particle = p;

    }

    public Color baseColor = Color.gray;
    public Vector3 location;

    public bool Buildable => _isBuildable && Occupied == false;

    public void Init(bool isOffset,  Vector3 location, GridManager gridManager)
    {
        _isBuildable = false;
        
        _renderer.color = isOffset ? _offsetColor : _baseColor;
        baseColor = isOffset ? _offsetColor : _baseColor;

        this.location = location;
        this._gridManager = gridManager;
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

    private void OnMouseDown() {
       

        // changeFlage is a check to see if a building can be placed on the location
        // changeFlage = _gridManager.CanAddBlockToTile(location);
        _gridManager.CanAddTowerToTile(location);
        if (changeFlage)
        {

            Debug.Log("added or removed at tile");
        }
        else
        {
            Debug.Log("cancel other buliding to create new one");

        }

    }

    

    /// Lets manager inform tile of its valid neighbors.
    public void setAdjacentTiles(Tile upTile, Tile downTile, Tile leftTile, Tile rightTile)
    {
        this.upTile = upTile;
        this.downTile = downTile;
        this.rightTile = rightTile;
        this.leftTile = leftTile;
    }


    public void SetTower(Tower t)
    {

        Debug.Log("setting building");
        tower = t;
       /* var tower = Instantiate(_towerPrefab);
        if (tower.OccupiedTile != null) tower.OccupiedTile.OccupiedTower = null;
        Vector3 tPostion = new  Vector3(transform.position.x, transform.position.y, -1);
        tower.transform.position = tPostion;
        this.OccupiedTower = tower;
        tower.OccupiedTile = this;*/


    }

}
