using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Managers
{
    public class MenuManager : MonoBehaviour
    {
        public void PlayGame() => SceneManager.LoadScene((int)Scenes.Game);

        public void QuitGame() => Application.Quit();
    }
}
