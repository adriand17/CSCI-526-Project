using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private WaterDrop _dropPrefab;
    [SerializeField] private BaseTower _towerPrefab;
    [SerializeField] private Grid grid;
    [SerializeField] private Transform _camera;
    [SerializeField] private BuildingManager buildingManager;


    private Dictionary<Vector2, Tile> _tiles;
    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateGrid()
    {

        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                //Debug.Log(x + " " + y);
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                //for checker board patter...
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset, _towerPrefab);

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        SetAdjacentTiles();
        SpawnWaterDrop();

        _camera.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }


    public void SetAdjacentTiles()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector2 pos = new Vector2(x, y);
                Tile t = _tiles[pos];

                //null represent the border for now.
                Tile underTile = null;
                Tile rightTile = null;
                Tile leftTile = null;
                if (y > 0)
                {
                    underTile = _tiles[new Vector2(x, y - 1)];
                }

                if(x > 0)
                {
                    leftTile = _tiles[new Vector2(x -1, y)];
                }

                if (x < _width - 1)
                {
                    rightTile = _tiles[new Vector2(x + 1, y)];
                }
                //if(x)
                t.setAdjacentTiles(underTile, leftTile, rightTile);

            }
        }
    }

    public void SpawnWaterDrop()
    {
        var spawnedDrop = Instantiate(_dropPrefab, new Vector3(5, 8, -1), Quaternion.identity);
        spawnedDrop.Init(GetTileAtPosition(5, 8));
        //for checker board patter...
       // spawnedTile.Init(isOffset, _towerPrefab);
    }


    public Tile GetTileAtPosition(float x, float y)
    {
        Debug.Log(x + " " + y);
        Vector2 pos = new Vector2(x, y);
        if (_tiles.TryGetValue(pos, out var tile))
        {
            Debug.Log("return tile");
            return tile;
        }

        return null;
    }
  
}
