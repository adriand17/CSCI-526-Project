using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Death
{
    public enum Cause {
        Laser,
        Heating,  // Heated by magma block
        Cooling,  // Frozen by ice block (typically doesn't cause death)
        Erosion,  // Eroded by water
        Explosion // Exploded by TNT
    }

    // Define an extension method in a non-nested static class.
    public static class Extensions { 
        public static string name(this Cause cause) { 
            switch (cause) {
                case Cause.Laser:
                    return "Laser";
                case Cause.Heating:
                    return "Heating";
                case Cause.Cooling:
                    return "Cooling";
                case Cause.Erosion:
                    return "Erosion";
                case Cause.Explosion:
                    return "Explosion";
                default:
                    return "Unknown";
            }
        }
    }
}
