using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CondensationBlock: Block {

    /// How much this heats surrounding blocks.
   // public static int TempChange = +3;

    private GridManager _gridManager;

    public CondensationBlock(Particle particle, GridManager gridManger): base(BlockType.Condensation, particle) {
        _gridManager = gridManger;
    }

    public override void Tick() {
        void ConvertVaporToWater(Tile neighbor) { 
            if (neighbor == null || neighbor.particle == null) {
                return;
            }
            Particle p = neighbor.particle;
            if (p.block.GetType() != typeof(VaporBlock)) {
                return;
            }
            _gridManager.ReplaceBlockAtTile(neighbor, BlockType.Water);
            
        }

        /// Convert adjacent water.
        ConvertVaporToWater(particle.tile.upTile);
        ConvertVaporToWater(particle.tile.downTile);
        ConvertVaporToWater(particle.tile.leftTile);
        ConvertVaporToWater(particle.tile.rightTile);

    }
}