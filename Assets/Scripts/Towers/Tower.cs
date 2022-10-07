using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    private Vector3 projectileShootFromPosition;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Camera cam;
    [SerializeField] public TowerEnergyBar eb;
    

    public Vector3 towerPosition;
    public bool inEnergizedState = false;
    private Tile towerTile;
    private int range;
    private List<Tile> inRangeTiles;
    private GridManager gridManager;
    private TowerManager towerManager;
    private float energizedTimerMax;
    private float engergizedTimer;
    private float shootTimerMax;
    private float shootTimer;
    private float energyMax = 100f;
    private float energy = 0f;
    


    public void Init(Tile t, GridManager gridManager, TowerManager towerMan)
    {
        range = 2;
        int rangeArrayLength = 2 * range + 1;
        towerTile = t;
        inRangeTiles = new List<Tile>();
        towerPosition = t.location;
        projectileShootFromPosition = transform.Find("ProjectileShootFromPosition").position;
        this.gridManager = gridManager;
        this.towerManager = towerMan;

        inRangeTiles = gridManager.GetTowerTiles(towerPosition, range);
    }
    // Start is called before the first frame update
    void Start()
    {

        
    }

    private void Awake()
    {
        shootTimerMax = 2f;
        energizedTimerMax = 12f;
        engergizedTimer = energizedTimerMax;
    }

    // Update is called once per frame
    void Update()
    {



        if (inEnergizedState)
        {

            engergizedTimer -= Time.deltaTime;
            if (engergizedTimer <= 0f)
            {
                engergizedTimer = energizedTimerMax;
                inEnergizedState = false;
                shootTimerMax = 2f;
                energy = 0;
            }
        }
      
        shootTimer -= Time.deltaTime;

        if(shootTimer <= 0f)
        {
            shootTimer = shootTimerMax;
            CheckTilesInRange();
        }

    }


    public void OnMouseDown()
    {
        Debug.Log("clicked on tower");
        towerManager.RemoveTowerFromTile(towerTile);
    }





    public void CheckTilesInRange()
    {
        foreach(var tile in inRangeTiles)
        {
            if(tile.particle != null && tile.particle.getBlockType() == BlockType.Water)
            {
                ShootWater(tile);
                break;
            }
        }
      
   
    }

    public float GetEnergy()
    {
        return this.energy;
    }


    public float GetEnergyPercentage()
    {
        return energy / energyMax;
    }


    public void UseEnergy(float used)
    {
        if (energy < 0)
        {
            energy = 0;
        }

    }

    public void IncreaseEnergy(float laser_energy)
    {
        this.energy += laser_energy;
        if(energy >= energyMax)
        {
            energy = energyMax;
        }

        if(energy == energyMax)
        {
            inEnergizedState = true;
            shootTimerMax = 0.5f;
        }
    }


    public void ShootWater(Tile t)
    {

        ProjectileTowerLaser.Create(projectileShootFromPosition, t.location, projectile);
        gridManager.DestoryWateratTile(t);

    }

  

}
