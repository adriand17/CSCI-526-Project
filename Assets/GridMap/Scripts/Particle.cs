using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Water,   // Water block.
    Bedrock, // Uneditable level structure block.
    Dirt,    // Player placable block.
}

public enum WaterFlow { 
    Still, 
    Left,  
    Right, 
    Down
}

public class Particle : MonoBehaviour{

    private BlockType blockType;
    private float waterInterval;
    [SerializeField] private SpriteRenderer _renderer;
    public BlockType getBlockType() { 
        return blockType; 
    } 
    public void setBlockType(BlockType blockType) { 
        this.blockType = blockType; 
        /// Reset metadata.
        _waterFlow = WaterFlow.Still;
    }

    public Tile tile;
    private bool atBottom = false;
    private WaterFlow _waterFlow;

    public Particle(BlockType type)
    {
        blockType = type;
    }

    public void Init(BlockType type, Tile t)
    {
        blockType = type;
        this.tile = t;
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
        }
    }

    /// Try to move the water particle to the left.
    /// Returns true if the particle moved.
    private bool flowLeft() {
        if (tile.leftTile != null && tile.leftTile.particle == null) {
            this._waterFlow = WaterFlow.Left;
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
            this._waterFlow = WaterFlow.Right;
            Tile oldTile = this.tile;
            tile.rightTile.SetParticle(this);
            MoveWater(Vector3.right);
            oldTile.SetParticle(null);
            
            
            return true;
        }
        return false;
    }

    private void hasHitBottom(GridManager grid)
    {
        if (!atBottom && tile.underTile == null)
        {
            atBottom = true;
            grid.TakeDamage();
        }
    }

    /// Try to move water.
    private void WaterTick() {

        
        // Check if water can flow down.
        if (tile.underTile != null && tile.underTile.particle == null) {
            this._waterFlow = WaterFlow.Down;
            Tile oldTile = this.tile;
            tile.underTile.SetParticle(this);
            MoveWater(Vector3.down);
            oldTile.SetParticle(null);
            return;
        }

        switch (_waterFlow)
        {
            case WaterFlow.Still:
                if (Random.value >= 0.5)
                {
                    if (!flowLeft() && !flowRight())
                    {
                        _waterFlow = WaterFlow.Still;
                    }
                }
                else
                {
                    if (!flowRight() && !flowLeft())
                    {
                        _waterFlow = WaterFlow.Still;
                    }
                }
                break;
            case WaterFlow.Down:
                if (Random.value >= 0.5)
                {
                    if (!flowLeft() && !flowRight())
                    {
                        _waterFlow = WaterFlow.Still;
                    }
                }
                else
                {
                    if (!flowRight() && !flowLeft())
                    {
                        _waterFlow = WaterFlow.Still;
                    }
                }
                break;
            case WaterFlow.Left:
                if (!flowLeft() && !flowRight())
                {
                    _waterFlow = WaterFlow.Still;
                }
                break;

            case WaterFlow.Right:
                if (!flowRight() && !flowLeft())
                {
                    _waterFlow = WaterFlow.Still;
                }
                break;
        }
    }

    /// Async coroutine to animate the drop moving.
    private void MoveWater(Vector3 direction)
    {
        //this.isMoving = true;
        Tile destinationTile;
        switch (_waterFlow)
        {
            case WaterFlow.Down:
                destinationTile = tile.underTile;
                break;
            case WaterFlow.Right:
                destinationTile = tile.rightTile;
                break;
            case WaterFlow.Left:
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
