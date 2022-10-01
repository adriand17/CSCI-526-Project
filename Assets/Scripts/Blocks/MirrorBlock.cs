using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBlock: Block {

    public MirrorBlock(Particle particle): base(BlockType.Mirror, particle) {
    }

    public override void Tick() {
        // Do nothing.
    }
}