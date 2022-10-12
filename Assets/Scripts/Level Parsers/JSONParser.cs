using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class JSONParser : MonoBehaviour
{
    public GameManager _gameManager;

    [System.Serializable]
    public class GridLocations
    {
        public List<int> locations;
    }


    [System.Serializable]
    public class Wave
    {
        public List<GridLocations> wavelocations;
      
    }

    [System.Serializable]
    public class WaveArray
    {
        public List<Wave> waves;
        public int rows;
        public int cols;
    }

    [System.Serializable]
    public class GridLocationsArray
    {
        public List<GridLocations> indicies;
        public int rows;
        public int cols;
    }

    public void Parse(TextAsset text)
    {
        JSONNode data = JSON.Parse(text.text);
        foreach (JSONNode n in data["indicies"])
        {
            GridLocations dropLocations = new GridLocations();
            dropLocations.locations = new List<int>();
            foreach (JSONNode x in n)
            {
                dropLocations.locations.Add(x.AsInt);
            }
            _gameManager._dropLocations.indicies.Add(dropLocations);
        }
    }
    public void ParseLevel(TextAsset text)
    {
        JSONNode data = JSON.Parse(text.text);
        foreach (JSONNode n in data["indicies"])
        {
            GridLocations gridLocations = new GridLocations();
            gridLocations.locations = new List<int>();
            foreach (JSONNode x in n)
            {
                gridLocations.locations.Add(x.AsInt);
            }
            _gameManager._gridLocations.indicies.Add(gridLocations);
        }
        _gameManager._gridLocations.rows = data["rows"];
        _gameManager._gridLocations.cols = data["cols"];
    }


    public void ParseWaves(TextAsset text)
    {
        JSONNode data = JSON.Parse(text.text);
        foreach (JSONNode m in data["waves"])
        {
            Wave w = new Wave();
            w.wavelocations = new List<GridLocations>();
            foreach(JSONNode n in m)
            {
                foreach (JSONNode x in n)
                {
                    GridLocations dropLocations = new GridLocations();
                    dropLocations.locations = new List<int>();
                    foreach (JSONNode i in x)
                    {
                        dropLocations.locations.Add(i.AsInt);
                    }
                    w.wavelocations.Add(dropLocations);

                }      
            }
            _gameManager._wavesArray.waves.Add(w);

        }
    }
}
