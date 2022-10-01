using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlockTypeExtension;

public class Shooting : MonoBehaviour {

    private static readonly string FireKey = "space";

    public Transform rightpivot;
    public Transform firepoint;
    
    // Renders line from gun to target.
    private LineRenderer laserRenderer;
    
    /// How often to apply heat.
    private static float laserHeatInterval = 0.1f;
    private float timeSinceHeat = 0f;

    public static int TempLaser = -2;
    private LaserStatus laserStatus;
    
    void Start() {
        laserRenderer = GetComponent<LineRenderer>();
        laserStatus = new LaserStatus();
    }

    void Update() {
        timeSinceHeat += Time.deltaTime;

        /// `GetKey` instead of `GetKeyDown` allows continuous firing.
        if (Input.GetKey(Shooting.FireKey) && laserStatus.canFire()) {
            bool ChangeTemperature = false;
            if (timeSinceHeat > laserHeatInterval) {
                ChangeTemperature = true;
                timeSinceHeat = 0f;
                timeSinceHeat = 0f;
            }
            laserStatus.isFiring = true;
            DrawLaser(ChangeTemperature);
        } else if (Input.GetKeyUp(Shooting.FireKey)) {
            laserStatus.isFiring = false;
            laserRenderer.positionCount = 0;
        } 
        //If user keep press fire and no energy, do nothing.
        if (!laserStatus.canFire() && laserStatus.isFiring) {
            laserRenderer.positionCount = 0;
        } else {
            laserStatus.updateLaserEnergyLevel();
        }

    }

    void DrawLaser(bool ChangeTemperature) {
        // List of positions for line renderer to draw.
        List<Vector3> positions = new List<Vector3>();
        
        // Start at the gun.
        Vector3 raycastDirection = firepoint.up;
        Vector3 raycastStart = firepoint.position;
        positions.Add(raycastStart);

        for (int i = 0; i < laserStatus.getCurrentReflectLevel(); i++) {
            // Find the first opaque object hit by the laser.
            RaycastHit2D[] hits = Physics2D.RaycastAll(raycastStart, raycastDirection, laserStatus.maxRange());
            RaycastHit2D hit = Physics2D.Raycast(raycastStart, raycastDirection, laserStatus.maxRange());
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

            Vector3 targetPosition = raycastStart + (raycastDirection.normalized * laserStatus.maxRange());
            if (hit.collider == null) {
                // Laser shoots off into space.
                positions.Add(targetPosition);
                break;
            }
            Particle particle = hit.collider.gameObject.GetComponent<Particle>();
            if (particle == null) { 
                // Laser shoots off into space.
                positions.Add(targetPosition);
                break;
            }

            positions.Add(hit.point);
            BlockType blockType = particle.getBlockType();
            if (blockType == BlockType.Water) {
                WaterBlock waterBlock = (WaterBlock)particle.block;
                if (ChangeTemperature) {
                    waterBlock.ChangeTemperature(TempLaser);
                }
                
                break;
            } else if (blockType == BlockType.Bedrock || blockType == BlockType.Dirt) {
                // No reflections, stop here.
                break;
            } else if (blockType == BlockType.Mirror) {
                // Change direction for next ray.
                raycastDirection = Vector3.Reflect(raycastDirection, hit.normal);
                
                // Move slightly away from the wall to avoid re-colliding with it.
                raycastStart = hit.point + (hit.normal.normalized * 0.001f);
                
                continue;
            } else {
                Debug.Log("ERROR: Unknown block type");
                break;
            }
        }

        // Assign positions to line renderer.
        laserRenderer.positionCount = positions.Count;
        laserRenderer.SetPositions(positions.ToArray());
    }
}
