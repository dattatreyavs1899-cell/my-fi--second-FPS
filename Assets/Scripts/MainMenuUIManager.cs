using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Type the EXACT name of your main game scene here.")]
    public string gameSceneName = "GameScene";

    public void PlayGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT GAME: The player has exited the application.");

        Application.Quit();
    }
}