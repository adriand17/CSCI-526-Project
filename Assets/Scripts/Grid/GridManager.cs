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
    [SerializeField] public TowerManager _towerManager;
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Particle _particlePrefab;
    [SerializeField] private Grid grid;
    [SerializeField] private Transform _camera;
    [SerializeField] private GameObject _nextWaveButton;
    [SerializeField] private GameObject _GameOverText;
 

    /// Type of block placed when player builds.
    protected BlockType buildType = BlockType.Dirt;

    public HashSet<Particle> particles = new HashSet<Particle>();
    private Dictionary<Vector2, Tile> _tiles;

    /// [HEALTH SYSTEM]
    [SerializeField] private HealthBar healthBar;
    public int maxHealth = 50;
    public int damage = 2;
    public int currentHealth;

    /// [BUILD LIMIT HUD]
    private int _goldSpent = 0;
    private int _goldLimit = 100;

    /// Displays number of available building blocks.
    [SerializeField] private TextMeshProUGUI _remainingGoldText;

    /// Displays the "Buildable Blocks" label.
    [SerializeField] private TextMeshProUGUI _goldLabelText;

    /// Empty flashing animation.
    private Coroutine TextFlash;

    // Start is called before the first frame update
    public void onStart()
    {
        _remainingGoldText.text = (_goldLimit - _goldSpent).ToString();
        GenerateGrid();
        ResetHealth();
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
                Vector2 gridPosition = new Vector2(x, y);
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                //for checker board patter...
                var isOffset = (x + y) % 2 == 1;
                spawnedTile.Init(isOffset, new Vector3(x, y), this);

                _tiles[gridPosition] = spawnedTile;
            }
        }

        AddLevelBlocks();

        _camera.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }

    public void DrawParticle(BlockType type, Vector3 pos)
    {
        var tile = _tiles[pos];
        var particle = Instantiate(_particlePrefab, new Vector3(pos.x, pos.y), Quaternion.identity);
        particle.Init(type, tile, this);
        tile.SetParticle(particle);
        particles.Add(particle);
    }

    // Create inital level geometry.
    private void AddLevelBlocks()
    {
        GameManager _gameManager = GameManager.Instance;
        int rowCount = _gameManager._gridLocations.indicies.Count;
        int colCount;
        for (int row = 0; row < rowCount; row++)
        {
            colCount = _gameManager._gridLocations.indicies[row].locations.Count;
            for (int col = 0; col < colCount; col++)
            {
                int drawRow = (rowCount - 1) - row;
                int drawCol = col;
                int blockID = _gameManager._gridLocations.indicies[row].locations[col];
                Vector3 pos = new Vector3(drawCol, drawRow);
                switch (blockID)
                {
                    case -1:
                        /// Represents air.
                        break;
                    case 0:
                        DrawParticle(BlockType.Water, pos);
                        break;
                    case 1:
                        DrawParticle(BlockType.Bedrock, pos);
                        break;
                    case 2:
                        DrawParticle(BlockType.Dirt, pos);
                        break;
                    case 3:
                        DrawParticle(BlockType.Mirror, pos);
                        break;
                    case 4:
                        DrawParticle(BlockType.Glass, pos);
                        break;
                    case 5:
                        DrawParticle(BlockType.Magma, pos);
                        break;
                    case 6:
                        DrawParticle(BlockType.BlueIce, pos);
                        break;
                    case 7:
                        DrawParticle(BlockType.TNT, pos);
                        break;
                    default:
                        Debug.LogError($"Invalid block ID {blockID} at row {drawRow}, col {drawCol}");
                        break;
                }
            }
        }
    }

    public Tile GetTileAt(Vector2 position)
    {
        if (_tiles.ContainsKey(position))
        {
            return _tiles[position];
        }
        else
        {
            return null;

        }
    }

    private int buildTypePrice(BlockType buildType)
    {
        switch (buildType)
        {
            case BlockType.TNT:
                return 50;
            case BlockType.Glass:
                return 20;
            case BlockType.Dirt:
                return 30;
            case BlockType.Mirror:
                return 40;
            case BlockType.Magma:
            case BlockType.Bedrock:
            case BlockType.BlueIce:
                return 60;

            default:
                Debug.LogError("Non placeable block type have no price: " + buildType);
                return 0;
        }
    }


    public bool CanAddBlockToTile(Vector3 pos)
    {
        Tile t = _tiles[pos];
        //Debug.Log(t._isPassable);
        // if the existing building count excess the limit and player want to add budling on the pos

        if (_goldSpent + buildTypePrice(buildType) > _goldLimit)
        {
            /// Can only remove.
            if (t.particle != null && (t.particle.getBlockType() == BlockType.Dirt || t.particle.getBlockType() == BlockType.Glass || t.particle.getBlockType() == BlockType.Mirror))
            {
                _goldSpent -= buildTypePrice(t.particle.getBlockType());
                DestroyImmediate(t.particle.gameObject);
                particles.Remove(t.particle);
                t.particle = null;
                _remainingGoldText.text = (_goldLimit - _goldSpent).ToString();
            }
            else if (t.particle == null)
            {
                // Tile is empty.
                if (TextFlash != null)
                {
                    StopCoroutine(TextFlash);
                }
                TextFlash = StartCoroutine(FlashCountText());
            }

            Debug.Log(_goldSpent + "/" + _goldLimit);
            return false;
        }
        else
        {
            if (t.particle == null)
            {
                _goldSpent += buildTypePrice(buildType);
                // DrawParticle(buildType, pos);
                DrawParticle(BlockType.Vapor, pos);
                _remainingGoldText.text = (_goldLimit - _goldSpent).ToString();

                /// Log block placement.
                int level = 0;
                string uri = $"https://docs.google.com/forms/d/e/1FAIpQLSdfkfxAYRFo31DSvEuicQb5tr1xx7a3Q-DvU4ZpT_inCt7xtA/formResponse?usp=pp_url&entry.1421622821={level}&entry.2002566203={pos.x}&entry.1372862866={pos.y}&entry.1572288735={BlockType.Dirt}";
                MakeGetRequest(uri);
            }
            else if (t.particle.getBlockType() == BlockType.Dirt || t.particle.getBlockType() == BlockType.Glass || t.particle.getBlockType() == BlockType.Mirror)
            {
                _goldSpent -= buildTypePrice(buildType);
                DestroyImmediate(t.particle.gameObject);
                particles.Remove(t.particle);
                t.particle = null;
                _remainingGoldText.text = (_goldLimit - _goldSpent).ToString();
            }

            Debug.Log(_goldSpent + "/" + _goldLimit);
            return true;
        }
    }


    public bool CanAddTowerToTile(Vector3 pos)
    {      
        Tile t = GetTileAt(pos);

        if (t.particle == null)
        {
            _towerManager.BuildTowerOnTile(t);
            return true;
        }

        return false;
    }

    public List<Tile> GetTowerTiles(Vector3 position, int range)
    {
       List<Tile> inRangeTiles = new List<Tile>();
       int posx = (int)position.x;
       int posy = (int)position.y;
       for(int r = 1; r < range + 1; r++)
       {
            for (int x = posx - r; x < posx + r + 1; x++)
            {
                for (int y = posy - r; y < posy + r + 1; y++)
                {
                    if((((posx + r) == x) || ((posx - r) == x)) ||
                            (((posy + r) == y) || ((posy - r) == y)))
                    {
                        Tile t = GetTileAt(new Vector3(x, y));
                        if(t != null)
                        {
                            inRangeTiles.Add(t);
                        }
                        
                    }
                }
            }

        }

        return inRangeTiles;
    }

    public void ResetGrid()
    {
        /// TODO
        foreach (Particle p in particles)
        {
            if (p.getBlockType() != BlockType.Bedrock && p != null)
            {
                DestroyImmediate(p.gameObject);
            }
        }
        _goldSpent = 0;
        particles.Clear();
        ResetHealth();
        _remainingGoldText.text = (_goldLimit - _goldSpent).ToString();
    }

    public void TakeDamage()
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    private void checkWaterAtBottom()
    {
        //for now check if bottom row has water
        for (int x = 0; x < _width; x++)
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
  
    public void DestoryWateratTile(Tile t)
    {
        particles.Remove(t.particle);
        DestroyImmediate(t.particle.gameObject);
    }

    /// How long to play pulse animation.
    [SerializeField] private float flashDuration = 2.4f;

    /// Pulses building count text red. 
    /// Reminds player they can't build.
    private IEnumerator FlashCountText()
    {
        float counter = 0;
        while (counter <= flashDuration)
        {
            /// Reset immediately if player can build.
            if (_goldLimit - _goldSpent >= buildTypePrice(buildType))
            {
                _remainingGoldText.color = Color.white;
                _goldLabelText.color = Color.white;
                yield break;
            }

            _remainingGoldText.color = Color.Lerp(Color.white, Color.red, counter % 0.8f);
            _goldLabelText.color = Color.Lerp(Color.white, Color.red, counter % 0.8f);
            counter += Time.deltaTime;
            yield return null;
        }

        /// Final color reset.
        _remainingGoldText.color = Color.white;
        _goldLabelText.color = Color.white;
        yield return null;
    }

    public int GetWaterCount()
    {
        int count = 0;
        //if check if all waves are complete
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Tile t = _tiles[new Vector3(x, y)];
                if (t.particle != null && t.particle.getBlockType() == BlockType.Water)
                {
                    count++;
                }
            }
        }

        return count;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int getHeight()
    {
        return _height;
    }

    public int getWidth()
    {
        return _width;
    }

    /// Allow other classes to make requests via the grid.
    /// The grid is never destroyed, and won't "drop" the co-routines.
    public void MakeGetRequest(string uri)
    {
        StartCoroutine(GetRequest(uri));
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
                    Debug.Log("GET Request successful");
                    break;
            }
        }
    }

    public void SetBlockBuildType(BlockType type)
    {
        buildType = type;
    }

    public void setGridHeight(int h) {
        _height = h;
    }

    public void setGridWidth(int w) {
        _width = w;
    }
}
