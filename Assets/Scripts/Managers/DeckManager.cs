using Assets.Scripts.Events;
using Assets.Scripts.Extensions;
using Assets.Scripts.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class DeckManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Tile baseTile;
        [SerializeField] private TileTypeModel[] tileTypes;
        [SerializeField] private SpriteRenderer deckSpriteRenderer;
        [SerializeField] private TMP_Text cardCountLabel;

        [Header("Events")]
        [SerializeField] private TileEvent onTileDrawn;
        [SerializeField] private GameEvent onGameComplete;

        [Header("Variables")]
        [SerializeField] private int tileCount = 10;
        [SerializeField] private float drawTileInterval = 0.4f;

        private Queue<Tile> tiles;
        private bool drawing;

        private void Awake()
        {
            GenerateDeck();
        }

        private void Start()
        {
            DrawTiles(3);
        }

        private void GenerateDeck()
        {
            var deckTileParent = gameObject.CreateChild("Tiles").transform;

            tiles = new Queue<Tile>();
            var tileList = new List<Tile>();

            foreach (var type in tileTypes)
            {
                for (int i = 0; i < tileCount; i++)
                {
                    var newTile = Instantiate(baseTile, deckTileParent);
                    newTile.gameObject.SetActive(false);

                    newTile.SetTileType(type);

                    tileList.Add(newTile);
                }
            }

            foreach (var tile in tileList.OrderBy(x => Random.Range(0f, 1f)))
                tiles.Enqueue(tile);

            UpdateDeckCount();
        }

        public void DrawTiles(int count)
        {
            if (tiles.Count == 0)
            {
                onGameComplete?.Raise();
                return;
            }
            else if (drawing)
                return;

            StartCoroutine(DrawTilesWithInterval(count));
        }

        private IEnumerator DrawTilesWithInterval(int count)
        {
            drawing = true;

            for (int i = 0; i < count; i++)
            {
                if (tiles.Count == 0)
                    break;

                var nextTile = tiles.Dequeue();
                onTileDrawn?.Raise(nextTile);

                UpdateDeckCount();
                yield return new WaitForSeconds(drawTileInterval);
            }

            if (tiles.Count == 0)
            {
                deckSpriteRenderer.sprite = null;
                cardCountLabel.alpha = 0;
            }

            drawing = false;
        }

        private void UpdateDeckCount() => cardCountLabel.text = tiles.Count.ToString();
    }
}
