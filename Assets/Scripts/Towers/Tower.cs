using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    private Vector3 projectileShootFromPosition;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Camera cam;
    public Vector3 towerPosition;
    private Tile towerTile;
    private int range;
    private List<Tile> inRangeTiles;
    private GridManager gridManager;
    private float shootTimerMax;
    private float shootTimer;


    public void Init(Tile t, GridManager gridManager)
    {
        range = 2;
        int rangeArrayLength = 2 * range + 1;
        towerTile = t;
        inRangeTiles = new List<Tile>();
        towerPosition = t.location;
        projectileShootFromPosition = transform.Find("ProjectileShootFromPosition").position;
        this.gridManager = gridManager;

        inRangeTiles = gridManager.GetTowerTiles(towerPosition, range);
        Debug.Log(inRangeTiles.Count);
    }
    // Start is called before the first frame update
    void Start()
    {

        
    }

    private void Awake()
    {
        shootTimerMax = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {

  
        

        shootTimer -= Time.deltaTime;

        if(shootTimer <= 0f)
        {
            shootTimer = shootTimerMax;
            CheckTilesInRange();
        }

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


    public void ShootWater(Tile t)
    {
        //Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        //pos.z = -1;
        
        ProjectileTowerLaser.Create(projectileShootFromPosition, t.location, projectile);
        gridManager.DestoryWateratTile(t);

        //destory the water
    }

  

}
