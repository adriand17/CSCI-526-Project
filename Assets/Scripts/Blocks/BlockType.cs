using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Water,   // 0: Water block.
    Bedrock, // 1: Uneditable level structure block.
    Dirt,    // 2: Player placable block.
    Mirror,  // 3: Mirror block.
    Glass,   // 4: Glass block.
    Magma,   // 5: Makes water hot.
    BlueIce, // 6: Makes water cold.
    TNT,     // 7: Explodes non-water blocks.
    Evaporator, //8: Converts liquid water to gas
    Condensation, //9. Convert vapor to water
    Vapor, //10: Gas form of water, moves up instead
    RainMaker,
    RainTrigger,
    PortalEntry, //13: Portal Entry
    PortalExit, //14: Portal Exit
    None
}

// TODO: TNT
// - lit by laser
// - lit by magma
//   - leave this for player discovery?

// TODO: Sponge
// - absorbs water in an area
// - when containing water, leaks it out slowly
// - shoot with laser to dry it out
// - how to prevent pick-n-place abuse?

// Note: ice might thaw naturally (at a slow rate) to avoid buildup.