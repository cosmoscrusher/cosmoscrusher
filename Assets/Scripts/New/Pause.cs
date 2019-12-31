using UnityEngine;

namespace Assets.Scripts.New
{
    public class Pause : MonoBehaviour
    {
        public GameObject pausePanel;
        public GameObject gameOverPanel;

        void Start()
        {
            pausePanel.SetActive(false);
        }
        void Update()
        {
            if (!gameOverPanel.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
            {
                if (!pausePanel.activeInHierarchy)
                {
                    PauseGame();
                }
                else if (pausePanel.activeInHierarchy)
                {
                    ContinueGame();
                }
            }
        }
        private void PauseGame()
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            //Disable scripts that still work while timescale is set to 0
        }
        private void ContinueGame()
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
            //enable the scripts again
        }
    }
}
