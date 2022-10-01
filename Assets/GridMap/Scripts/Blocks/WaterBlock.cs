using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBlock: Block {

    /// Direction that the water is flowing.
    private WaterFlowDirection flowDirection;

    /// Temperature of water.
    public float temperature { get; set; }
    private static int tempFreeze = 0;
    private static int tempVapor = 10;
    private static int tempInit = 5;
    public static int TempLaser = +2;

    public WaterBlock(Particle particle): base(BlockType.Water, particle) {
        this.temperature = tempInit;
        this.flowDirection = WaterFlowDirection.Still;
    }

    public override void Tick() {
        CoolWater();
        WaterFlow();
    }

    private void CoolWater() { 
        if (blockType != BlockType.Water) {
            Debug.LogError("Cool: non-water particle");
            return;
        }

        void TradeHeat(Tile neighbor) { 
            if (neighbor == null || neighbor.particle == null) {
                return;
            }
            Particle p = neighbor.particle;
            if (p.block.GetType() != typeof(WaterBlock)) {
                return;
            }
            WaterBlock wb = (WaterBlock)p.block;
            if (wb.temperature >= this.temperature) {
                return;
            }
            
            wb.temperature += 1;
            this.temperature -= 1;
            wb.ShowWaterHeat();
        }

        /// Share heat with neighbors.
        TradeHeat(particle.tile.upTile);
        TradeHeat(particle.tile.downTile);
        TradeHeat(particle.tile.leftTile);
        TradeHeat(particle.tile.rightTile);

        /// Cool off naturally.
        if (temperature > WaterBlock.tempInit) {
            temperature -= 1;
        }

        ShowWaterHeat();
    }

    public void HeatWater(int tempChange) {
        temperature += tempChange;
        if (temperature >= tempVapor) {
            particle.DeleteParticle();
            return;
        } else { 
            ShowWaterHeat();
        }
    }

    /// Try to move the water particle to the left.
    /// Returns true if the particle moved.
    private bool flowLeft() {
        if (particle.tile.leftTile != null && particle.tile.leftTile.particle == null) {
            this.flowDirection = WaterFlowDirection.Left;
            Tile oldTile = this.particle.tile;
            particle.tile.leftTile.SetParticle(this.particle);
            MoveWater(Vector3.left);
            oldTile.SetParticle(null);
            
            return true;
        }
        return false;
    }

    /// Try to move the particle to the right.
    /// Returns true if the particle moved.
    private bool flowRight() {
        if (particle.tile.rightTile != null && particle.tile.rightTile.particle == null) {
            this.flowDirection = WaterFlowDirection.Right;
            Tile oldTile = this.particle.tile;
            particle.tile.rightTile.SetParticle(this.particle);
            MoveWater(Vector3.right);
            oldTile.SetParticle(null);
            
            return true;
        }
        return false;
    }

    private void MoveWater(Vector3 direction) {
        Tile destinationTile;
        switch (flowDirection) {
            case WaterFlowDirection.Down:
                destinationTile = particle.tile.downTile;
                break;
            case WaterFlowDirection.Right:
                destinationTile = particle.tile.rightTile;
                break;
            case WaterFlowDirection.Left:
                destinationTile = particle.tile.leftTile;
                break;
            default:
                destinationTile = particle.tile;
                break;
        }
     
        this.particle.tile = destinationTile;

        particle.transform.position = new Vector3(destinationTile.transform.position.x, destinationTile.transform.position.y, -1);
    }

    private void ShowWaterHeat() { 
        /// Get redder based on temperature.
        float red = (float)(temperature - tempFreeze) / (float)(tempVapor - tempFreeze);
        red *= 0.75f; // Dampen effect.
        particle._renderer.color = new Color(red, 0, 1);
    }

    private void WaterFlow() { 
        // Check if water can flow down.
        if (particle.tile.downTile != null && particle.tile.downTile.particle == null) {
            this.flowDirection = WaterFlowDirection.Down;
            Tile oldTile = this.particle.tile;
            particle.tile.downTile.SetParticle(this.particle);
            MoveWater(Vector3.down);
            oldTile.SetParticle(null);
            return;
        }

        switch (flowDirection) {
            case WaterFlowDirection.Still:
                if (Random.value >= 0.5) {
                    if (!flowLeft() && !flowRight()) {
                        flowDirection = WaterFlowDirection.Still;
                    }
                } else {
                    if (!flowRight() && !flowLeft()) {
                        flowDirection = WaterFlowDirection.Still;
                    }
                }
                break;
            
            case WaterFlowDirection.Down:
                if (Random.value >= 0.5) {
                    if (!flowLeft() && !flowRight()) {
                        flowDirection = WaterFlowDirection.Still;
                    }
                } else {
                    if (!flowRight() && !flowLeft()) {
                        flowDirection = WaterFlowDirection.Still;
                    }
                }
                break;
            
            case WaterFlowDirection.Left:
                if (!flowLeft() && !flowRight()) {
                    flowDirection = WaterFlowDirection.Still;
                }
                break;

            case WaterFlowDirection.Right:
                if (!flowRight() && !flowLeft()) {
                    flowDirection = WaterFlowDirection.Still;
                }
                break;
        }
    }
}
