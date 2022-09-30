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
- [ ] add a temperature value
- [ ] shed heat to adjacent water / non-water
- [ ] add a a heating "tower" block
- [ ] add a cooling "tower" block
- [ ] add an "ice" block
- [ ] transform to ice when too cold
- [ ] have laser increase temperature
- [ ] destroy water that's too hot

 */

public class Particle : MonoBehaviour{

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

    /// [DIRT SPECIFIC]
    /// Dirt Durability.
    private static int DirtMaxDurability = 5;
    private int dirtDurability = DirtMaxDurability;

    public Particle(BlockType type)
    {
        blockType = type;
    }

    public void Init(BlockType type, Tile t, GridManager gridManager)
    {
        blockType = type;
        this.tile = t;
        this._gridManager = gridManager;
        switch (type)
        {
            case BlockType.Water:
                _renderer.color = Color.blue;
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

    private void hasHitBottom()
    {
        if (!atBottom && tile.downTile == null)
        {
            atBottom = true;
           // grid.TakeDamage();
        }
    }


    private void OnMouseDown()
    {


        // changeFlage is a check to see if a building can be placed on the location
        Debug.Log("clicking on particle");

        bool changeFlage = _gridManager.CanAddBlockToTile(this.tile.location);
        if (changeFlage)
        {

            Debug.Log("added or rmoved at tile");
            /*if (particle == null) {
                _gridManager.DrawParticle(BlockType.Dirt, this.location);

            } else if (particle.getBlockType() == BlockType.Dirt) {
                 this._gridManager.particles.Remove(particle);
                 SetParticle(null);
            }*/
        }
        else
        {
            Debug.Log("cancel other buliding to create new one");

        }

    }

    /// Try to move water.
    private void WaterTick() {

        
        // Check if water can flow down.
        if (tile.downTile != null && tile.downTile.particle == null) {
            this._waterFlowDirection = WaterFlowDirection.Down;
            Tile oldTile = this.tile;
            tile.downTile.SetParticle(this);
            MoveWater(Vector3.down);
            oldTile.SetParticle(null);
            return;
        }

        switch (_waterFlowDirection)
        {
            case WaterFlowDirection.Still:
                if (Random.value >= 0.5)
                {
                    if (!flowLeft() && !flowRight())
                    {
                        _waterFlowDirection = WaterFlowDirection.Still;
                    }
                }
                else
                {
                    if (!flowRight() && !flowLeft())
                    {
                        _waterFlowDirection = WaterFlowDirection.Still;
                    }
                }
                break;
            case WaterFlowDirection.Down:
                if (Random.value >= 0.5)
                {
                    if (!flowLeft() && !flowRight())
                    {
                        _waterFlowDirection = WaterFlowDirection.Still;
                    }
                }
                else
                {
                    if (!flowRight() && !flowLeft())
                    {
                        _waterFlowDirection = WaterFlowDirection.Still;
                    }
                }
                break;
            case WaterFlowDirection.Left:
                if (!flowLeft() && !flowRight())
                {
                    _waterFlowDirection = WaterFlowDirection.Still;
                }
                break;

            case WaterFlowDirection.Right:
                if (!flowRight() && !flowLeft())
                {
                    _waterFlowDirection = WaterFlowDirection.Still;
                }
                break;
        }
    }

    /// Async coroutine to animate the drop moving.
    private void MoveWater(Vector3 direction)
    {
        //this.isMoving = true;
        Tile destinationTile;
        switch (_waterFlowDirection)
        {
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
            tile.SetParticle(null);
            _gridManager.particles.Remove(this);
            Destroy(this.gameObject);
        }
    }



}
