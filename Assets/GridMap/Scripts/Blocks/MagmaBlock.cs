using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaBlock: Block {

    /// How much this heats surrounding blocks.
    public static int TempChange = +3;

    public MagmaBlock(Particle particle): base(BlockType.Magma, particle) {
    }

    public override void Tick() {
        void HeatAdjacentWater(Tile neighbor) { 
            if (neighbor == null || neighbor.particle == null) {
                return;
            }
            Particle p = neighbor.particle;
            if (p.block.GetType() != typeof(WaterBlock)) {
                return;
            }
            WaterBlock wb = (WaterBlock)p.block;
            wb.HeatWater(MagmaBlock.TempChange);
        }

        /// Heat up adjacent water.
        HeatAdjacentWater(particle.tile.upTile);
        HeatAdjacentWater(particle.tile.downTile);
        HeatAdjacentWater(particle.tile.leftTile);
        HeatAdjacentWater(particle.tile.rightTile);
    }
}