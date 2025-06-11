using System;

public class GameEvents
{
    public  event Action<bool> OnGameFinished;
    public  event Action<int> OnChangeScore;
    public  event Action OnRestartLevel;

    public  void TriggerGameFinish(bool isWin)
    {
        OnGameFinished?.Invoke(isWin);
    }

    public  void RestartLevel()
    {
        OnRestartLevel?.Invoke();
    }

    public void ChangeScore(int score)
    {
        OnChangeScore?.Invoke(score);
    }
}
