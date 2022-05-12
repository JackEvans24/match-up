using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class PointSoundManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AudioSource audioSource;

        [Header("Sounds")]
        [SerializeField] private AudioClip positiveSound;
        [SerializeField] private AudioClip neutralSound;
        [SerializeField] private AudioClip negativeSound;
        [SerializeField] private AudioClip bonusSound;

        public void TilesScored(ScoreType scoreType)
        {
            AudioClip clip = null;

            switch (scoreType)
            {
                case ScoreType.Positive:
                    clip = positiveSound;
                    break;
                case ScoreType.Neutral:
                    clip = neutralSound;
                    break;
                case ScoreType.Negative:
                    clip = negativeSound;
                    break;
                case ScoreType.Bonus:
                    clip = bonusSound;
                    break;
            }

            if (clip == null)
                return;

            audioSource.PlayOneShot(clip);
        }
    }
}
