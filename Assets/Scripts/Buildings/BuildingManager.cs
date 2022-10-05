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
    }


    public void AddBuildingToList(BaseTower tower)
    {
       // _buildings.Add(tower);
    }


}
