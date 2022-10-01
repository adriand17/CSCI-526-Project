using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedrockBlock: Block {

    public BedrockBlock(Particle particle): base(BlockType.Bedrock, particle) {
    }

    public override void Tick() {
        // Do nothing.
    }
}