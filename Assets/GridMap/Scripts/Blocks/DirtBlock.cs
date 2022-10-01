using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtBlock: Block {

    private static int DirtMaxDurability = 5;
    private int dirtDurability = DirtMaxDurability;

    public DirtBlock(Particle particle): base(BlockType.Dirt, particle) {
        this.dirtDurability = DirtMaxDurability;
    }

    public override void Tick() {
        bool upIsWater = particle.tile.upTile != null && particle.tile.upTile.particle != null && particle.tile.upTile.particle.getBlockType() == BlockType.Water;
        bool leftIsWater = particle.tile.leftTile != null && particle.tile.leftTile.particle != null && particle.tile.leftTile.particle.getBlockType() == BlockType.Water;
        bool rightIsWater = particle.tile.rightTile != null && particle.tile.rightTile.particle != null && particle.tile.rightTile.particle.getBlockType() == BlockType.Water;

        if (upIsWater || leftIsWater || rightIsWater) { 
            dirtDurability -= 1;
        }
        
        /// Swap in a broken sprite.
        switch (dirtDurability) { 
            case 5:
                particle._renderer.sprite = Resources.Load<Sprite>("Dirt");
                break;
            
            case 4:
                particle._renderer.sprite = Resources.Load<Sprite>("Dirt Break 1");
                break;
            
            case 3:
                particle._renderer.sprite = Resources.Load<Sprite>("Dirt Break 2");
                break;
            
            case 2:
                particle._renderer.sprite = Resources.Load<Sprite>("Dirt Break 3");
                break;
            
            case 1: 
                particle._renderer.sprite = Resources.Load<Sprite>("Dirt Break 4");
                break;
        }
        
        if (dirtDurability <= 0) {    
            particle.DeleteParticle();
        }
    }
}