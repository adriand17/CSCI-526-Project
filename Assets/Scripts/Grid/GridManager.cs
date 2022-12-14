using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour
{

    [SerializeField] public GameManager _gameManager;
    [SerializeField] public TowerManager _towerManager;
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Blast1 _blastPrefab;
    [SerializeField] private Particle _particlePrefab;
    [SerializeField] private Grid grid;
    [SerializeField] private Transform _camera;
    [SerializeField] private GameObject _GameOverText;
 
    //Keep Track of Rain Makers
    public List<RainMakerBlock> _rainMakerBlocks;

    /// Type of block placed when player builds.
    protected BlockType buildType = BlockType.None;

    public HashSet<Particle> particles = new HashSet<Particle>();
    private Dictionary<Vector2, Tile> _tiles;
    BlockRange br = new BlockRange();
    
    /// Empty flashing animation.
    private Coroutine TextFlash;


    public int waterCount;

    private List<Particle> portalList = new List<Particle>();
    // Start is called before the first frame update
    public void onStart()
    {
        waterCount = 0;
        _rainMakerBlocks = new List<RainMakerBlock>();
        SetBlockBuildType(_gameManager._blockSelectionButtonTypes[0]);
        GenerateGrid();
        ResetHealth();
    }

    void ResetHealth()
    {
        _GameOverText.SetActive(false);
    }

    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        _gameManager.textPlaceBoxes[0].text = _gameManager.blocksPlaced[0].ToString();
        _gameManager.textPlaceBoxes[1].text = _gameManager.blocksPlaced[1].ToString();
        _gameManager.textPlaceBoxes[2].text = _gameManager.blocksPlaced[2].ToString();
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

        if (type == BlockType.RainMaker)
        {
            _rainMakerBlocks.Add((RainMakerBlock)particle.block);
        }
    }

    public void CreateExplosion(Vector3 location)
    {
        var blast = Instantiate(_blastPrefab, new Vector3(location.x, location.y), Quaternion.identity);
        blast.Init(this);
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
                        waterCount++;
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
                    case 8:
                        DrawParticle(BlockType.Evaporator, pos);
                        break;
                    case 9:
                        DrawParticle(BlockType.Condensation, pos);
                        break;
                    case 10:
                        DrawParticle(BlockType.Vapor, pos);
                        waterCount++;
                        break;
                    case 11:
                        DrawParticle(BlockType.RainMaker, pos);
                        break;
                    case 12:
                        DrawParticle(BlockType.RainTrigger, pos);
                        break;
                    case 13:
                        DrawParticle(BlockType.PortalEntry, pos);
                        break;
                    case 14:
                        DrawParticle(BlockType.PortalExit, pos);
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
            case BlockType.Evaporator:
                return 10;
            case BlockType.Condensation:
                return 10;
            default:
                Debug.LogError("Non placeable block type have no price: " + buildType);
                return 0;
        }
    }


    public bool CanAddBlockToTile(Vector3 pos)
    {
        Tile t = _tiles[pos];
        
        // if the existing building count excess the limit and player want to add budling on the pos
        int index = _gameManager.whichButtonPressed(buildType);

        if (t.particle == null && buildType != BlockType.None && index != -1 && _gameManager.blocksPlaced[index] > 0)
        {
            return true;
        }
        return false;
    }
    
    public void AddBlockToTile(Vector3 pos)
    {
        Tile t = _tiles[pos];
        int index = _gameManager.whichButtonPressed(buildType);
        DrawParticle(buildType, pos);
        
        _gameManager.blocksPlaced[index]--;
        _gameManager.textPlaceBoxes[index].text = _gameManager.blocksPlaced[index].ToString();
        
        /// Log block placement.
        string level = SceneManager.GetActiveScene().name;
        string uri = $"https://docs.google.com/forms/d/e/1FAIpQLSfP2foRoq7thSwuBci5e3pqX4SAXmmWIHdhKSGDei93TckhgQ/formResponse?usp=pp_url&entry.1767141370={pos.x}&entry.1642741263={pos.y}&entry.913150214={buildType}&entry.274409603={level}&submit";
        MakeGetRequest(uri);
    }

    public bool CanBreakBlockAtTile(Vector3 pos)
    {
        Tile t = _tiles[pos];
        // if the existing building count excess the limit and player want to add budling on the pos
        int index = _gameManager.whichButtonPressed(buildType);

        if (t.particle != null && t.particle.getBlockType() == buildType && index!= -1 && _gameManager.blocksPlaced[index] < _gameManager._blocksGiven[index])
        {
            return true;
        }
        return false;
    }
    
    public void BreakBlockAtTile(Vector3 pos)
    {
        Tile t = _tiles[pos];
        int index = _gameManager.whichButtonPressed(buildType);
        DestroyImmediate(t.particle.gameObject);
        particles.Remove(t.particle);
        removePortal(t.particle);

        t.particle = null;
        _gameManager.blocksPlaced[index]++;
        _gameManager.textPlaceBoxes[index].text = _gameManager.blocksPlaced[index].ToString();
    }

    public BlockType getBuildType()
    {
        return buildType;
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

    public List<Tile> GetInRangeTiles(Vector3 position)
    {
        List<Tile> inRangeTiles = new List<Tile>();
        int posx = (int)position.x;
        int posy = (int)position.y;
        Tile t = GetTileAt(new Vector3(posx, posy));

        inRangeTiles = br.GetInRangeTiles(t, buildType);
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
        /*for (int i = 0; i < 3; i++)
        {
            _gameManager.blocksPlaced[i] = 0;
        }*/
        waterCount = 0;
        particles.Clear();
        portalList.Clear();
        GenerateGrid();
        ResetHealth();
    }

    public void DestoryParticleAtTile(Tile t)
    {
        particles.Remove(t.particle);
        DestroyImmediate(t.particle.gameObject);
    }

    /// How long to play pulse animation.
    [SerializeField] private float flashDuration = 2.4f;

    public int GetWaterCount()
    {
        return waterCount;
    }

    public Block ReplaceBlockAtTile (Tile t, BlockType replace)
    {
        DestoryParticleAtTile(t);
        DrawParticle(replace, t.location);
        return t.particle.block;
  
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

    public void addPortal(Particle particle) {
        portalList.Add(particle);
    }

    public void removePortal(Particle particle) {
        Debug.Log("removePortal");
        portalList.Remove(particle);
    }

    public List<Vector3> getAllPortalPosition() {
        List<Vector3> positions = new List<Vector3>();
        foreach (Particle particle in portalList) {
            Vector3 pos = particle.gameObject.transform.position;
            positions.Add(pos);
        }
        return positions;        
    }
}
