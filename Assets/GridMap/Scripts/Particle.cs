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

    public Particle(BlockType type) {
        blockType = type;
    }

    public void Tick(GridManager grid) { 
        
    }
}
