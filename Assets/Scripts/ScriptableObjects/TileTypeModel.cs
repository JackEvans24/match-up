using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Tile Type", menuName = "Tile Type")]
    public class TileTypeModel : ScriptableObject
    {
        public Sprite icon;
        public Color colour;
        public TileType tileType;
    }

    public enum TileType
    {
        Water,
        Fire,
        Grass
    }
}
