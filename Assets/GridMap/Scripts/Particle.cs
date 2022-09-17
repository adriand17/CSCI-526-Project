using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType { 
    Water,   // Water block.
    Bedrock, // Uneditable level structure block.
    Dirt,    // Player placable block.
}

public enum WaterFlow { 
    Still, 
    Left,  
    Right, 
}

public class Particle {

    public BlockType blockType;
    public Tile tile;
    private WaterFlow _waterFlow;

    public Particle(BlockType type) {
        blockType = type;
    }

    public void Tick(GridManager grid) { 
        if (blockType == BlockType.Water) {
            WaterTick(grid);
        }
    }

    /// Try to move the water particle to the left.
    /// Returns true if the particle moved.
    private bool flowLeft() {
        if (tile.leftTile != null && tile.leftTile.particle == null) {
            Tile oldTile = this.tile;
            tile.leftTile.SetParticle(this);
            oldTile.SetParticle(null);
            this._waterFlow = WaterFlow.Left;
            return true;
        }
        return false;
    }

    /// Try to move the particle to the right.
    /// Returns true if the particle moved.
    private bool flowRight() {
        if (tile.rightTile != null && tile.rightTile.particle == null) {
            Tile oldTile = this.tile;
            tile.rightTile.SetParticle(this);
            oldTile.SetParticle(null);
            this._waterFlow = WaterFlow.Right;
            return true;
        }
        return false;
    }

    /// Try to move water.
    private void WaterTick(GridManager grid) { 
        // Check if water can flow down.
        if (tile.underTile != null && tile.underTile.particle == null) {
            Tile oldTile = this.tile;
            tile.underTile.SetParticle(this);
            oldTile.SetParticle(null);
            return;
        }

        switch (_waterFlow) {
            case WaterFlow.Still:
                if (Random.value >= 0.5) {
                    if (!flowLeft() && !flowRight()) {
                        _waterFlow = WaterFlow.Still;
                    }
                } else {
                    if (!flowRight() && !flowLeft()) {
                        _waterFlow = WaterFlow.Still;
                    }
                } 
                break;
            
            case WaterFlow.Left:
                if (!flowLeft() && !flowRight()) {
                    _waterFlow = WaterFlow.Still;
                }
                break;
            
            case WaterFlow.Right:
                if (!flowRight() && !flowLeft()) {
                    _waterFlow = WaterFlow.Still;
                }
                break;
        }
    }
}
