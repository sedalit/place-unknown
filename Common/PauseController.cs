using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] private UIPanel pausePanel;

    private bool isGamePaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pausePanel.Show();
        Time.timeScale = 0;
        isGamePaused = true;

        GameStateController.GameStateChanged?.Invoke(GameState.Pause);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pausePanel.Hide();
        isGamePaused = false;

        GameStateController.GameStateChanged?.Invoke(GameState.Play);
    }
}
