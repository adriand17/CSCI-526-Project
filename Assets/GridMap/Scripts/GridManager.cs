using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] public GameManager _gameManager;
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private BaseTower _towerPrefab;
    [SerializeField] private Particle _particlePrefab;
    [SerializeField] private Grid grid;
    [SerializeField] private Transform _camera;
    [SerializeField] private BuildingManager buildingManager;
    [SerializeField] private bool enableText;

    public HashSet<Particle> particles = new HashSet<Particle>();

    private Dictionary<Vector2, Tile> _tiles;
    private float waterInterval;

    void Awake(){
        Instance = this;
    }
    private int _buildingCount = 0;
    private int _buildingLimit = 3;
    private Text _MyText;
    private float _timeToAppear = 2f;
    private float _timeWhenDisappear;

    // Start is called before the first frame update
    public void onStart()
    {
        enableText = false;
        GenerateGrid();
		// A correct website page.
        StartCoroutine(GetRequest("https://docs.google.com/forms/d/e/1FAIpQLSdH4rGRcgwsHFzd5gCYm-uOJ6yOjeC1HQWpnNTCZkM3o7l-BA/formResponse?usp=pp_url&entry.49243494=Yes&submit=Submit"));

    }

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
                spawnedTile.Init(isOffset, _towerPrefab, new Vector3(x, y), this);

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

    public void DrawParticle(BlockType type, Vector3 pos)
    {
        var tile = _tiles[pos];
        var particle = Instantiate(_particlePrefab, new Vector3(pos.x, pos.y), Quaternion.identity);
        particle.Init(type, tile);
        tile.SetParticle(particle);
        particles.Add(particle);
    }

    // Create inital level geometry.
    private void SetUnpassableTiles()
    {

        DrawParticle(BlockType.Dirt, new Vector3(5, 6));
        DrawParticle(BlockType.Dirt, new Vector3(4, 6));
        DrawParticle(BlockType.Dirt, new Vector3(3, 6));
        DrawParticle(BlockType.Dirt, new Vector3(2, 6));
        DrawParticle(BlockType.Dirt, new Vector3(1, 6));

        DrawParticle(BlockType.Dirt, new Vector3(6, 4));
        DrawParticle(BlockType.Dirt, new Vector3(5, 4));
        DrawParticle(BlockType.Dirt, new Vector3(4, 4));
        DrawParticle(BlockType.Dirt, new Vector3(3, 4));
        DrawParticle(BlockType.Dirt, new Vector3(2, 4));
        DrawParticle(BlockType.Dirt, new Vector3(1, 4));
        DrawParticle(BlockType.Dirt, new Vector3(0, 4));

        DrawParticle(BlockType.Dirt, new Vector3(8, 3));
        DrawParticle(BlockType.Dirt, new Vector3(7, 2));
        DrawParticle(BlockType.Dirt, new Vector3(6, 2));
        DrawParticle(BlockType.Dirt, new Vector3(5, 2));
        DrawParticle(BlockType.Dirt, new Vector3(4, 2));
        DrawParticle(BlockType.Dirt, new Vector3(3, 2));
        DrawParticle(BlockType.Dirt, new Vector3(2, 2));
        DrawParticle(BlockType.Dirt, new Vector3(1, 2));

        DrawParticle(BlockType.Dirt, new Vector3(1, 1));
        DrawParticle(BlockType.Dirt, new Vector3(1, 0));
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

   /* public bool UpdatePassability(Vector3 pos )
    {
        Tile t = _tiles[pos];
        Debug.Log(t._isPassable);
        // if the existing building count excess the limit and player want to add budling on the pos
        if (_buildingCount >= _buildingLimit && t._isPassable)
        {
            enableText = true;
            return false;
        }
        else
        {
            if (t._isPassable)
            {
                _buildingCount++;
            }
            else
            {
                _buildingCount--;
            }
            return true;
        }
    }*/

    public void ResetGrid()
    {
        /// TODO
        foreach (Particle p in particles)
        {
            if(p.getBlockType() == BlockType.Water)
            {
                DestroyImmediate(p.gameObject);
               
            }
            
        }
        particles.Clear();
      
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

    public int getHeight()
    {
        return _height;
    }

    public int getWidth()
    {
        return _width;
    }

}
