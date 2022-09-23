using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Water,   // Water block.
    Bedrock, // Uneditable level structure block.
    Dirt,    // Player placable block.
}

public enum WaterFlowDirection { 
    Still, 
    Left,  
    Right, 
    Down
}

public class Particle : MonoBehaviour{

    /// Kind of block this particle is.
    private BlockType blockType;
    public BlockType getBlockType() { 
        return blockType; 
    } 
    public void setBlockType(BlockType blockType) { 
        this.blockType = blockType; 
        /// Reset metadata.
        _waterFlowDirection = WaterFlowDirectionDirection.Still;
    }

    /// Amount of time since last update.
    private float waterInterval;

    [SerializeField] private SpriteRenderer _renderer;
    
    public Tile tile;
    private bool atBottom = false;
    public bool userPlaced = false;

    /// Direction that the water is flowing.
    private WaterFlowDirection _waterFlowDirection;
    
    private GridManager _gridManager;

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
                _renderer.color = Color.black;
                break;
            case BlockType.Dirt:
                _renderer.color = new Color(0.5f, 0.25f, 0);
                break;
        }

    }

    public void Update()
    {
        if (blockType == BlockType.Water)
        {

            waterInterval += Time.deltaTime;
            if (waterInterval < 0.25f)
            {
                return;
            }
            waterInterval = 0;
            WaterTick();


            //check if water at bottom
            if(tile.underTile == null)
            {
                hasHitBottom();
            }
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
        if (!atBottom && tile.underTile == null)
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
        if (tile.underTile != null && tile.underTile.particle == null) {
            this._waterFlowDirection = WaterFlowDirection.Down;
            Tile oldTile = this.tile;
            tile.underTile.SetParticle(this);
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
                destinationTile = tile.underTile;
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

}
