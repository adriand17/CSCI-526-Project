using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Death;

public enum WaterFlowDirection { 
    Still, 
    Left,  
    Right,
    Up,
    Down
}

public class Particle : MonoBehaviour {

    public Block block { get; private set; }

    /// Kind of block this particle is.
    public BlockType getBlockType() { 
        return block.blockType; 
    } 
    public void setBlockType(BlockType blockType, GridManager gridManager) { 
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
                block = new BlueIceBlock(this, gridManager);
                break;
            case BlockType.TNT:
                block = new TNTBlock(this, gridManager);
                break;
            case BlockType.Vapor:
                block = new VaporBlock(this);
                break;
            case BlockType.Evaporator:
                block = new EvaporationBlock(this, gridManager);
                break;
            case BlockType.Condensation:
                block = new CondensationBlock(this, gridManager);
                break;
            case BlockType.RainMaker:
                block = new RainMakerBlock(this);
                break;
            case BlockType.RainTrigger:
                block = new RainTriggerBlock(this);
                break;
            case BlockType.PortalEntry:
                block = new PortalEntryBlock(this, gridManager);
                //gridManager.addPortal(this); 
                break;    
            case BlockType.PortalExit:
                block = new PortalExitBlock(this, gridManager);
                gridManager.addPortal(this);
                break;             
            default:
                //Debug.LogError("Unknown block type: " + blockType);
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
    
    /// Parent grid manager.
    private GridManager _gridManager;

    public void Init(BlockType type, Tile t, GridManager gridManager) {
        /// Calculate a random delay.
        delay = Random.Range(0, TickInterval);

        setBlockType(type, gridManager);
        this.tile = t;
        this._gridManager = gridManager;
        
        /// Prevents particle hiding behind tile.
        _renderer.sortingLayerName = "ParticleLayer";
        
        setBlockSprite();
    }

    private void setBlockSprite() { 
        switch (block.blockType) {
            case BlockType.Water:
                WaterBlock waterBlock = (WaterBlock)block;
                waterBlock.UpdateSprite();
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

            case BlockType.TNT:
                _renderer.sprite = Resources.Load<Sprite>("TNT");
                _renderer.color = Color.white;
                break;
            case BlockType.Vapor:
                _renderer.sprite = Resources.Load<Sprite>("smoke1");
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
            case BlockType.RainTrigger:
                _renderer.sprite = Resources.Load<Sprite>("RainTrigger");
                _renderer.color = Color.white;
                break;
            case BlockType.RainMaker:
                _renderer.sprite = Resources.Load<Sprite>("RainMaker");
                _renderer.color = Color.white;
                break;
            case BlockType.PortalEntry:
                _renderer.sprite = Resources.Load<Sprite>("PortalEntry");
                _renderer.color = Color.white;
                break;
            case BlockType.PortalExit:
                _renderer.sprite = Resources.Load<Sprite>("PortalExit");
                _renderer.color = Color.white;
                break;
            default:
                Debug.LogError("Unhandled block type: " + block.blockType);
                break;
        }
    }

    private void OnMouseEnter()
    {
        if(_gridManager.CanBreakBlockAtTile(this.tile.location)) { 
            _renderer.sprite = Resources.Load<Sprite>("Pickaxe");
        } else { 
            _renderer.sprite = Resources.Load<Sprite>("Barrier");
        }
        _renderer.color = new Color(1, 1, 1, 0.5f);
    }

    private void OnMouseExit()
    {
        setBlockSprite();
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
        if(_gridManager.CanBreakBlockAtTile(this.tile.location)) { 
            _gridManager.BreakBlockAtTile(this.tile.location);
        }
    }

    public void DeleteParticle(Cause cause, BlockType blockType) {
        if (cause == Cause.Explosion) { 
            if (blockType == BlockType.TNT) {
                int exlposionNum = Random.Range(1, 4);
                if (exlposionNum == 1) {
                    tile.playSoundNamed("explode1");
                } else if (exlposionNum == 2) {
                    tile.playSoundNamed("explode2");
                } else if (exlposionNum == 3) {
                    tile.playSoundNamed("explode3");
                } else if (exlposionNum == 4) {
                    tile.playSoundNamed("explode4");
                }
            } else { 
                /// Do nothing, TNT plays sound.
            }
        } else { 
            if (getBlockType() == BlockType.Water) {
                tile.playSoundNamed("fizz");
            }
        }

        if (getBlockType() == BlockType.Water) {
            // Log water position on death.
            string level = SceneManager.GetActiveScene().name;
            string causeStr = cause.name();
            string url = $"https://docs.google.com/forms/d/e/1FAIpQLSePv5UFqW2cPAadJGiioCkeaH7Uoe09bREG5CYxSNP4JEjmxQ/formResponse?usp=pp_url&entry.1230153677={tile.location.x}&entry.813067426={tile.location.y}&entry.541452585={causeStr}&entry.711829831={level}&submit";

            /// Make request on object that isn't about to die.
            /// Docs: https://docs.unity3d.com/ScriptReference/MonoBehaviour.StartCoroutine.html
            /// > Coroutines are ... stopped when the MonoBehaviour is destroyed
            _gridManager.MakeGetRequest(url);
            _gridManager.waterCount--;
        } 

        tile.SetParticle(null);
        _gridManager.particles.Remove(this);
        _gridManager.removePortal(this);
        Destroy(this.gameObject);
    }

    public List<Vector3> getAllPortalPosition() {
       return _gridManager.getAllPortalPosition();
    }
}
