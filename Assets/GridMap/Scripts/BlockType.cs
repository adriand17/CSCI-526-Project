using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Water,   // 0, Water block.
    Bedrock, // 1, Uneditable level structure block.
    Dirt,    // 2, Player placable block.
    Mirror,  // 3, Mirror block.
    Glass,   // 4, Glass block.
    Magma,   // 5, Makes water hot.
    Cooler,  // Makes water cold.
}