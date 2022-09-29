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
    
    // Whether the laser is currently being fired.
    private bool laserIsFiring = false;
    
    private IEnumerator coroutineForDestoryLaser;
    private float waitTime = 0.1f;
    
    void Start() {
        laserRenderer = GetComponent<LineRenderer>();
    }

    void Update() {
        /// `GetKey` instead of `GetKeyDown` allows continuous firing.
        if (Input.GetKey("space") && !laserIsFiring) {
            laserIsFiring = true;
            DrawLaser();
            coroutineForDestoryLaser = WaitAndDisappear(waitTime);
            StartCoroutine(coroutineForDestoryLaser);
        }
    }

    // Stop rendering laser after set time.
    private IEnumerator WaitAndDisappear(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        laserRenderer.positionCount = 0;
        laserIsFiring = false;
    }
    
    void DrawLaser() {
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
            if (blockType == BlockType.Water) {
                particle.HeatWater(Particle.TempLaser);
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
