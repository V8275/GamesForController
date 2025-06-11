using UnityEngine;
using UnityEngine.UI;

public class UIController : Controller
{
    [Header("End Game Panel")]
    [SerializeField]
    private GameObject EndGamePanel;
    [SerializeField]
    private Text EndGameText;
    [SerializeField] 
    private string EndGameLoseTitle = "Вы проиграли!";
    [SerializeField] 
    private string EndGameWinTitle = "Вы выиграли!";
    [Header("Score")]
    [SerializeField]
    private Text ScoreText;

    private GameEvents gameEvents;

    public override void Init(GameEvents events)
    {
        gameEvents = events;
        EndGamePanel.SetActive(false);
        gameEvents.OnGameFinished += (iswin) => 
        { OpenEndScreen(iswin); };
        gameEvents.OnChangeScore += (score) =>
        { UpdateScore(score); };
    }

    public override void UpdateController() { }

    private void OpenEndScreen(bool win)
    {
        EndGamePanel.SetActive(true);
        if (win)
        {
            EndGameText.text = EndGameWinTitle;
        }
        else
        {
            EndGameText.text = EndGameLoseTitle;
        }
    }

    private void UpdateScore(int score)
    {
        ScoreText.text = "" + score;
    }
}
