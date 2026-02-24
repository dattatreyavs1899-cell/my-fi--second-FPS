using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    [Header("UI References")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI timeText;
    public Image mainPanelBG;
    public Button retryButton;
    public Button mainMenuButton;

    [Header("Panel Colors")]
    public Color winColor = new Color(0f, 0.4f, 0f, 0.8f);
    public Color loseColor = new Color(0.4f, 0f, 0f, 0.8f);

    [Header("Scene Settings")]
    public string mainMenuSceneName = "MainMenu";

    private bool isTimerRunning = false;
    private float elapsedSeconds = 0f;

    void Awake()
    {
        if (Instance == null) Instance = this;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    void Start()
    {
        if (retryButton != null) retryButton.onClick.AddListener(RetryGame);
        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    void Update()
    {
        if (isTimerRunning)
        {
            elapsedSeconds += Time.deltaTime;
        }
    }

    public void StartTimer()
    {
        isTimerRunning = true;
        elapsedSeconds = 0f;
    }

    public void ShowGameOver(bool isWin)
    {
        isTimerRunning = false;
        gameOverPanel.SetActive(true);

        int minutes = Mathf.FloorToInt(elapsedSeconds / 60F);
        int seconds = Mathf.FloorToInt(elapsedSeconds - minutes * 60);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);

        timeText.text = "Time Taken: " + timeString;

        if (isWin)
        {
            titleText.text = "YOU WIN!";
            mainPanelBG.color = winColor;
        }
        else
        {
            titleText.text = "YOU LOSE!";
            mainPanelBG.color = loseColor;
        }

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void RetryGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}