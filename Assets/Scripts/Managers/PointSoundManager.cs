using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class PointSoundManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AudioSource audioSource;

        [Header("Sounds")]
        [SerializeField] private AudioClip neutralSound;
        [SerializeField] private AudioClip basicSound;
        [SerializeField] private AudioClip bonusSound;

        public void TilesScored(ScoreType scoreType)
        {
            AudioClip clip = null;

            switch (scoreType)
            {
                case ScoreType.Basic:
                    clip = basicSound;
                    break;
                case ScoreType.None:
                    clip = neutralSound;
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
