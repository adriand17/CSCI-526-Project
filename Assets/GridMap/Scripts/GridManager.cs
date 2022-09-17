using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
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

    private List<WaterDrop> dropList = new List<WaterDrop>();
    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
		// A correct website page.
        StartCoroutine(GetRequest("https://docs.google.com/forms/d/e/1FAIpQLSdH4rGRcgwsHFzd5gCYm-uOJ6yOjeC1HQWpnNTCZkM3o7l-BA/formResponse?usp=pp_url&entry.49243494=Yes&submit=Submit"));
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
                var isOffset = (x + y) % 2 == 1;
                spawnedTile.Init(isOffset, _towerPrefab, true, new Vector3(x, y), this);

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }


        SetUnpassableTiles();
        SetAdjacentTiles();
        
        SpawnWaterDrop();

        _camera.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }

    /// Provides each tile with a reference to its valid neighbors.
    public void SetAdjacentTiles()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector2 pos = new Vector2(x, y);
                Tile t = _tiles[pos];

                // Look up valid adjacent tiles.
                // null represents the world's borders.
                Tile underTile = (y > 0) 
                	? _tiles[new Vector2(x, y - 1)] 
                	: null;
                Tile rightTile = (x < _width - 1)
                	? _tiles[new Vector2(x + 1, y)]
                	: null;
                Tile leftTile = (x > 0)
                	? _tiles[new Vector2(x -1, y)]
                	: null;
                
                t.setAdjacentTiles(underTile, leftTile, rightTile);
            }
        }
    }

    // Create inital level geometry.
    public void SetUnpassableTiles()
    {

     
        Tile t = _tiles[new Vector3(5,6)];
        t.SetTileUnpassable();

        t = _tiles[new Vector3(4, 6)];
        t.SetTileUnpassable();

        t = _tiles[new Vector3(3, 6)];
        t.SetTileUnpassable();

        t = _tiles[new Vector3(2, 6)];
        t.SetTileUnpassable();

        t = _tiles[new Vector3(1, 6)];
        t.SetTileUnpassable();


        t = _tiles[new Vector3(6, 4)];
        t.SetTileUnpassable();

        t = _tiles[new Vector3(5, 4)];
        t.SetTileUnpassable();

        t = _tiles[new Vector3(4, 4)];
        t.SetTileUnpassable();

        t = _tiles[new Vector3(3, 4)];
        t.SetTileUnpassable();

        t = _tiles[new Vector3(2, 4)];
        t.SetTileUnpassable();

        t = _tiles[new Vector3(1, 4)];
        t.SetTileUnpassable();

        t = _tiles[new Vector3(0, 4)];
        t.SetTileUnpassable();


        t = _tiles[new Vector3(8, 3)];
        t.SetTileUnpassable();

        t = _tiles[new Vector3(7, 2)];
        t.SetTileUnpassable();

        t = _tiles[new Vector3(6, 2)];
        t.SetTileUnpassable();

        t = _tiles[new Vector3(5, 2)];
        t.SetTileUnpassable();

        t = _tiles[new Vector3(4, 2)];
        t.SetTileUnpassable();

        t = _tiles[new Vector3(3, 2)];
        t.SetTileUnpassable();

        t = _tiles[new Vector3(2, 2)];
        t.SetTileUnpassable();

        t = _tiles[new Vector3(1, 2)];
        t.SetTileUnpassable();


        t = _tiles[new Vector3(1, 1)];
        t.SetTileUnpassable();

        t = _tiles[new Vector3(1, 0)];
        t.SetTileUnpassable();

    }

    public void SpawnWaterDrop()
    {
        var spawnedDrop = Instantiate(_dropPrefab, new Vector3(5, 8, -1), Quaternion.identity);
        spawnedDrop.Init(GetTileAtPosition(5, 8));
        dropList.Add(spawnedDrop);

        spawnedDrop = Instantiate(_dropPrefab, new Vector3(7, 8, -1), Quaternion.identity);
        spawnedDrop.Init(GetTileAtPosition(7, 8));
        dropList.Add(spawnedDrop);

        spawnedDrop = Instantiate(_dropPrefab, new Vector3(10, 8, -1), Quaternion.identity);
        spawnedDrop.Init(GetTileAtPosition(10, 8));
        dropList.Add(spawnedDrop);

        spawnedDrop = Instantiate(_dropPrefab, new Vector3(13, 8, -1), Quaternion.identity);
        spawnedDrop.Init(GetTileAtPosition(13, 8));
        dropList.Add(spawnedDrop);

        spawnedDrop = Instantiate(_dropPrefab, new Vector3(1, 8, -1), Quaternion.identity);
        spawnedDrop.Init(GetTileAtPosition(1, 8));
        dropList.Add(spawnedDrop);
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

    public void ResetGrid()
    {
        foreach(WaterDrop drop in dropList)
        {
            DestroyImmediate(drop.gameObject);
        }
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector2 pos = new Vector2(x, y);
                Tile t = _tiles[pos];
                if (t._filledWater.activeSelf == false){
                    t._hasWater = false;
                }

            }
        }

                dropList.Clear();

        SpawnWaterDrop();
    }
 
	IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }

}
