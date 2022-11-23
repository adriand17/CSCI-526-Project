using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Death;

public class MagmaBlock: Block {

    /// How much this heats surrounding blocks.
    public static int TempChange = +3;

    public MagmaBlock(Particle particle): base(BlockType.Magma, particle) {
    }

    public override void Tick() {
        void HeatAdjacentWater(Tile neighbor, int tempChange) { 
            if (neighbor == null || neighbor.particle == null) {
                return;
            }
            Particle p = neighbor.particle;
            if (p.block.GetType() != typeof(WaterBlock)) {
                return;
            }
            WaterBlock wb = (WaterBlock)p.block;
            wb.ChangeTemperature(tempChange, Cause.Heating);
        }

        /// Heat up adjacent water.
        HeatAdjacentWater(particle.tile.upTile,    +3);
        HeatAdjacentWater(particle.tile.downTile,  +3);
        HeatAdjacentWater(particle.tile.leftTile,  +3);
        HeatAdjacentWater(particle.tile.rightTile, +3);

        /// Heat corners.
        HeatAdjacentWater(particle.tile.getRelativeTile(new Vector2(+1, +1)),  +2);
        HeatAdjacentWater(particle.tile.getRelativeTile(new Vector2(+1, -1)),  +2);
        HeatAdjacentWater(particle.tile.getRelativeTile(new Vector2(-1, +1)),  +2);
        HeatAdjacentWater(particle.tile.getRelativeTile(new Vector2(-1, -1)),  +2);

        /// Heat further water.
        HeatAdjacentWater(particle.tile.getRelativeTile(new Vector2(+2, +2)),  +1);
        HeatAdjacentWater(particle.tile.getRelativeTile(new Vector2(+2, -2)),  +1);
        HeatAdjacentWater(particle.tile.getRelativeTile(new Vector2(-2, +2)),  +1);
        HeatAdjacentWater(particle.tile.getRelativeTile(new Vector2(-2, -2)),  +1);
        
        HeatAdjacentWater(particle.tile.getRelativeTile(new Vector2(+2, +1)),  +1);
        HeatAdjacentWater(particle.tile.getRelativeTile(new Vector2(+2, -1)),  +1);
        HeatAdjacentWater(particle.tile.getRelativeTile(new Vector2(-2, +1)),  +1);
        HeatAdjacentWater(particle.tile.getRelativeTile(new Vector2(-2, -1)),  +1);
        
        HeatAdjacentWater(particle.tile.getRelativeTile(new Vector2(+1, +2)),  +1);
        HeatAdjacentWater(particle.tile.getRelativeTile(new Vector2(+1, -2)),  +1);
        HeatAdjacentWater(particle.tile.getRelativeTile(new Vector2(-1, +2)),  +1);
        HeatAdjacentWater(particle.tile.getRelativeTile(new Vector2(-1, -2)),  +1);

        HeatAdjacentWater(particle.tile.getRelativeTile(new Vector2(+2,  0)),  +1);
        HeatAdjacentWater(particle.tile.getRelativeTile(new Vector2(-2, 0 )),  +1);
        HeatAdjacentWater(particle.tile.getRelativeTile(new Vector2( 0, +2)),  +1);
        HeatAdjacentWater(particle.tile.getRelativeTile(new Vector2( 0, -2)),  +1);
    }
}