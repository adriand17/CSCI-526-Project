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
    private Sprite defaultSprite;

    /// Tile's grid coordinates.
    public Vector2 gridPosition;

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

    public void Init(bool isOffset,  Vector2 gridPosition, GridManager gridManager)
    {
        _isBuildable = false;
        
        _renderer.color = isOffset ? _offsetColor : _baseColor;
        baseColor = isOffset ? _offsetColor : _baseColor;

        this.gridPosition = gridPosition;
        this.location = new Vector3(gridPosition.x, gridPosition.y, -1);
        this._gridManager = gridManager;
        defaultSprite = _renderer.sprite;
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
        if(particle == null)
        {
            showPreview();
        }
        
        
    }

    private void OnMouseExit()
    {
        _highlight.SetActive(false);
       
        hidePreview();
       
        
    }

    private void showPreview()
    {
        BlockType b = _gridManager.getBuildType();
        Debug.Log(_renderer.sprite);
        var c = Color.white;
        c.a = 0.5f;
        switch (b)
        {
            case BlockType.Bedrock:
                _renderer.sprite = Resources.Load<Sprite>("Bedrock");
                _renderer.color = Color.white;
                break;

            case BlockType.Dirt:
                _renderer.sprite = Resources.Load<Sprite>("Dirt");
                _renderer.color = Color.white;
                break;

            case BlockType.Mirror:
                _renderer.sprite = Resources.Load<Sprite>("Mirror");
                _renderer.color = Color.white;
                break;

            case BlockType.Glass:
                _renderer.sprite = Resources.Load<Sprite>("Glass");
                _renderer.color = Color.white;
                break;

            case BlockType.Magma:
                _renderer.sprite = Resources.Load<Sprite>("Magma");
                _renderer.color = Color.white;
                break;

            case BlockType.BlueIce:
                _renderer.sprite = Resources.Load<Sprite>("BlueIce");
                _renderer.color = Color.white;
                break;

            case BlockType.TNT:
                _renderer.sprite = Resources.Load<Sprite>("TNT");
                _renderer.color = Color.white;
                break;
            case BlockType.Vapor:
                _renderer.sprite = Resources.Load<Sprite>("Water");
                _renderer.color = Color.white;
                break;
            case BlockType.Evaporator:
                _renderer.sprite = Resources.Load<Sprite>("Evaporator");
                _renderer.color = Color.white;
                break;
            case BlockType.Condensation:
                _renderer.sprite = Resources.Load<Sprite>("Condensation");
                _renderer.color = Color.white;
                break;
            case BlockType.None:
                break;
            default:
                //Debug.LogError("Unhandled block type: " + type);
                break;
        }
        _renderer.color = c;
    }


    private void hidePreview()
    {
        Debug.Log(_renderer.sprite);
        _renderer.sprite = defaultSprite;
        _renderer.color = baseColor;
    }

    private void OnMouseDown() {
        // changeFlage is a check to see if a building can be placed on the location
        changeFlage = _gridManager.CanAddBlockToTile(location);
        //_gridManager.CanAddTowerToTile(location);
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
        //this.upTile = upTile;
        //this.downTile = downTile;
        //this.rightTile = rightTile;
        //this.leftTile = leftTile;
    }


    public void SetTower(Tower t)
    {
        tower = t;
    }

    /// [TILE GRID COORDINATES]
    public Tile getRelativeTile(Vector2 position) {
        return _gridManager.GetTileAt(gridPosition + position);
    }

    public Tile upTile {
        get { return getRelativeTile(Vector2.up); }
    }

    public Tile downTile {
        get { return getRelativeTile(Vector2.down); }
    }

    public Tile leftTile {
        get { return getRelativeTile(Vector2.left); }
    }

    public Tile rightTile {
        get { return getRelativeTile(Vector2.right); }
    }
}
