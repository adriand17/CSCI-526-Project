using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Water,   // Water block.
    Bedrock, // Uneditable level structure block.
    Dirt,    // Player placable block.
    Mirror,  // Mirror block.
    Glass,   // Glass block.
    Magma,  // Makes water hot.
    Cooler,  // Makes water cold.
}