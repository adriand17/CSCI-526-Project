using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RainMakerBlock: Block {
    private GridManager _gridManager;
    public RainMakerBlock(Particle particle): base(BlockType.RainMaker, particle) {
        _gridManager = GameManager.Instance._gridManager;
    }

    public override void Tick() {
        
    }

    public void SpawnWater() {
        Tile tile = particle.tile.getRelativeTile(Vector2.down);
        if (tile != null && tile.particle == null) {
            _gridManager.DrawParticle(BlockType.Water, tile.location);
            _gridManager.waterCount++;
        }
    }
}