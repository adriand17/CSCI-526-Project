using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaporationBlock: Block {

    /// How much this heats surrounding blocks.
   // public static int TempChange = +3;

    private GridManager _gridManager;

    public EvaporationBlock(Particle particle, GridManager gridManger): base(BlockType.Evaporator, particle) {
        _gridManager = gridManger;
    }

    public override void Tick() {
        void ConvertWatertoVapor(Tile neighbor) { 
            if (neighbor == null || neighbor.particle == null) {
                return;
            }
            Particle p = neighbor.particle;
            if (p.block.GetType() != typeof(WaterBlock)) {
                return;
            }
            WaterBlock wb = (WaterBlock)p.block;
            _gridManager.ReplaceBlockAtTile(neighbor, BlockType.Vapor);
            
        }

        /// Convert adjacent water.
        ConvertWatertoVapor(particle.tile.upTile);
        ConvertWatertoVapor(particle.tile.downTile);
        ConvertWatertoVapor(particle.tile.leftTile);
        ConvertWatertoVapor(particle.tile.rightTile);

    }
}