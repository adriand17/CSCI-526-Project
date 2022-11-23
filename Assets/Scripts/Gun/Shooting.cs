using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlockTypeExtension;
using Death;

public class Shooting : MonoBehaviour {

    private static readonly string FireKey = "space";

    private struct LineGroup {
        public LineGroup(Vector3 pos, Vector3 direction)
        {
            start = pos;
            dir = direction;
        }

        public Vector3 start;
        public Vector3 dir;
    }

    public Transform rightpivot;
    public Transform firepoint;
    [SerializeField] private GameObject _childLinePrefab;

    // Renders line from gun to target.
    private LineRenderer laserRenderer;
        
    /// How often to apply heat.
    private static float laserHeatInterval = 0.1f;
    private float timeSinceHeat = 0f;

    public static int TempLaser = +2;
    private LaserStatus laserStatus = new LaserStatus();
    private List<GameObject> childLines = new List<GameObject>();
    private List<LineGroup> childLineStartPositions = new List<LineGroup>();

    
    public void setLaserStatus(LaserStatus status) {
        this.laserStatus = status;
    }

    public LaserStatus getLaserStatus()
    {
        return laserStatus;
    }

    void Start() {
        laserRenderer = GetComponent<LineRenderer>();
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
            removeChildLine();            
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
        childLineStartPositions = new List<LineGroup>();
        // Start at the gun.
        Vector3 raycastDirection = firepoint.up;
        Vector3 raycastStart = firepoint.position;
        positions.Add(raycastStart);
        bool isPortalEntry = false;

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

            //Avoid z position lost.
            positions.Add(new Vector3(hit.point.x, hit.point.y, raycastStart.z));
            BlockType blockType = particle.getBlockType();
            bool breakLoop = false;
            switch (blockType) {
                case BlockType.Water:
                    WaterBlock waterBlock = (WaterBlock)particle.block;
                    if (ChangeTemperature) {
                        waterBlock.ChangeTemperature(TempLaser, Cause.Laser);
                    }
                    breakLoop = true;
                    break;

                case BlockType.TNT:
                    TNTBlock tntBlock = (TNTBlock)particle.block;
                    tntBlock.StartCountdown();
                    breakLoop = true;
                    break;
                
                case BlockType.RainTrigger:
                    RainTriggerBlock rtBlock = (RainTriggerBlock)particle.block;
                    rtBlock.SpawnWater();
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
                    Vector3 start = hit.point + (hit.normal.normalized * 0.001f);
                    //Avoid z position lost.
                    start.z = raycastStart.z;
                    raycastStart = start;
                    break;
                case BlockType.PortalEntry:
                    Debug.Log("BlockType.PortalEntry");
                    isPortalEntry = true;
                    var allPosition = particle.getAllPortalPosition();
                    //We don't destroy child line every time, only when if we need and we will create it.
                    while (childLines.Count < allPosition.Count) {
                        addChildLine();
                    }
                    foreach (Vector3 pos in allPosition) {
                        Vector3 childStart = new Vector3(pos.x, pos.y, raycastStart.z);
                        var lineInfo = new LineGroup(childStart, raycastDirection.normalized);
                        childLineStartPositions.Add(lineInfo);

                    }
                    breakLoop = true;
                    break;
                case BlockType.PortalExit:
                    //Debug.Log(hit.collider.gameObject);
                    breakLoop = true;
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

        if (isPortalEntry && childLineStartPositions.Count != 0) {
            DrawChildLaser(ChangeTemperature);
        } else {
            removeChildLine();
        }
    }

    void DrawChildLaser(bool ChangeTemperature) {
        // List of positions for line renderer to draw.
        for (int i = 0;i < childLineStartPositions.Count;i++){
            DrawChildLaserImp(childLines[i], childLineStartPositions[i], ChangeTemperature);
        }
        

    }

    void DrawChildLaserImp(GameObject line, LineGroup lineInfo, bool ChangeTemperature) { 
        List<Vector3> positions = new List<Vector3>();

        Vector3 raycastDirection = lineInfo.dir;
        Vector3 raycastStart = lineInfo.start;
        positions.Add(raycastStart);
            Debug.Log(lineInfo.start + " " + lineInfo.dir);



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
                
                if (_bt != BlockType.PortalExit && _bt.isOpaqueToLaser()) {
                    Debug.Log("HAHAHA");
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

            //Avoid z position lost.
            positions.Add(new Vector3(hit.point.x, hit.point.y, raycastStart.z));
            BlockType blockType = particle.getBlockType();
            bool breakLoop = false;
            switch (blockType) {
                case BlockType.Water:
                    WaterBlock waterBlock = (WaterBlock)particle.block;
                    if (ChangeTemperature) {
                        waterBlock.ChangeTemperature(TempLaser, Cause.Laser);
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
                    Vector3 start = hit.point + (hit.normal.normalized * 0.001f);
                    //Avoid z position lost.
                    start.z = raycastStart.z;
                    raycastStart = start;
                    break;
                case BlockType.PortalEntry:
                    breakLoop = true;
                    break;
                case BlockType.PortalExit:
                    positions.Add(targetPosition);
                    breakLoop = true;
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

        var renderer = line.GetComponent<LineRenderer>();
        Debug.Log("child.PortalEntry" + positions.Count + line);

        renderer.positionCount = positions.Count;
        renderer.SetPositions(positions.ToArray());


    }


    void removeChildLine() {
        childLineStartPositions.Clear();
        foreach(var child in childLines) {
            var renderer = child.GetComponent<LineRenderer>();

            renderer.positionCount = 0;
        }
    }

    void addChildLine() {
        var line = Instantiate(_childLinePrefab, transform);
        childLines.Add(line);
    }
}
