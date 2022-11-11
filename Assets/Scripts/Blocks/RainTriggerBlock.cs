using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockTypeExtension;

public class RainTriggerBlock: Block
{

    private GridManager _gridManager;
    public RainTriggerBlock(Particle particle): base(BlockType.RainTrigger, particle) {
       _gridManager = GameManager.Instance._gridManager;
    }

    public override void Tick() {
        // Do nothing.
    }

    public void SpawnWater() {
        foreach (var rainMakerBlock in _gridManager._rainMakerBlocks)
        {
            rainMakerBlock.SpawnWater();
        }
    }

}