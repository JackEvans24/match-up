using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Directions
    {
        public static IEnumerable<Vector2Int> CardinalDirections = new Vector2Int[] { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
    }
}
