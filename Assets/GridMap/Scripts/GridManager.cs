using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] public GameManager _gameManager;
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private BaseTower _towerPrefab;
    [SerializeField] private Particle _particlePrefab;
    [SerializeField] private Grid grid;
    [SerializeField] private Transform _camera;
    [SerializeField] private BuildingManager buildingManager;
    [SerializeField] private GameObject _nextWaveButton;
    [SerializeField] private GameObject _GameOverText;

    public HashSet<Particle> particles = new HashSet<Particle>();
    [SerializeField] private HealthBar healthBar;
    public int maxHealth = 50;
    public int damage = 2;
    public int currentHealth;


    private Dictionary<Vector2, Tile> _tiles;
    
    private int _buildingCount = 0;
    [SerializeField] private int _buildingLimit = 3;
    [SerializeField] public TextMeshProUGUI _buildingCountText;

    // Start is called before the first frame update
    public void onStart()
    {
        _buildingCountText.text = (_buildingLimit - _buildingCount).ToString();
        GenerateGrid();
        ResetHealth();
        // A correct website page.
        StartCoroutine(GetRequest("https://docs.google.com/forms/d/e/1FAIpQLSdH4rGRcgwsHFzd5gCYm-uOJ6yOjeC1HQWpnNTCZkM3o7l-BA/formResponse?usp=pp_url&entry.49243494=Yes&submit=Submit"));

    }

    void Update()
    {
        checkWaterAtBottom();
        if (currentHealth == 0)
        {
            _nextWaveButton.SetActive(false);
            _GameOverText.SetActive(true);
        }
    }

    void ResetHealth()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        _nextWaveButton.SetActive(true);
        _GameOverText.SetActive(false);
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
                    ? _tiles[new Vector2(x - 1, y)]
                    : null;

                t.setAdjacentTiles(underTile, leftTile, rightTile);
            }
        }
    }

    public void DrawParticle(BlockType type, Vector3 pos)
    {
        var tile = _tiles[pos];
        var particle = Instantiate(_particlePrefab, new Vector3(pos.x, pos.y), Quaternion.identity);
        //prevent lazer from targeting
       /* if(type != BlockType.Water)
        {
            particle.tag = "Wall";
        }*/
        particle.Init(type, tile, this);
        tile.SetParticle(particle);
        particles.Add(particle);
    }

    // Create inital level geometry.
    private void SetUnpassableTiles()
    {

        DrawParticle(BlockType.Bedrock, new Vector3(5, 6));
        DrawParticle(BlockType.Bedrock, new Vector3(4, 6));
        DrawParticle(BlockType.Bedrock, new Vector3(3, 6));
        DrawParticle(BlockType.Bedrock, new Vector3(2, 6));
        DrawParticle(BlockType.Bedrock, new Vector3(1, 6));

        DrawParticle(BlockType.Bedrock, new Vector3(6, 4));
        DrawParticle(BlockType.Bedrock, new Vector3(5, 4));
        DrawParticle(BlockType.Bedrock, new Vector3(4, 4));
        DrawParticle(BlockType.Bedrock, new Vector3(3, 4));
        DrawParticle(BlockType.Bedrock, new Vector3(2, 4));
        DrawParticle(BlockType.Bedrock, new Vector3(1, 4));


        DrawParticle(BlockType.Bedrock, new Vector3(6, 2));
        DrawParticle(BlockType.Bedrock, new Vector3(5, 2));
        DrawParticle(BlockType.Bedrock, new Vector3(4, 2));
        DrawParticle(BlockType.Bedrock, new Vector3(3, 2));
        DrawParticle(BlockType.Bedrock, new Vector3(2, 2));
        DrawParticle(BlockType.Bedrock, new Vector3(1, 2));

        DrawParticle(BlockType.Bedrock, new Vector3(9, 3));
        DrawParticle(BlockType.Bedrock, new Vector3(10, 3));
        DrawParticle(BlockType.Bedrock, new Vector3(11, 3));
        DrawParticle(BlockType.Bedrock, new Vector3(12, 3));
        DrawParticle(BlockType.Bedrock, new Vector3(13, 3));
        DrawParticle(BlockType.Bedrock, new Vector3(14, 3));
        DrawParticle(BlockType.Bedrock, new Vector3(9, 4));
        DrawParticle(BlockType.Bedrock, new Vector3(14, 4));
        DrawParticle(BlockType.Bedrock, new Vector3(14, 5));


        DrawParticle(BlockType.Bedrock, new Vector3(0, 0));
        DrawParticle(BlockType.Bedrock, new Vector3(1, 0));
        DrawParticle(BlockType.Bedrock, new Vector3(2, 0));
        DrawParticle(BlockType.Bedrock, new Vector3(3, 0));
        DrawParticle(BlockType.Bedrock, new Vector3(4, 0));
        DrawParticle(BlockType.Bedrock, new Vector3(5, 0));
        DrawParticle(BlockType.Mirror, new Vector3(6, 0));

        DrawParticle(BlockType.Mirror, new Vector3(9, 0));
        DrawParticle(BlockType.Bedrock, new Vector3(10, 0));
        DrawParticle(BlockType.Bedrock, new Vector3(11, 0));
        DrawParticle(BlockType.Bedrock, new Vector3(12, 0));
        DrawParticle(BlockType.Bedrock, new Vector3(13, 0));
        DrawParticle(BlockType.Bedrock, new Vector3(14, 0));
        DrawParticle(BlockType.Bedrock, new Vector3(15, 0));

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

   public bool CanAddBlockToTile(Vector3 pos )
    {
        Tile t = _tiles[pos];
        //Debug.Log(t._isPassable);
        // if the existing building count excess the limit and player want to add budling on the pos
        
        if (_buildingCount >= _buildingLimit)
        {
            //can only remove
            if(t.particle != null && t.particle.getBlockType() == BlockType.Dirt)
            {
                _buildingCount--;
                DestroyImmediate(t.particle.gameObject);
                particles.Remove(t.particle);
                t.particle = null;
                _buildingCountText.text = (_buildingLimit - _buildingCount).ToString();
            }
            Debug.Log(_buildingCount + "/" + _buildingLimit);
            return false;
        }
        else
        {
            if (t.particle == null)
            {
                _buildingCount++;
                DrawParticle(BlockType.Dirt, pos);
                t.particle.userPlaced = true;
                _buildingCountText.text = (_buildingLimit - _buildingCount).ToString();
               
            }
            else if (t.particle.getBlockType() == BlockType.Dirt)
            {
                _buildingCount--;
                DestroyImmediate(t.particle.gameObject);
                particles.Remove(t.particle);
                t.particle = null;
                _buildingCountText.text = (_buildingLimit - _buildingCount).ToString();
            }
            Debug.Log(_buildingCount + "/" + _buildingLimit);
            return true;
        }
    }

    public void ResetGrid()
    {
        /// TODO
        foreach (Particle p in particles)
        {
            if(p.getBlockType() != BlockType.Bedrock && p != null)
            {
                DestroyImmediate(p.gameObject);
               
            } 
        }
        _buildingCount = 0;
        particles.Clear();
        ResetHealth();
        _buildingCountText.text = (_buildingLimit - _buildingCount).ToString();
      
    }

    public void TakeDamage()
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
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

    private void checkWaterAtBottom()
    {
        //for now check if bottom row has water
        for(int x = 0; x < _width; x++)
        {
            
            Tile t = _tiles[new Vector3(x, 0)];
            if (t.particle != null && t.particle.getBlockType() == BlockType.Water)
            {
                particles.Remove(t.particle);
                DestroyImmediate(t.particle.gameObject);
                t.particle = null;
                
                TakeDamage();
               
            }
        }
    }

   /* public void DestroyWaterParticle(Particle p)
    {
        if (p != null && p.getBlockType() == BlockType.Water)
        {
            DestroyImmediate(p.gameObject);

        }
    }*/

    public int getHeight()
    {
        return _height;
    }

    public int getWidth()
    {
        return _width;
    }

}
