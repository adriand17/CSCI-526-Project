using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _rangeHighlight;
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
    List<Tile> tList = new List<Tile>();

    public bool Buildable => _isBuildable && Occupied == false;

    /// Audio Source lives in Tile, so that it can play 
    /// Particle death sounds after Particle is destroyed.
    private AudioSource audioSource;

    public void Init(bool isOffset,  Vector2 gridPosition, GridManager gridManager)
    {
        _isBuildable = false;

        
        

        _renderer.color = isOffset ? _offsetColor : _baseColor;
        baseColor = isOffset ? _offsetColor : _baseColor;

        var c = baseColor;
        c.a = 0.01f; // Semi-transparent
        _renderer.color = c;
        baseColor = c;

        this.gridPosition = gridPosition;
        this.location = new Vector3(gridPosition.x, gridPosition.y, -1);
        this._gridManager = gridManager;
        defaultSprite = _renderer.sprite;

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
       if (Buildable)
        {
            _renderer.color = Color.green;
            _isBuildable = true;
        }
    }
    
    private void OnMouseEnter()
    {
        _highlight.SetActive(true);
        if(particle == null)
        {
            showPreview();
            showRangePreview();
        }
    }

    private void OnMouseExit()
    {
        _highlight.SetActive(false);
        hidePreview();
        hideRangePreview();
    }

    // Shows a semi-transparent version of block,
    // so the player can preview their action.
    private void showPreview()
    {
        var c = Color.white;
        c.a = 0.5f; // Semi-transparent
     
        BlockType b = _gridManager.getBuildType();
        switch (b)
        {
            case BlockType.Bedrock:
                _renderer.sprite = Resources.Load<Sprite>("Bedrock");
                break;

            case BlockType.Dirt:
                _renderer.sprite = Resources.Load<Sprite>("Dirt");
                break;

            case BlockType.Mirror:
                _renderer.sprite = Resources.Load<Sprite>("Mirror");
                break;

            case BlockType.Glass:
                _renderer.sprite = Resources.Load<Sprite>("Glass");
                break;

            case BlockType.Magma:
                _renderer.sprite = Resources.Load<Sprite>("Magma");
                break;

            case BlockType.BlueIce:
                _renderer.sprite = Resources.Load<Sprite>("BlueIce");
                break;

            case BlockType.TNT:
                _renderer.sprite = Resources.Load<Sprite>("TNT");
                break;
            
            case BlockType.Vapor:
                _renderer.sprite = Resources.Load<Sprite>("Water");
                break;
            
            case BlockType.Evaporator:
                _renderer.sprite = Resources.Load<Sprite>("Evaporator");
                break;
            
            case BlockType.Condensation:
                _renderer.sprite = Resources.Load<Sprite>("Condensation");
                break;
            case BlockType.PortalEntry:
                _renderer.sprite = Resources.Load<Sprite>("PortalEntry");
                break;
            case BlockType.PortalExit:
                _renderer.sprite = Resources.Load<Sprite>("PortalExit");
                break;
            case BlockType.None:
                break;
            
            default:
                //Debug.LogError("Unhandled block type: " + type);
                break;
        }
        _renderer.color = c;
    }

    private void showRangePreview()
    {
        tList = _gridManager.GetInRangeTiles(location);
        var c = Color.red;
        c.a = 0.5f;
        foreach (Tile t in tList)
        {
            if (t != null && t.particle == null)
                t._rangeHighlight.SetActive(true);
        }
    }

    private void hideRangePreview()
    {
        foreach (Tile t in tList)
        {
            if (t != null)
                t._rangeHighlight.SetActive(false);
        }
    }

    private void hidePreview()
    {
        Debug.Log(_renderer.sprite);
        _renderer.sprite = defaultSprite;
        _renderer.color = baseColor;
    }

    private void OnMouseDown() {
        if (_gridManager.CanAddBlockToTile(location)) { 
            _gridManager.AddBlockToTile(location);
        }
    }

    /// Lets manager inform tile of its valid neighbors.
    public void setAdjacentTiles(Tile upTile, Tile downTile, Tile leftTile, Tile rightTile)
    {
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

    /// [SOUNDS]
    public void playSoundNamed(string soundName) {
        audioSource.clip = Resources.Load<AudioClip>(soundName);
        audioSource.Play();
    }
}
