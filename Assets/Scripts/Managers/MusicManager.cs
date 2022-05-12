using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class MusicManager : MonoBehaviour
    {
        private static MusicManager instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this.gameObject);

            DontDestroyOnLoad(this.gameObject);
        }
    }
}
