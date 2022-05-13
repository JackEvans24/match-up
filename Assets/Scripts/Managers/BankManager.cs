using Assets.Scripts.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class BankManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Tile tile;

        [Header("Events")]
        [SerializeField] private GameEvent onBankEmptied;

        [Header("Bank Variables")]
        [SerializeField] private int maxTileCount = 3;
        [SerializeField] private float bankXOffset = 2;
        [SerializeField] private float bankTileSpacing = 2f;

        private List<Tile> tiles = new List<Tile>();

        public void AddToBank(Tile tile)
        {
            var pos = GetBankTilePosition(tiles.Count);
            tile.Initialise(pos);

            tiles.Add(tile);

            tile.gameObject.SetActive(true);
        }

        public void TilePlaced(Tile placedTile)
        {
            tiles.Remove(placedTile);

            foreach (var tile in tiles)
            {
                tile.Lock();
            }
        }

        public void TileReturned(Tile returnedTile)
        {
            tiles.Add(returnedTile);

            foreach (var tile in tiles)
            {
                tile.Unlock();
            }
        }

        public void ActivePlayerChanged(Player _)
        {
            if (tiles.Count == 0)
                onBankEmptied?.Raise();
            else
            {
                foreach (var tile in tiles)
                {
                    tile.Unlock();
                }
            }
        }

        private Vector2 GetBankTilePosition(int i)
        {
            var gridCenter = ((Vector2)GridManager.Instance.GridDimensions - Vector2.one) * 0.5f;
            var centerOffset = ((maxTileCount - 1f) / 2) - i;

            return new Vector2(gridCenter.x + bankXOffset, gridCenter.y + (centerOffset * bankTileSpacing));
        }
    }
}
