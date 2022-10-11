using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueIceBlock: Block {

    public BlueIceBlock(Particle particle): base(BlockType.BlueIce, particle) {
    }

    /// Cools water in a diamond pattern.
    public override void Tick() {
        void CoolAdjacentWater(Tile neighbor, int tempChange) { 
            if (neighbor == null || neighbor.particle == null) {
                return;
            }
            Particle p = neighbor.particle;
            if (p.block.GetType() != typeof(WaterBlock)) {
                return;
            }
            WaterBlock wb = (WaterBlock)p.block;
            wb.ChangeTemperature(tempChange, "BlueIce");
        }

        /// Cool distance 1.
        CoolAdjacentWater(particle.tile.upTile,    -3);
        CoolAdjacentWater(particle.tile.downTile,  -3);
        CoolAdjacentWater(particle.tile.leftTile,  -3);
        CoolAdjacentWater(particle.tile.rightTile, -3);

        /// Cool distance 2.
        CoolAdjacentWater(particle.tile.getRelativeTile(new Vector2(+2, +0)),  -2);
        CoolAdjacentWater(particle.tile.getRelativeTile(new Vector2(-2, +0)),  -2);
        CoolAdjacentWater(particle.tile.getRelativeTile(new Vector2(+0, +2)),  -2);
        CoolAdjacentWater(particle.tile.getRelativeTile(new Vector2(+0, -2)),  -2);

        CoolAdjacentWater(particle.tile.getRelativeTile(new Vector2(+1, +1)),  -2);
        CoolAdjacentWater(particle.tile.getRelativeTile(new Vector2(+1, -1)),  -2);
        CoolAdjacentWater(particle.tile.getRelativeTile(new Vector2(-1, +1)),  -2);
        CoolAdjacentWater(particle.tile.getRelativeTile(new Vector2(-1, -1)),  -2);

        /// Cool distance 3.
        CoolAdjacentWater(particle.tile.getRelativeTile(new Vector2(+3, +0)),  -1);
        CoolAdjacentWater(particle.tile.getRelativeTile(new Vector2(-3, +0)),  -1);
        CoolAdjacentWater(particle.tile.getRelativeTile(new Vector2(+0, +3)),  -1);
        CoolAdjacentWater(particle.tile.getRelativeTile(new Vector2(+0, -3)),  -1);

        CoolAdjacentWater(particle.tile.getRelativeTile(new Vector2(+2, +1)),  -1);
        CoolAdjacentWater(particle.tile.getRelativeTile(new Vector2(+2, -1)),  -1);
        CoolAdjacentWater(particle.tile.getRelativeTile(new Vector2(-2, +1)),  -1);
        CoolAdjacentWater(particle.tile.getRelativeTile(new Vector2(-2, -1)),  -1);

        CoolAdjacentWater(particle.tile.getRelativeTile(new Vector2(+1, +2)),  -1);
        CoolAdjacentWater(particle.tile.getRelativeTile(new Vector2(+1, -2)),  -1);
        CoolAdjacentWater(particle.tile.getRelativeTile(new Vector2(-1, +2)),  -1);
        CoolAdjacentWater(particle.tile.getRelativeTile(new Vector2(-1, -2)),  -1);
    }
}