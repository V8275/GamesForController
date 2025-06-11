using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Text fpsText;
    [SerializeField]
    private GameObject DisconnectPanel;

    private float deltaTime = 0.0f;
    private bool debugMode = false;

    private void Start()
    {
        if(fpsText) debugMode = true;
        if (WebSocketManager.Instance != null)
            WebSocketManager.Instance.InitUI(DisconnectPanel);
    }

    void Update()
    {
        if (debugMode)
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            fpsText.text = $"FPS: {Mathf.Round(fps)}";
        }
    }

    public void RestartLevel()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLevel(int i)
    {
        LoadScene(i);
    }

    public void LoadMenu()
    {
        LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void LoadScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Continue()
    {
        Time.timeScale = 1;
    }
}
