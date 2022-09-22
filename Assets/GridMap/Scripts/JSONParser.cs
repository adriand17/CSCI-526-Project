using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class JSONParser : MonoBehaviour
{
    public GameManager _gameManager;

    [System.Serializable]
    public class DropLocations
    {
        public List<int> locations;
    }

    [System.Serializable]
    public class DropLocationsArray
    {
        public List<DropLocations> indicies;
        public int rows;
        public int cols;
    }

    public void Parse(TextAsset text)
    {
        JSONNode data = JSON.Parse(text.text);
        foreach (JSONNode n in data["indicies"])
        {
            DropLocations dropLocations = new DropLocations();
            dropLocations.locations = new List<int>();
            foreach (JSONNode x in n)
            {
                dropLocations.locations.Add(x.AsInt);
            }
            _gameManager._dropLocations.indicies.Add(dropLocations);
        }
    }
}
