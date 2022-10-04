using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTBlock: Block {
    
    public TNTBlock(Particle particle): base(BlockType.TNT, particle) {
    }

    public override void Tick() {
        // Do nothing.
    }
}