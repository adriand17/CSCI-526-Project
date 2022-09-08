using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private bool _isBuildable;
    [SerializeField] public BaseTower OccupiedTower;

    //public BaseTower OccupiedTower;

    private bool Occupied = false;
    //public bool Buildable => _isBuildable && OccupiedTower == null;
    public bool Buildable => _isBuildable && Occupied == false;

    // Start is called before the first frame update

    public void Init(bool isOffset)
    {
        _isBuildable = false;
        _renderer.color = _baseColor;
    }

    private void Update()
    {
        if (Buildable)
        {
            _renderer.color = Color.green;
            _isBuildable = true;
        }
        else
        {
        }
    }


    private void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        _highlight.SetActive(false);
    }

    private void OnMouseDown()
    {

        if (_isBuildable)
        {
            
            _renderer.color = Color.red;
            Occupied = true;
        }
        
    }


    public void SetBuilding(BaseTower tower)
    {
        if (tower.OccupiedTile != null) tower.OccupiedTile.OccupiedTower = null;
        tower.transform.position = transform.position;
        this.OccupiedTower = tower;
        tower.OccupiedTile = this;


    }

}
