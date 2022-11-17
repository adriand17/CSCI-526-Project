using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalExitBlock: Block {

    public PortalExitBlock(Particle particle, GridManager gridManger): base(BlockType.PortalExit, particle) {
    }

    public override void Tick() {
        // Do nothing.
    }
}