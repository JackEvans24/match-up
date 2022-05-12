using Assets.Scripts.Events;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class GridSpace : MonoBehaviour
    {
        [Header("Animation References")]
        [SerializeField] private SpriteRenderer borderSpriteRenderer;
        [SerializeField] private TMP_Text pointsText;

        [Header("Animation Variables")]
        [SerializeField] private float borderScale = 1.2f;
        [SerializeField] private Color borderColour = Color.white;
        [SerializeField] private Color positiveTextColour = Color.green;
        [SerializeField] private Color negativeTextColour = Color.red;
        [SerializeField] private float fadeTime = 0.5f;

        [Header("Animation Events")]
        [SerializeField] private TileEvent onBasicPointsScored;
        [SerializeField] private TileEvent onBonusPointsScored;

        [Header("Runtime variables")]
        public Vector2Int Coordinates;
        public Tile CurrentTile;

        public void Initialise(Vector2Int coordinates)
        {
            Coordinates = coordinates;
        }

        public bool TryAddTile(Tile newTile)
        {
            if (CurrentTile != null)
                return false;

            CurrentTile = newTile;
            return true;
        }

        public Tile RemoveTile()
        {
            var tile = CurrentTile;
            CurrentTile = null;
            return tile;
        }

        public int CalculatePointsGivenTile(Tile tile)
        {
            if (CurrentTile == null)
                return 0;
            else if ((tile.GridSpace.Coordinates.x != Coordinates.x) == (tile.GridSpace.Coordinates.y != Coordinates.y))
                return 0;

            var spaceDifference = Mathf.Max(
                Mathf.Abs(tile.GridSpace.Coordinates.x - Coordinates.x),
                Mathf.Abs(tile.GridSpace.Coordinates.y - Coordinates.y)
            );
            var spacePoints = PointsCalculator.PointsValues[spaceDifference - 1];
            var coefficient = tile.TileType == CurrentTile.TileType ? 1 : -1;

            return spacePoints * coefficient;
        }

        public IEnumerator Animate(int points)
        {
            var borderAnimations = GetBorderAnimations();

            if (points != 0)
            {
                onBasicPointsScored?.Raise(CurrentTile);
                yield return GetTextAnimation(points).WaitForCompletion();
            }

            foreach (var animation in borderAnimations)
                yield return animation.WaitForCompletion();
        }

        public IEnumerator AnimateBonus(int points, bool showText)
        {
            var borderAnimations = GetBorderAnimations();

            onBonusPointsScored?.Raise(CurrentTile);

            if (showText)
                yield return GetTextAnimation(points).WaitForCompletion();

            foreach (var animation in borderAnimations)
                yield return animation.WaitForCompletion();
        }

        private IEnumerable<Sequence> GetBorderAnimations()
        {
            var returnScale = borderSpriteRenderer.transform.localScale;
            var borderScaleSequence = DOTween.Sequence()
                .Append(borderSpriteRenderer.transform.DOScale(borderScale, fadeTime))
                .Append(borderSpriteRenderer.transform.DOScale(returnScale, fadeTime));

            var returnColour = borderSpriteRenderer.color;
            var borderColourSequence = DOTween.Sequence()
                .Append(borderSpriteRenderer.DOColor(borderColour, fadeTime))
                .Append(borderSpriteRenderer.DOColor(returnColour, fadeTime));

            return new Sequence[] { borderScaleSequence, borderColourSequence };
        }

        private Sequence GetTextAnimation(int points)
        {
            var sign = points > 0 ? "+" : "-";
            pointsText.text = $"{sign}{Mathf.Abs(points)}";
            pointsText.color = points > 0 ? positiveTextColour : negativeTextColour;

            var textSequence = DOTween.Sequence()
                .Append(pointsText.DOFade(1f, fadeTime))
                .Append(pointsText.DOFade(0f, fadeTime));

            return textSequence;
        }
    }
}
