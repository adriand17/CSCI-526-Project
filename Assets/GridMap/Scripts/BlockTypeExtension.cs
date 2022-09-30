using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockTypeExtension { 
    public static class Extensions {
        public static bool isOpaqueToLaser(this BlockType blockType) {
            switch (blockType) {
                case BlockType.Glass:
                    return false;
                
                case BlockType.Water:
                case BlockType.Bedrock:
                case BlockType.Mirror:
                case BlockType.Dirt:
                case BlockType.Heater:
                case BlockType.Cooler:
                    return true;
                
                default:
                    return true;
            }
        }
    }
}