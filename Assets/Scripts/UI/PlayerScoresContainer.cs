using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class PlayerScoresContainer : MonoBehaviour
    {
        [SerializeField] private Vector2 gameOverMinAnchor;
        [SerializeField] private Vector2 gameOverMaxAnchor;

        private RectTransform rectTransform;

        private void Awake()
        {
            this.rectTransform = GetComponent<RectTransform>();
        }

        public void GameOver()
        {
            rectTransform.anchorMin = gameOverMinAnchor;
            rectTransform.anchorMax = gameOverMaxAnchor;
        }
    }
}
