using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockID 
{ 
    Air,     // Absence of a block.
    Water,   // Water block.
    Bedrock, // Uneditable level structure block.
    Dirt,    // Player placable block.
}

public class Particle : MonoBehaviour
{

    public BlockID blockID = BlockID.Air;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
