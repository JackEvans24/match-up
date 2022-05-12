using Assets.Scripts.Coroutines;
using Assets.Scripts.Events;
using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class PointsCalculator : MonoBehaviour
    {
        public static int[] PointsValues = new int[] { 5, 3, 2, 1 };

        [Header("Events")]
        [SerializeField] private ScoreEvent onTilesScored;
        [SerializeField] private PlayerEvent onScoreUpdated;

        private Dictionary<Vector2Int, GridSpace> gridSpaces;
        private Vector2Int gridDimensions;

        public void Initialise()
        {
            gridSpaces = GridManager.Instance.GridSpaces;
            gridDimensions = GridManager.Instance.GridDimensions;
        }

        public void SetEvents(PlayerEvent onScoreUpdated)
        {
            this.onScoreUpdated = onScoreUpdated;
        }

        public IEnumerator CalculatePoints(Tile tile, Player currentPlayer)
        {
            if (gridSpaces == null || gridDimensions == null)
                this.Initialise();

            // Calculate basic points (tiles in the same row or column)
            var coroutines = new CoroutineCollection();

            for (int i = 1; i <= 4; i++)
            {
                var coordinatesToCheck = Directions.CardinalDirections.Select(dir => tile.GridSpace.Coordinates + (dir * i));
                var spacesToCheck = new List<GridSpace>();
                foreach (var coordinate in coordinatesToCheck)
                {
                    if (gridSpaces.TryGetValue(coordinate, out var space))
                        spacesToCheck.Add(space);
                }

                if (spacesToCheck.Count > 0)
                {
                    var roundPoints = 0;
                    var hasScore = false;

                    foreach (var space in spacesToCheck)
                    {
                        var spacePoints = space.CalculatePointsGivenTile(tile);
                        currentPlayer.AddPoints(spacePoints);
                        roundPoints += spacePoints;

                        hasScore |= spacePoints != 0;

                        coroutines.Add(StartCoroutine(space.Animate(spacePoints)));
                    }

                    onTilesScored?.Raise(hasScore ? ScoreType.Basic : ScoreType.None);
                    onScoreUpdated?.Raise(currentPlayer);
                }

                yield return coroutines.WaitForCompletion();
            }

            // Calculate basic points (3 or more of the same tile in a row/column)
            // 5 points for 3 in a row, 10 for 4, 15 for 5
            var bonusPoints = 0;
            var markedForDeletion = new HashSet<GridSpace>();

            // Bonus points for tiles in column
            var columnBonusSpaces = GetBonusSpaces(tile, BonusDirection.Column);
            if (columnBonusSpaces.Count >= 3)
                bonusPoints += (columnBonusSpaces.Count - 2) * 5;

            // Bonus points for tiles in row
            var rowBonusSpaces = GetBonusSpaces(tile, BonusDirection.Row);
            if (rowBonusSpaces.Count >= 3)
                bonusPoints += (rowBonusSpaces.Count - 2) * 5;

            if (bonusPoints > 0)
            {
                // Show bonus animations
                var bonusSpaces = columnBonusSpaces.Union(rowBonusSpaces);
                foreach (var space in bonusSpaces)
                    coroutines.Add(StartCoroutine(space.AnimateBonus(bonusPoints, space == tile.GridSpace)));

                currentPlayer.AddPoints(bonusPoints);
                onScoreUpdated?.Raise(currentPlayer);

                onTilesScored?.Raise(ScoreType.Bonus);

                yield return coroutines.WaitForCompletion();

                // Delete marked tiles
                foreach (var space in bonusSpaces)
                {
                    var markedTile = space.RemoveTile();
                    markedTile.gameObject.SetActive(false);
                }
            }
        }

        private HashSet<GridSpace> GetBonusSpaces(Tile tile, BonusDirection direction)
        {
            var (negativeDirection, positiveDirection) = direction == BonusDirection.Row ?
                (Vector2Int.left, Vector2Int.right) :
                (Vector2Int.down, Vector2Int.up);

            var negativeSpaces = GetMatchingSpacesInDirection(tile, negativeDirection);
            var positiveSpaces = GetMatchingSpacesInDirection(tile, positiveDirection);

            var result = new HashSet<GridSpace>();

            if (negativeSpaces.Count + positiveSpaces.Count > 1)
            {
                result.Add(tile.GridSpace);
                result.UnionWith(negativeSpaces);
                result.UnionWith(positiveSpaces);
            }

            return result;
        }

        private HashSet<GridSpace> GetMatchingSpacesInDirection(Tile currentTile, Vector2Int direction)
        {
            var result = new HashSet<GridSpace>();

            while (true)
            {
                var coordinateToCheck = currentTile.GridSpace.Coordinates + (direction * (result.Count + 1));
                if (!gridSpaces.TryGetValue(coordinateToCheck, out var spaceToCheck))
                    break;

                if (spaceToCheck.CurrentTile == null || spaceToCheck.CurrentTile.TileType != currentTile.TileType)
                    break;

                result.Add(spaceToCheck);
            }

            return result;
        }

        private enum BonusDirection { Row, Column }
    }
}
