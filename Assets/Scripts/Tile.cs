using Assets.Scripts.Events;
using Assets.Scripts.Managers;
using Assets.Scripts.ScriptableObjects;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts
{
    public class Tile : Draggable
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer backgroundSpriteRenderer;
        [SerializeField] private SpriteRenderer borderSpriteRenderer;
        [SerializeField] private SpriteRenderer iconSpriteRenderer;

        [Header("Events")]
        [SerializeField] private TileEvent onTileDropped;
        [SerializeField] private TileEvent onTileReturned;

        [Header("Sprite Layer")]
        [SerializeField] private string activeLayerName;
        [SerializeField] private string defaultLayerName;

        [Header("Wiggle Movement")]
        [SerializeField] private Vector2 wiggleDistance;
        [SerializeField] private Vector2 wiggleSpeed;
        [SerializeField] private Ease easing;

        private TileTypeModel tileTypeData;
        public TileType TileType { get => tileTypeData.tileType; }

        [HideInInspector] public GridSpace GridSpace;

        private Vector3 originalPosition;

        private Sequence xMovement;
        private Sequence yMovement;

        private void Awake()
        {
            Initialise(transform.position);
        }

        public void Initialise(Vector3 position)
        {
            originalPosition = position;
            target = originalPosition;
        }

        public void SetTileType(TileTypeModel typeData)
        {
            iconSpriteRenderer.sprite = typeData.icon;
            iconSpriteRenderer.color = typeData.colour;

            tileTypeData = typeData;
        }

        public void Lock()
        {
            this.canDrag = false;
        }

        public void Unlock()
        {
            this.canDrag = true;
        }

        private void OnMouseUp()
        {
            if (!canDrag)
                return;

            SetSpriteSortingLayers(false);

            var gridSpace = GridManager.Instance?.GetSpaceAtPosition(transform.position);

            if (gridSpace != null)
            {
                if (GridSpace == gridSpace)
                {
                    SetTileTarget(gridSpace);
                    return;
                }

                if (gridSpace.TryAddTile(this))
                {
                    GridSpace?.RemoveTile();
                    GridSpace = gridSpace;

                    SetTileTarget(gridSpace);

                    onTileDropped?.Raise(this);
                }
                else if (GridSpace != null)
                {
                    SetTileTarget(GridSpace);
                }
                else
                {
                    target = originalPosition;
                }
            }
            else
            {
                ReturnTile();
            }
        }

        private void OnMouseDown()
        {
            SetSpriteSortingLayers(true);
            StopWiggle();
        }

        public void ReturnTile()
        {
            StopWiggle();

            if (GridSpace != null)
            {
                GridSpace.RemoveTile();
                GridSpace = null;

                onTileReturned?.Raise(this);
            }

            target = originalPosition;
        }

        public void ConfirmTilePlacement()
        {
            if (GridSpace == null)
                throw new System.InvalidOperationException("Tile has no GridSpace");

            StopWiggle();

            target = GridSpace.transform.position;
            transform.position = target;

            canDrag = false;
        }

        private void SetTileTarget(GridSpace gridSpace)
        {
            StopWiggle();

            target = gridSpace.transform.position;
            transform.position = target;

            StartWiggle();
        }

        private void SetSpriteSortingLayers(bool active)
        {
            var layerName = active ? activeLayerName : defaultLayerName;

            iconSpriteRenderer.sortingLayerName = layerName;
            backgroundSpriteRenderer.sortingLayerName = layerName;
            borderSpriteRenderer.sortingLayerName = layerName;
        }

        private void StartWiggle()
        {
            xMovement = DOTween.Sequence()
                .Append(transform.DOMoveX(target.x + wiggleDistance.x, wiggleSpeed.x / 2).SetEase(Ease.OutSine))
                .Append(transform.DOMoveX(target.x - wiggleDistance.x, wiggleSpeed.x).SetEase(Ease.InOutSine))
                .Append(transform.DOMoveX(target.x, wiggleSpeed.x / 2).SetEase(Ease.InSine))
                .SetLoops(-1);

            yMovement = DOTween.Sequence()
                .Append(transform.DOMoveY(target.y + wiggleDistance.y, wiggleSpeed.y / 2).SetEase(Ease.OutSine))
                .Append(transform.DOMoveY(target.y - wiggleDistance.y, wiggleSpeed.y).SetEase(Ease.InOutSine))
                .Append(transform.DOMoveY(target.y, wiggleSpeed.y / 2).SetEase(Ease.InSine))
                .SetLoops(-1);
        }

        private void StopWiggle()
        {
            if (xMovement != null)
            {
                xMovement.Kill();
                xMovement = null;
            }
            if (yMovement != null)
            {
                yMovement.Kill();
                yMovement = null;
            }
        }
    }
}
