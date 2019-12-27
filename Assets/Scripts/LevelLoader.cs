using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class LevelLoader : MonoBehaviour
    {
        [UsedImplicitly]
        public void LevelLoad(string levelName)
        {
            SceneManager.LoadScene(levelName);
            //SceneManager.LoadSceneAsync(levelName);
        }

        [UsedImplicitly]
        public void CloseGame()
        {
            Application.Quit();
        }
    }
}