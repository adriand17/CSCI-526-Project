using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockTypeExtension { 
    public static class Extensions {
        public static bool isOpaqueToLaser(this BlockType blockType) {
            return blockType == BlockType.Bedrock || blockType == BlockType.Mirror;
        }
    }
}