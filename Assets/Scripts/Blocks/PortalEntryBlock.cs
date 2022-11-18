using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEntryBlock: Block {

    private GridManager _gridManager;
    public PortalEntryBlock(Particle particle, GridManager gridManger): base(BlockType.PortalEntry, particle) {
    }

    public override void Tick() {
        // Do nothing.
    }
}