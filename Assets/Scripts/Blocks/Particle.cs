using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaterFlowDirection { 
    Still, 
    Left,  
    Right, 
    Down
}

/**
# Water Todo
- [x] add a temperature value
- [x] shed heat to adjacent water / non-water
- [ ] add a a heating "tower" block
- [ ] add a cooling "tower" block
- [ ] add an "ice" block
- [ ] transform to ice when too cold
- [x] have laser increase temperature
- [x] destroy water that's too hot

 */

public class Particle : MonoBehaviour {

    public Block block { get; private set; }

    /// Kind of block this particle is.
    public BlockType getBlockType() { 
        return block.blockType; 
    } 
    public void setBlockType(BlockType blockType) { 
        switch (blockType) {
            case BlockType.Water:
                block = new WaterBlock(this);
                break;
            case BlockType.Bedrock:
                block = new BedrockBlock(this);
                break;
            case BlockType.Dirt:
                block = new DirtBlock(this);
                break;
            case BlockType.Mirror:
                block = new MirrorBlock(this);
                break;
            case BlockType.Glass:
                block = new GlassBlock(this);
                break;
            case BlockType.Magma:
                block = new MagmaBlock(this);
                break;
            case BlockType.BlueIce:
                block = new BlueIceBlock(this);
                break;
            default:
                Debug.LogError("Unknown block type: " + blockType);
                break;
        }
    }

    /// Amount of time since last update.
    private float _timeSinceLastUpdate;
    public static float TickInterval = 0.75f;
    private float delay;

    [SerializeField] public SpriteRenderer _renderer;
    
    /// Reference to the tile where this particle is located.
    public Tile tile;

    private bool atBottom = false;
    public bool userPlaced = false;

    /// Parent grid manager.
    private GridManager _gridManager;

    public void Init(BlockType type, Tile t, GridManager gridManager) {
        /// Calculate a random delay.
        delay = Random.Range(0, TickInterval);

        setBlockType(type);
        this.tile = t;
        this._gridManager = gridManager;
        
        /// Prevents particle hiding behind tile.
        _renderer.sortingLayerName = "ParticleLayer";
        
        switch (type) {
            case BlockType.Water:
                _renderer.color = new Color(0, 0, 1);
                break;
            
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

            default:
                Debug.LogError("Unhandled block type: " + type);
                break;
        }
    }

    public void Update() {
        _timeSinceLastUpdate += Time.deltaTime;
        if (_timeSinceLastUpdate < Particle.TickInterval + delay) {
            return;
        }
        _timeSinceLastUpdate = delay;
        
        if (getBlockType() == BlockType.Water) {
            //check if water at bottom
            if(tile.downTile == null) {
                hasHitBottom();
            }
        }
        if (block != null) {
            block.Tick();
        }
    }

    private void hasHitBottom() {
        if (!atBottom && tile.downTile == null) {
            atBottom = true;
        }
    }


    private void OnMouseDown() {
        // changeFlage is a check to see if a building can be placed on the location
        Debug.Log("clicking on particle");

        bool changeFlage = _gridManager.CanAddBlockToTile(this.tile.location);
        if (changeFlage)
        {
            Debug.Log("added or rmoved at tile");
        } else {
            Debug.Log("cancel other buliding to create new one");
        }
    }

    public void DeleteParticle() {
        if (getBlockType() == BlockType.Water) {
            // Log water position on death.
            int level = 0;
            string cause = "Laser";
            string url = $"https://docs.google.com/forms/d/e/1FAIpQLSd8VI1L_HMJ3GxVBSVzR44PyB3NPiK_6GqeYe7zqZqafrFtIQ/formResponse?usp=pp_url&entry.1421622821={level}&entry.2002566203={tile.location.x}&entry.1372862866={tile.location.y}&entry.1572288735={cause}&submit=Submit";

            /// Make request on object that isn't about to die.
            /// Docs: https://docs.unity3d.com/ScriptReference/MonoBehaviour.StartCoroutine.html
            /// > Coroutines are ... stopped when the MonoBehaviour is destroyed
            _gridManager.MakeGetRequest(url);
        }

        tile.SetParticle(null);
        _gridManager.particles.Remove(this);
        Destroy(this.gameObject);
    }
}
