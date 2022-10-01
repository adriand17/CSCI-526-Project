using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block {
    /// Kind of block this particle is.
    public BlockType blockType { get; }

    /// Parent particle.
    public Particle particle { get; }

    public Block(BlockType blockType, Particle particle) {
        this.blockType = blockType;
        this.particle = particle;
    }

    /// Override to perform updates.
    public abstract void Tick();
}

public class GlassBlock: Block {
    
    public GlassBlock(Particle particle): base(BlockType.Glass, particle) {
    }

    public override void Tick() {
        // Do nothing.
    }
}

public class CoolerBlock: Block {

    public CoolerBlock(Particle particle): base(BlockType.Cooler, particle) {
    }

    public override void Tick() {
        // Do nothing.
    }
}