using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaBlock: Block {

    public MagmaBlock(Particle particle): base(BlockType.Magma, particle) {
    }

    public override void Tick() {
        // Do nothing.
    }
}