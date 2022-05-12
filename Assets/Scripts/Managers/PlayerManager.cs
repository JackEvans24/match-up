using Assets.Scripts.Coroutines;
using Assets.Scripts.Events;
using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerScoreIndicator scoreIndicator;
        [SerializeField] private RectTransform playerScoresContainer;
        [SerializeField] private PointsCalculator pointsCalculator;

        [Header("Events")]
        [SerializeField] private PlayerEvent onScoreUpdated;
        [SerializeField] private PlayerEvent onActivePlayerChanged;

        [Header("Variables")]
        [SerializeField] private int playerCount = 2;

        private List<Player> players;
        private int currentPlayerIndex;

        private Player currentPlayer { get => players[currentPlayerIndex]; }

        private void Start()
        {
            SetUpPlayers();
        }

        private void SetUpPlayers()
        {
            players = new List<Player>();

            for (int i = 0; i < playerCount; i++)
            {
                var newPlayer = new Player($"Player {i + 1}");
                players.Add(newPlayer);

                var newScoreIndicator = Instantiate(scoreIndicator, playerScoresContainer);
                newScoreIndicator.Initialise(newPlayer);
            }

            currentPlayerIndex = 0;
            onActivePlayerChanged?.Raise(currentPlayer);
        }

        public void TileConfirmed(Tile tile) => StartCoroutine(CountPoints(tile));

        private IEnumerator CountPoints(Tile tile)
        {
            yield return pointsCalculator.CalculatePoints(tile, currentPlayer);

            SelectNextPlayer();
            onActivePlayerChanged?.Raise(currentPlayer);
        }

        private void SelectNextPlayer()
        {
            currentPlayerIndex++;
            if (currentPlayerIndex >= players.Count)
                currentPlayerIndex = 0;
        }
    }
}
