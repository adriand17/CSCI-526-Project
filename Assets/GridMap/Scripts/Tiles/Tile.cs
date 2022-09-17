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

    private bool Occupied = false;

    /// Tile's valid neighbors.
    /// Null indicates the world border.
    public Tile leftTile = null;
    public Tile rightTile = null;
    public Tile underTile = null;
    
    private GridManager _gridManager;

    public Particle particle;
    public void SetParticle(Particle p)
    {
        this.particle = p;
        if (p == null) {
            _renderer.color = _baseColor;
            return;
        }

        p.tile = this;
        switch (particle.getBlockType())
        {
            case BlockType.Water:
                _renderer.color = Color.blue;
                break;
            case BlockType.Bedrock:
                _renderer.color = Color.black;
                break;
            case BlockType.Dirt:
                _renderer.color = new Color(0.5f, 0.25f, 0);
                break;
        }
    }

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
        if (particle == null) {
            Particle p = new Particle(BlockType.Water);
            SetParticle(p);
            this._gridManager.particles.Add(p);
        } else if (particle.getBlockType() == BlockType.Dirt) {
            this._gridManager.particles.Remove(particle);
            SetParticle(null);
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
