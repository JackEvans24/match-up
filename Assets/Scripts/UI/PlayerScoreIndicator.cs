using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class PlayerScoreIndicator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_Text nameLabel;
        [SerializeField] private TMP_Text scoreLabel;

        [Header("Variables")]
        [SerializeField] private Color activeColor;
        [SerializeField] private Color inactiveColor;

        private Player player;

        public void Initialise(Player player)
        {
            this.player = player;
            nameLabel.text = player.Name;
        }

        public void ScoreUpdated(Player player)
        {
            if (player != this.player)
                return;

            scoreLabel.text = player.Score.ToString();
        }

        public void ActivePlayerChanged(Player player)
        {
            var color = player == this.player ? activeColor : inactiveColor;

            nameLabel.color = color;
            scoreLabel.color = color;
        }

        public void GameOver()
        {
            nameLabel.color = activeColor;
            scoreLabel.color = activeColor;
        }
    }
}
