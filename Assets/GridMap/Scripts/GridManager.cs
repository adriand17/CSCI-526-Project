using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private BaseTower _towerPrefab;
    [SerializeField] private Grid grid;
    [SerializeField] private Transform _camera;
    [SerializeField] private BuildingManager buildingManager;


    private Dictionary<Vector2, Tile> _tiles;
    private float waterInterval;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
		// A correct website page.
        StartCoroutine(GetRequest("https://docs.google.com/forms/d/e/1FAIpQLSdH4rGRcgwsHFzd5gCYm-uOJ6yOjeC1HQWpnNTCZkM3o7l-BA/formResponse?usp=pp_url&entry.49243494=Yes&submit=Submit"));
    }

    // Update is called once per frame
    void Update() {
        /// Wait for 0.5s.
        waterInterval += Time.deltaTime;
        if (waterInterval < 0.25f) {
            return;
        }
        waterInterval = 0;

        Dictionary<Vector2, Tile.BlockID> blockMap = new Dictionary<Vector2, Tile.BlockID>();
        for (int y = 0; y < _height; y++) {
            for (int x = 0; x < _width; x++) {
                Tile.BlockID blockID = _tiles[new Vector2(x, y)].blockID;
                blockMap.Add(new Vector2(x, y), blockID);
            }
        }

        /// Clone the grid, so it can be processed row by row.
        Dictionary<Vector2, Tile.BlockID> mapClone = new Dictionary<Vector2, Tile.BlockID>(blockMap);
        
        bool flowLeft(int x, int y) {
            Vector2 here = new Vector2(x, y);
            Vector2 left = new Vector2(x - 1, y);
            if (mapClone.ContainsKey(left) && mapClone[left] == Tile.BlockID.Air && blockMap[left] == Tile.BlockID.Air) {
                blockMap[left] = Tile.BlockID.WaterLeft;
                blockMap[here] = Tile.BlockID.Air;
                return true;
            }
            return false;
        }

        bool flowRight(int x, int y) {
            Vector2 here = new Vector2(x, y);
            Vector2 right = new Vector2(x + 1, y);
            if (mapClone.ContainsKey(right) && mapClone[right] == Tile.BlockID.Air && blockMap[right] == Tile.BlockID.Air) {
                blockMap[right] = Tile.BlockID.WaterRight;
                blockMap[here] = Tile.BlockID.Air;
                return true;
            }
            return false;
        }

        for (int y = 0; y < _height; y++) {
            int x, threshold, step;
            if (Random.value >= 0.5) {
                x = 0;
                threshold = _width;
                step = 1;
            } else {
                x = _width - 1;
                threshold = -1;
                step = -1;
            }
            for (; x != threshold; x += step) {
                Vector2 here = new Vector2(x, y);
                Vector2 down = new Vector2(x, y - 1);
                Vector2 left = new Vector2(x - 1, y);
                Vector2 right = new Vector2(x + 1, y);
                
                /// Ensure block is water of some kind.
                Tile tile = _tiles[here];
                if (!(tile.blockID == Tile.BlockID.WaterStill || tile.blockID == Tile.BlockID.WaterLeft || tile.blockID == Tile.BlockID.WaterRight)) {
                    continue;
                }

                /// Try to fall.
                if (mapClone.ContainsKey(down) && mapClone[down] == Tile.BlockID.Air) {
                    blockMap[down] = Tile.BlockID.WaterStill;
                    blockMap[here] = Tile.BlockID.Air;
                    continue;
                }
                
                if (tile.blockID == Tile.BlockID.WaterStill) {
                    if (Random.value >= 0.5) {
                        if (flowLeft(x, y)) {
                            continue;
                        } else if (flowRight(x, y)) {
                            continue;
                        }
                    } else {
                        if (flowRight(x, y)) {
                            continue;
                        } else if (flowLeft(x, y)) {
                            continue;
                        }
                    }
                } else if (tile.blockID == Tile.BlockID.WaterLeft) {
                    if (flowLeft(x, y)) {
                        continue;
                    } else if (flowRight(x, y)) {
                        continue;
                    }
                } else if (tile.blockID == Tile.BlockID.WaterRight) {
                    if (flowRight(x, y)) {
                        continue;
                    } else if (flowLeft(x, y)) {
                        continue;
                    }
                }
            }
            
            /// Update the mapClone to the current state of the grid.
            mapClone = new Dictionary<Vector2, Tile.BlockID>(blockMap);
        }

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                Tile tile = _tiles[new Vector2(x, y)];
                tile.SetBlockID(blockMap[new Vector2(x, y)]);
            }
        }
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
                spawnedTile.Init(isOffset, _towerPrefab, new Vector3(x, y));

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        SetUnpassableTiles();
        SetAdjacentTiles();
        
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
        _tiles[new Vector3(6, 6)].SetBlockID(Tile.BlockID.WaterStill);
        
        _tiles[new Vector3(5, 6)].SetBlockID(Tile.BlockID.Dirt);
        _tiles[new Vector3(4, 6)].SetBlockID(Tile.BlockID.Dirt);
        _tiles[new Vector3(3, 6)].SetBlockID(Tile.BlockID.Dirt);
        _tiles[new Vector3(2, 6)].SetBlockID(Tile.BlockID.Dirt);
        _tiles[new Vector3(1, 6)].SetBlockID(Tile.BlockID.Dirt);

        _tiles[new Vector3(6, 4)].SetBlockID(Tile.BlockID.Dirt);
        _tiles[new Vector3(5, 4)].SetBlockID(Tile.BlockID.Dirt);
        _tiles[new Vector3(4, 4)].SetBlockID(Tile.BlockID.Dirt);
        _tiles[new Vector3(3, 4)].SetBlockID(Tile.BlockID.Dirt);
        _tiles[new Vector3(2, 4)].SetBlockID(Tile.BlockID.Dirt);
        _tiles[new Vector3(1, 4)].SetBlockID(Tile.BlockID.Dirt);
        _tiles[new Vector3(0, 4)].SetBlockID(Tile.BlockID.Dirt);

        _tiles[new Vector3(8, 3)].SetBlockID(Tile.BlockID.Dirt);
        _tiles[new Vector3(7, 2)].SetBlockID(Tile.BlockID.Dirt);
        _tiles[new Vector3(6, 2)].SetBlockID(Tile.BlockID.Dirt);
        _tiles[new Vector3(5, 2)].SetBlockID(Tile.BlockID.Dirt);
        _tiles[new Vector3(4, 2)].SetBlockID(Tile.BlockID.Dirt);
        _tiles[new Vector3(3, 2)].SetBlockID(Tile.BlockID.Dirt);
        _tiles[new Vector3(2, 2)].SetBlockID(Tile.BlockID.Dirt);
        _tiles[new Vector3(1, 2)].SetBlockID(Tile.BlockID.Dirt);

        _tiles[new Vector3(1, 1)].SetBlockID(Tile.BlockID.Dirt);
        _tiles[new Vector3(1, 0)].SetBlockID(Tile.BlockID.Dirt);
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
        /// TODO
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
