using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{

    public GridManager _gridManager;


    [SerializeField] Tower _laserTowerPf;
    public static TowerManager Instance;
    private List<Tower> _towers;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _towers = new List<Tower>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void BuildTowerOnTile(Tile t)
    {
        //Instantiate new tower and add to list
        var tower = Instantiate(_laserTowerPf, new Vector3(t.location.x, t.location.y,-2), Quaternion.identity);
        tower.Init(t, _gridManager, this);
        t.tower = tower;
        _towers.Add(tower);
    }

    public void RemoveTower(Tower tower)
    {

    }

    public void RemoveTowerFromTile(Tile tile)
    {
        Tower t = tile.tower;
        Destroy(t.gameObject);
        _towers.Remove(t);
        tile.tower = null;
    }

    public int GetTowerCount()
    {
        return _towers.Count;
    }
}
