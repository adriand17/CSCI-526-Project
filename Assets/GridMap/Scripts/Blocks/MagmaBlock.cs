using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaterBlock: Block {

    public HeaterBlock(Particle particle): base(BlockType.Heater, particle) {
    }

    public override void Tick() {
        // Do nothing.
    }
}