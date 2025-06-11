using System;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField]
    private int[] BestScores;

    private static ScoreController _instance;
    private int currentScore = 0;
    private int NowScore = 0;
    private ScoreSaver sc;

    public static ScoreController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<ScoreController>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(ScoreController).Name);
                    _instance = singletonObject.AddComponent<ScoreController>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        LoadScores();
    }

    public void Init(int scoreNum)
    {
        currentScore = scoreNum;
        NowScore = 0;

        if (BestScores == null || BestScores.Length != sc.scores.Length)
        {
            BestScores = new int[sc.scores.Length];
            Array.Copy(sc.scores, BestScores, sc.scores.Length);
        }
    }

    public void SetCurrentScore(int sc)
    {
        NowScore += sc;
    }

    public int GetCurrentScore()
    {
        return NowScore;
    }

    public bool IsMoreThanBestScore()
    {
        return BestScores[currentScore] > NowScore ? true : false;
    }

    public void AddFinalScore(int score)
    {
        BestScores[currentScore] = score;
        sc.scores[currentScore] = score;
        SaveScores();
    }

    private void LoadScores()
    {
        string json = PlayerPrefs.GetString("SavedScores", string.Empty);

        if (!string.IsNullOrEmpty(json))
        {
            sc = JsonUtility.FromJson<ScoreSaver>(json);
        }
        else
        {
            // »нициализаци€ по умолчанию, если сохранений нет
            sc = new ScoreSaver();
            sc.scores = new int[BestScores.Length];
            Array.Copy(BestScores, sc.scores, BestScores.Length);
        }
    }

    private void SaveScores()
    {
        string json = JsonUtility.ToJson(sc);
        PlayerPrefs.SetString("SavedScores", json);
        PlayerPrefs.Save();
    }
}

[Serializable]
public class ScoreSaver
{
    public int[] scores;
}
