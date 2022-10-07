using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : Block {

    public int MaxDurability { get; }
    public int durability { get; private set; }

    public static float RecoveryTime = 3f * Particle.TickInterval;
    private float timeSinceDamaged = 0f;

    private string spriteName;

    public BreakableBlock(BlockType blockType, Particle particle, int maxDurability, string spriteName): base(blockType, particle) {
        this.MaxDurability = maxDurability;
        this.durability = maxDurability;
        this.spriteName = spriteName;
    }

    public override void Tick() {
        bool isWater(Tile neighbor) {
            if (neighbor == null || neighbor.particle == null) {
                return false;
            }
            if (neighbor.particle.getBlockType() != BlockType.Water) {
                return false;
            }
            
            WaterBlock waterBlock = (WaterBlock)neighbor.particle.block;
            if (waterBlock.flowDirection != WaterFlowDirection.Still) {
                return false;
            }
            if (waterBlock.IsFrozen()) {
                return false;
            }
            return true;
        }

        bool upIsWater = isWater(particle.tile.upTile);
        bool leftIsWater = isWater(particle.tile.leftTile);
        bool rightIsWater = isWater(particle.tile.rightTile);

        if (upIsWater || leftIsWater || rightIsWater) { 
            TakeDamage(1);
        } else { 
            timeSinceDamaged += Particle.TickInterval;
            if (timeSinceDamaged > RecoveryTime) {
                durability = MaxDurability;
                particle._renderer.sprite = Resources.Load<Sprite>(spriteName);
            }
        }
    }

    private void TakeDamage(int damage) {
        durability -= damage;

        /// Swap in a broken sprite.
        switch (durability) { 
            case 5:
                particle._renderer.sprite = Resources.Load<Sprite>(spriteName);
                break;
            
            case 4:
                particle._renderer.sprite = Resources.Load<Sprite>(spriteName + " Break 1");
                break;
            
            case 3:
                particle._renderer.sprite = Resources.Load<Sprite>(spriteName + " Break 2");
                break;
            
            case 2:
                particle._renderer.sprite = Resources.Load<Sprite>(spriteName + " Break 3");
                break;
            
            case 1: 
                particle._renderer.sprite = Resources.Load<Sprite>(spriteName + " Break 4");
                break;
        }

        if (durability <= 0) {    
            particle.DeleteParticle();
        }
    }
}
