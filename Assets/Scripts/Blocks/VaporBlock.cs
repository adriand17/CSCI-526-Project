using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaporBlock: Block {

    /// Direction that the water is flowing.
    public WaterFlowDirection flowDirection { get; private set; }

    /// Temperature of water.
    public float temperature { get; set; }
    private static float tempFreeze = 0f;
    private static float tempMin = -2f;
    private static float tempVapor = 10f;
    private static float tempInit = 5f;
    
    public VaporBlock(Particle particle): base(BlockType.Vapor, particle) {
        this.temperature = tempInit;
        this.flowDirection = WaterFlowDirection.Still;
    }

    public override void Tick() {
        if (temperature >= tempFreeze) {
            VaporFlow();
        }
    }

    public void UpdateSprite() { 
        if (temperature < tempFreeze) { 
            /// Set sprite to ice.
            particle._renderer.sprite = Resources.Load<Sprite>("Ice");
            particle._renderer.color = Color.white;
        } else {
            /// Set sprite to water.
            particle._renderer.sprite = Resources.Load<Sprite>("Water");
            particle._renderer.color = Color.white;
        }
    }

    private void VaporFlow() { 
        void MoveWater(Vector3 direction) {
            Tile destinationTile;
            switch (flowDirection) {
                case WaterFlowDirection.Up:
                    destinationTile = particle.tile.upTile;
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

        /// Try to move the water particle to the left.
        /// Returns true if the particle moved.
        bool flowLeft() {
            if (particle.tile.leftTile != null && particle.tile.leftTile.particle == null && particle.tile.leftTile.tower == null) {
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
        bool flowRight() {
            if (particle.tile.rightTile != null && particle.tile.rightTile.particle == null && particle.tile.rightTile.tower == null) {
                this.flowDirection = WaterFlowDirection.Right;
                Tile oldTile = this.particle.tile;
                particle.tile.rightTile.SetParticle(this.particle);
                MoveWater(Vector3.right);
                oldTile.SetParticle(null);
                
                return true;
            }
            return false;
        }

        // Check if water can flow up
        if (particle.tile.upTile != null && particle.tile.upTile.particle == null && particle.tile.upTile.tower == null) {
            this.flowDirection = WaterFlowDirection.Up;
            Tile oldTile = this.particle.tile;
            particle.tile.upTile.SetParticle(this.particle);
            MoveWater(Vector3.up);
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
            
            case WaterFlowDirection.Up:
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

    public bool IsFrozen() {
        return temperature < tempFreeze;
    }
}
