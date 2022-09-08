using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
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

        }
    }


}
