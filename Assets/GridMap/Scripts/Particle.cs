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

    /// Kind of block this particle is.
    private BlockType blockType;
    public BlockType getBlockType() { 
        return blockType; 
    } 
    public void setBlockType(BlockType blockType) { 
        this.blockType = blockType; 
        
        /// Reset metadata.
        _waterFlowDirection = WaterFlowDirection.Still;
        dirtDurability = Particle.DirtMaxDurability;
        temperature = Particle.tempInit;
    }

    /// Amount of time since last update.
    private float _timeSinceLastUpdate;
    private static float _WaterInterval = 0.5f;

    [SerializeField] private SpriteRenderer _renderer;
    
    /// Reference to the tile where this particle is located.
    public Tile tile;

    private bool atBottom = false;
    public bool userPlaced = false;

    /// Parent grid manager.
    private GridManager _gridManager;

    /// [WATER SPECIFIC]
    /// Direction that the water is flowing.
    private WaterFlowDirection _waterFlowDirection;

    /// Water Temperature.
    private int temperature; 
    private static int tempFreeze = 0;
    private static int tempVapor = 10;
    private static int tempInit = 5;
    public static int TempLaser = +2;

    /// [DIRT SPECIFIC]
    /// Dirt Durability.
    private static int DirtMaxDurability = 5;
    private int dirtDurability = DirtMaxDurability;

    public void Init(BlockType type, Tile t, GridManager gridManager) {
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
                dirtDurability = Particle.DirtMaxDurability;
                
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

            case BlockType.Heater:
                _renderer.sprite = Resources.Load<Sprite>("Magma");
                _renderer.color = Color.white;
                break;

            case BlockType.Cooler:
                break;

            default:
                Debug.LogError("Unhandled block type: " + type);
                break;
        }
    }

    public void Update() {
        _timeSinceLastUpdate += Time.deltaTime;
        if (_timeSinceLastUpdate < Particle._WaterInterval) {
            return;
        }
        _timeSinceLastUpdate = 0;
        
        if (blockType == BlockType.Water) {
            WaterTick();

            //check if water at bottom
            if(tile.downTile == null) {
                hasHitBottom();
            }
        } else if (blockType == BlockType.Dirt) {
            DirtTick();
        }
    }

    /// Try to move the water particle to the left.
    /// Returns true if the particle moved.
    private bool flowLeft() {
        if (tile.leftTile != null && tile.leftTile.particle == null) {
            this._waterFlowDirection = WaterFlowDirection.Left;
            Tile oldTile = this.tile;
            tile.leftTile.SetParticle(this);
            MoveWater(Vector3.left);
            oldTile.SetParticle(null);
            
            return true;
        }
        return false;
    }

    /// Try to move the particle to the right.
    /// Returns true if the particle moved.
    private bool flowRight() {
        if (tile.rightTile != null && tile.rightTile.particle == null) {
            this._waterFlowDirection = WaterFlowDirection.Right;
            Tile oldTile = this.tile;
            tile.rightTile.SetParticle(this);
            MoveWater(Vector3.right);
            oldTile.SetParticle(null);
            
            return true;
        }
        return false;
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

    /// Try to move water.
    private void WaterTick() {
        CoolWater();
        WaterFlow();
    }

    private void WaterFlow() { 
        // Check if water can flow down.
        if (tile.downTile != null && tile.downTile.particle == null) {
            this._waterFlowDirection = WaterFlowDirection.Down;
            Tile oldTile = this.tile;
            tile.downTile.SetParticle(this);
            MoveWater(Vector3.down);
            oldTile.SetParticle(null);
            return;
        }

        switch (_waterFlowDirection) {
            case WaterFlowDirection.Still:
                if (Random.value >= 0.5) {
                    if (!flowLeft() && !flowRight()) {
                        _waterFlowDirection = WaterFlowDirection.Still;
                    }
                } else {
                    if (!flowRight() && !flowLeft()) {
                        _waterFlowDirection = WaterFlowDirection.Still;
                    }
                }
                break;
            
            case WaterFlowDirection.Down:
                if (Random.value >= 0.5) {
                    if (!flowLeft() && !flowRight()) {
                        _waterFlowDirection = WaterFlowDirection.Still;
                    }
                } else {
                    if (!flowRight() && !flowLeft()) {
                        _waterFlowDirection = WaterFlowDirection.Still;
                    }
                }
                break;
            
            case WaterFlowDirection.Left:
                if (!flowLeft() && !flowRight()) {
                    _waterFlowDirection = WaterFlowDirection.Still;
                }
                break;

            case WaterFlowDirection.Right:
                if (!flowRight() && !flowLeft()) {
                    _waterFlowDirection = WaterFlowDirection.Still;
                }
                break;
        }
    }

    /// Async coroutine to animate the drop moving.
    private void MoveWater(Vector3 direction) {
        //this.isMoving = true;
        Tile destinationTile;
        switch (_waterFlowDirection) {
            case WaterFlowDirection.Down:
                destinationTile = tile.downTile;
                break;
            case WaterFlowDirection.Right:
                destinationTile = tile.rightTile;
                break;
            case WaterFlowDirection.Left:
                destinationTile = tile.leftTile;
                break;
            default:
                destinationTile = tile;
                break;
        }
     
        this.tile = destinationTile;

        transform.position = new Vector3(destinationTile.transform.position.x, destinationTile.transform.position.y, -1);
    }

    private void CoolWater() { 
        if (blockType != BlockType.Water) {
            Debug.LogError("Cool: non-water particle");
            return;
        }

        void TradeHeat(Tile neighbor) { 
            if (neighbor == null || neighbor.particle == null) {
                return;
            }
            Particle p = neighbor.particle;
            if (p.blockType != BlockType.Water || p.temperature >= this.temperature) {
                return;
            }
            
            p.temperature += 1;
            this.temperature -= 1;
            p.ShowWaterHeat();
        }

        /// Share heat with neighbors.
        TradeHeat(tile.upTile);
        TradeHeat(tile.downTile);
        TradeHeat(tile.leftTile);
        TradeHeat(tile.rightTile);

        /// Cool off naturally.
        if (temperature > Particle.tempInit) {
            temperature -= 1;
        }

        ShowWaterHeat();
    }

    public void HeatWater(int tempChange) {
        if (blockType != BlockType.Water) {
            Debug.LogError("Heat: non-water particle");
            return;
        }

        temperature += tempChange;
        if (temperature >= tempVapor) {
            DeleteParticle();
            return;
        } else { 
            ShowWaterHeat();
        }
    }

    private void ShowWaterHeat() { 
        /// Get redder based on temperature.
        float red = (float)(temperature - tempFreeze) / (float)(tempVapor - tempFreeze);
        red *= 0.75f; // Dampen effect.
        _renderer.color = new Color(red, 0, 1);
    }

    private void DirtTick() { 
        bool upIsWater = tile.upTile != null && tile.upTile.particle != null && tile.upTile.particle.blockType == BlockType.Water;
        bool leftIsWater = tile.leftTile != null && tile.leftTile.particle != null && tile.leftTile.particle.getBlockType() == BlockType.Water;
        bool rightIsWater = tile.rightTile != null && tile.rightTile.particle != null && tile.rightTile.particle.getBlockType() == BlockType.Water;

        if (upIsWater || leftIsWater || rightIsWater) { 
            dirtDurability -= 1;
        }
        
        /// Swap in a broken sprite.
        switch (dirtDurability) { 
            case 5:
                _renderer.sprite = Resources.Load<Sprite>("Dirt");
                break;
            
            case 4:
                _renderer.sprite = Resources.Load<Sprite>("Dirt Break 1");
                break;
            
            case 3:
                _renderer.sprite = Resources.Load<Sprite>("Dirt Break 2");
                break;
            
            case 2:
                _renderer.sprite = Resources.Load<Sprite>("Dirt Break 3");
                break;
            
            case 1: 
                _renderer.sprite = Resources.Load<Sprite>("Dirt Break 4");
                break;
        }
        
        if (dirtDurability <= 0) {    
            DeleteParticle();
        }
    }

    public void DeleteParticle() {
        if (blockType == BlockType.Water) {
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
