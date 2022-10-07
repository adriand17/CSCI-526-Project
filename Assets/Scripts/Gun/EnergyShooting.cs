using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlockTypeExtension;

public class EnergyShooting : MonoBehaviour
{

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

    /// How often to apply heat.
    private static float laserEnergyInterval = 0.1f;
    private float timeSinceEnergy = 0f;

    public static int EnergyLaser = 2;

    void Start()
    {
        laserRenderer = GetComponent<LineRenderer>();
        Color c = new Color(0, 1, 0);
        laserRenderer.startColor = c;
        laserRenderer.endColor = c;
    }

    void Update()
    {
        timeSinceEnergy += Time.deltaTime;

        /// `GetKey` instead of `GetKeyDown` allows continuous firing.
        if (Input.GetKey("space"))
        {

            Debug.Log("Shooting energy");
            bool ChangeEnergy = false;
            if (timeSinceEnergy > laserEnergyInterval)
            {
                ChangeEnergy = true;
                timeSinceEnergy = 0f;
            }

            DrawLaser(ChangeEnergy);
        }
        else if (Input.GetKeyUp("space"))
        {
            laserRenderer.positionCount = 0;
        }
    }

    void DrawLaser(bool ChangeEnergy)
    {
        // List of positions for line renderer to draw.
        List<Vector3> positions = new List<Vector3>();

        // Start at the gun.
        Vector3 raycastDirection = firepoint.up;
        Vector3 raycastStart = firepoint.position;
        positions.Add(raycastStart);

        for (int i = 0; i < EnergyShooting.maxReflections; i++)
        {
            // Find the first opaque object hit by the laser.
            RaycastHit2D[] hits = Physics2D.RaycastAll(raycastStart, raycastDirection, EnergyShooting.maxRange);
            RaycastHit2D hit = Physics2D.Raycast(raycastStart, raycastDirection, EnergyShooting.maxRange);
            foreach (var obj in hits)
            {
                // Find the hit tower.
                //Particle _p = obj.collider.gameObject.GetComponent<Particle>();
                Tower _t = obj.collider.gameObject.GetComponent<Tower>();
                if (_t != null)
                {
                    hit = obj;
                    break;
                }
             
            }
            if (hit.collider == null)
            {
                // Laser shoots off into space.
                positions.Add(raycastStart + (raycastDirection.normalized * EnergyShooting.maxRange));
                break;
            }
            Tower tower = hit.collider.gameObject.GetComponent<Tower>();
            if (tower == null)
            {
                // Laser shoots off into space.
                positions.Add(raycastStart + (raycastDirection.normalized * EnergyShooting.maxRange));
                break;
            }

            positions.Add(hit.point);
            if (tower != null)
            {
                //WaterBlock waterBlock = (WaterBlock)particle.block;
                Debug.Log("Hit an tower");
                if (ChangeEnergy)
                {
                    //waterBlock.ChangeTemperature(EnergyLaser);
                    tower.IncreaseEnergy(EnergyLaser);

                }

                break;
            }
           /* if (blockType == BlockType.Mirror)
            {
                // Change direction for next ray.
                raycastDirection = Vector3.Reflect(raycastDirection, hit.normal);

                // Move slightly away from the wall to avoid re-colliding with it.
                raycastStart = hit.point + (hit.normal.normalized * 0.001f);

                continue;
            }*//* if (blockType == BlockType.Mirror)
            {
                // Change direction for next ray.
                raycastDirection = Vector3.Reflect(raycastDirection, hit.normal);

                // Move slightly away from the wall to avoid re-colliding with it.
                raycastStart = hit.point + (hit.normal.normalized * 0.001f);

                continue;
            }*/
            else
            {
                Debug.Log("ERROR: Unknown block type");
                break;
            }
        }

        // Assign positions to line renderer.
        laserRenderer.positionCount = positions.Count;
        laserRenderer.SetPositions(positions.ToArray());
    }
}
