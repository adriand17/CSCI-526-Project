using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaporationBlock: Block {

    /// How much this heats surrounding blocks.
    public static int TempChange = +3;

    public EvaporationBlock(Particle particle): base(BlockType.Magma, particle) {
    }

    public override void Tick() {
        void ConvertWatertoVapor(Tile neighbor, int tempChange) { 
            if (neighbor == null || neighbor.particle == null) {
                return;
            }
            Particle p = neighbor.particle;
            if (p.block.GetType() != typeof(WaterBlock)) {
                return;
            }
            WaterBlock wb = (WaterBlock)p.block;
            wb.ChangeTemperature(tempChange, "Magma");
        }

        /// Heat up adjacent water.
        ConvertWatertoVapor(particle.tile.upTile,    +3);
        ConvertWatertoVapor(particle.tile.downTile,  +3);
        ConvertWatertoVapor(particle.tile.leftTile,  +3);
        ConvertWatertoVapor(particle.tile.rightTile, +3);

    }
}