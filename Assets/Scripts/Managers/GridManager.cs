using Assets.Scripts.Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance;

        [Header("References")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GridSpace gridSpace;
        [SerializeField] private Tile tile;

        [Header("Grid Variables")]
        [SerializeField] private Vector2Int gridDimensions;

        public Vector2Int GridDimensions { get => gridDimensions; }
        public Dictionary<Vector2Int, GridSpace> GridSpaces { get => this.gridSpacesLookup; }

        private Dictionary<Vector2Int, GridSpace> gridSpacesLookup;

        private void Awake()
        {
            if (Instance != null)
                Destroy(this.gameObject);

            Instance = this;
        }

        public GridSpace GetSpaceAtPosition(Vector2 position)
        {
            var nearestIntPosition = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
            if (!gridSpacesLookup.TryGetValue(nearestIntPosition, out var tile))
                return null;

            return tile;
        }

        private void Start()
        {
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            var gridTileParent = gameObject.CreateChild("Grid Tiles");
            gridSpacesLookup = new Dictionary<Vector2Int, GridSpace>();

            for (int y = 0; y < gridDimensions.y; y++)
            {
                for (int x = 0; x < gridDimensions.x; x++)
                {
                    var createdTile = Instantiate(gridSpace, new Vector3(x, y, 0), Quaternion.identity, gridTileParent.transform);
                    createdTile.name = $"Grid Tile ({x + 1},{y + 1})";

                    var coordinates = new Vector2Int(x, y);
                    createdTile.Initialise(coordinates);

                    gridSpacesLookup[coordinates] = createdTile;
                }
            }
        }
    }
}
