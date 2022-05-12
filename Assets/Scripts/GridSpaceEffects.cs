using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class GridSpaceEffects : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GridSpace gridSpace;

        [Header("Events")]
        [SerializeField] private UnityEvent onBonusFire;
        [SerializeField] private UnityEvent onBonusGrass;
        [SerializeField] private UnityEvent onBonusWater;

        public void PointsScored(Tile tile)
        {
            if (tile != gridSpace.CurrentTile)
                return;

            switch (tile.TileType)
            {
                case ScriptableObjects.TileType.Water:
                    onBonusWater?.Invoke();
                    break;
                case ScriptableObjects.TileType.Fire:
                    onBonusFire?.Invoke();
                    break;
                case ScriptableObjects.TileType.Grass:
                    onBonusGrass?.Invoke();
                    break;
            }
        }
    }
}
