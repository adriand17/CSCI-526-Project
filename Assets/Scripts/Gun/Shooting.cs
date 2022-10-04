using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlockTypeExtension;

public class Shooting : MonoBehaviour {

    public Transform rightpivot;
    public Transform firepoint;
    
    // Renders line from gun to target.
    private LineRenderer laserRenderer;
    
    // Furthest distance the laser can reach.
    private static int maxRange = 100;
    
    // Maximum number of times laser can bounce.
    private static int maxReflections = 3;
    
    /// How often to apply heat.
    private static float laserHeatInterval = 0.1f;
    private float timeSinceHeat = 0f;

    public static int TempLaser = -2;
    
    void Start() {
        laserRenderer = GetComponent<LineRenderer>();
    }

    void Update() {
        timeSinceHeat += Time.deltaTime;

        /// `GetKey` instead of `GetKeyDown` allows continuous firing.
        if (Input.GetKey("space")) {
            bool ChangeTemperature = false;
            if (timeSinceHeat > laserHeatInterval) {
                ChangeTemperature = true;
                timeSinceHeat = 0f;
                timeSinceHeat = 0f;
            }

            DrawLaser(ChangeTemperature);
        } else if (Input.GetKeyUp("space")) {
            laserRenderer.positionCount = 0;
        }
    }

    void DrawLaser(bool ChangeTemperature) {
        // List of positions for line renderer to draw.
        List<Vector3> positions = new List<Vector3>();
        
        // Start at the gun.
        Vector3 raycastDirection = firepoint.up;
        Vector3 raycastStart = firepoint.position;
        positions.Add(raycastStart);

        for (int i = 0; i < Shooting.maxReflections; i++) {
            // Find the first opaque object hit by the laser.
            RaycastHit2D[] hits = Physics2D.RaycastAll(raycastStart, raycastDirection, Shooting.maxRange);
            RaycastHit2D hit = Physics2D.Raycast(raycastStart, raycastDirection, Shooting.maxRange);
            foreach (var obj in hits) {
                // Find the hit particle's blocktype.
                Particle _p = obj.collider.gameObject.GetComponent<Particle>();
                if (_p == null) { 
                    continue;
                }
                BlockType _bt = _p.getBlockType();
                
                if (_bt.isOpaqueToLaser()) {
                    hit = obj;
                    break;
                }
            }
            if (hit.collider == null) {
                // Laser shoots off into space.
                positions.Add(raycastStart + (raycastDirection.normalized * Shooting.maxRange));
                break;
            }
            Particle particle = hit.collider.gameObject.GetComponent<Particle>();
            if (particle == null) { 
                // Laser shoots off into space.
                positions.Add(raycastStart + (raycastDirection.normalized * Shooting.maxRange));
                break;
            }

            positions.Add(hit.point);
            BlockType blockType = particle.getBlockType();
            bool breakLoop = false;
            switch (blockType) {
                case BlockType.Water:
                    WaterBlock waterBlock = (WaterBlock)particle.block;
                    if (ChangeTemperature) {
                        waterBlock.ChangeTemperature(TempLaser);
                    }
                    breakLoop = true;
                    break;

                case BlockType.TNT:
                    TNTBlock tntBlock = (TNTBlock)particle.block;
                    tntBlock.StartCountdown();
                    breakLoop = true;
                    break;
                
                case BlockType.Bedrock:
                case BlockType.Dirt:
                case BlockType.Magma:
                case BlockType.BlueIce:
                    // No reflections, stop here.
                    breakLoop = true;
                    break;
                
                case BlockType.Mirror:
                    // Change direction for next ray.
                    raycastDirection = Vector3.Reflect(raycastDirection, hit.normal);
                    
                    // Move slightly away from the wall to avoid re-colliding with it.
                    raycastStart = hit.point + (hit.normal.normalized * 0.001f);
                    
                    break;
                
                default:
                    Debug.Log("ERROR: Unknown block type");
                    breakLoop = true;
                    break;
            }
            if (breakLoop) {
                break;
            }
        }

        // Assign positions to line renderer.
        laserRenderer.positionCount = positions.Count;
        laserRenderer.SetPositions(positions.ToArray());
    }
}
