using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{

    
    [SerializeField] public GridManager gridManager;

    public static BuildingManager Instance;
    private List<ScriptableBuilding> _buildings;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        _buildings = new List<ScriptableBuilding>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //grid.SetValue(Grid.GetMouseWorldPosition(), 56);
            //var spawnedTower = Instantiate(_towerPrefab);
            //Tile selectedTile =  gridManager.GetTileAtPosition(Input.mousePosition.x, Input.mousePosition.y);
            //selectedTile.SetBuilding(spawnedTower); 

        }
    }


    public void AddBuildingToList(BaseTower tower)
    {
       // _buildings.Add(tower);
    }


}
