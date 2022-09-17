using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType { 
    Water,   // Water block.
    Bedrock, // Uneditable level structure block.
    Dirt,    // Player placable block.
}

public class Particle {

    public BlockType blockType;
    public Tile tile;

    public Particle(BlockType type) {
        blockType = type;
    }

    public void Tick(GridManager grid) { 
        if (blockType == BlockType.Water) {
            WaterTick(grid);
        }
    }

    private void WaterTick(GridManager grid) { 
        // Check if water can flow down.
        if (tile.underTile != null && tile.underTile.particle == null) {
            Tile oldTile = this.tile;
            tile.underTile.SetParticle(this);
            oldTile.SetParticle(null);
        }

    }
}
