using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    private Vector3 projectileShootFromPosition;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Camera cam;
    private Tile towerTile;
    private float range;
    private Tile[,] inRangeTiles = new Tile[5, 5];
    private GridManager gridManager;

    // Start is called before the first frame update
    void Start()
    {
        projectileShootFromPosition = transform.Find("ProjectileShootFromPosition").position;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            pos.z = -1;
            ProjectileTowerLaser.Create(projectileShootFromPosition, pos, projectile);
        }

    }

    public void CheckTilesInRange()
    {
        //look through inRangeTile
    }

   

    public void setTowerPosition()
    {
        float height = 2f * Camera.main.orthographicSize;
        Vector3 pos = cam.transform.position + new Vector3(0.0f, -height / 2f, 0f);
        pos.z = -5f;

        projectile.transform.position = pos;
    }
}
