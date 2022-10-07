using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtBlock: BreakableBlock {

    private static int DirtMaxDurability = 5;

    public DirtBlock(Particle particle): base(BlockType.Dirt, particle, DirtMaxDurability, "Dirt") {
    }

    public override void Tick() {
        base.Tick();
    }
}