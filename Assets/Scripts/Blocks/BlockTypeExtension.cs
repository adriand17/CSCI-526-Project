using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockTypeExtension
{
    public static class Extensions
    {
        public static bool isOpaqueToLaser(this BlockType blockType)
        {
            switch (blockType)
            {
                case BlockType.Glass:   
                    return false;

                case BlockType.Water:
                case BlockType.Bedrock:
                case BlockType.Mirror:
                case BlockType.Dirt:
                case BlockType.Magma:
                case BlockType.BlueIce:
                case BlockType.TNT:
                case BlockType.Evaporator:
                case BlockType.Condensation:
                case BlockType.Vapor:
                    return true;

                default:
                    Debug.LogError("Unknown block type: " + blockType);
                    return true;
            }
        }

        public static bool isExplodable(this BlockType blockType)
        {
            switch (blockType)
            {
                /// Custom chain explosion.
                case BlockType.TNT:
                    return true;

                /// Custom interaction.
                case BlockType.Water:

                /// Place-able blocks are explodable.
                case BlockType.Glass:
                case BlockType.Dirt:
                case BlockType.Mirror:
                    return true;

                /// Towers aren't explodable.
                case BlockType.Magma:
                case BlockType.BlueIce:
                    return false;

                /// Never destroyed.
                case BlockType.Bedrock:
                    return false;

                default:
                    Debug.LogError("Unknown block type: " + blockType);
                    return false;
            }
        }
    }
}