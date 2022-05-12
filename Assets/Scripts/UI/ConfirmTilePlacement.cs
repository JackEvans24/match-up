using Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ConfirmTilePlacement : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private TileEvent onTileConfirmed;

        private CanvasGroup canvasGroup;
        private Tile currentTile;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void TilePlaced(Tile tile)
        {
            ToggleCanvas(true);
            currentTile = tile;
        }

        public void TileReturned(Tile tile)
        {
            ToggleCanvas(false);
            currentTile = null;
        }

        public void ConfirmTile()
        {
            currentTile.ConfirmTilePlacement();

            ToggleCanvas(false);
            onTileConfirmed?.Raise(currentTile);
        }

        public void ReturnTile()
        {
            currentTile.ReturnTile();
        }

        private void ToggleCanvas(bool enabled)
        {
            canvasGroup.alpha = enabled ? 1 : 0;
            canvasGroup.blocksRaycasts = enabled;
            canvasGroup.interactable = enabled;
        }
    }
}
