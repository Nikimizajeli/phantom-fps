using System;
using System.Collections.Generic;
using System.Linq;
using Phantom.Scripts.Configuration;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct HighScoreEntry : IComparable<HighScoreEntry>
{
    public string playerName;
    public int score;

    public int CompareTo(HighScoreEntry other)
    {
        return score.CompareTo(other.score);
    }
}

[Serializable]
public class HighScoreData
{
    public List<HighScoreEntry> highScores;

    public HighScoreData(SortedSet<HighScoreEntry> highScoresSet)
    {
        highScores = new List<HighScoreEntry>(highScoresSet);
    }
}

public class HighScoresManager : MonoBehaviour
{
    private SortedSet<HighScoreEntry> _highScores = new SortedSet<HighScoreEntry>();

    protected void Start()
    {
        LoadHighScores();
    }

    protected void OnEnable()
    {
        EventDispatcher.Instance.AddListener<GameCompletedEvent>(OnGameCompleted);
    }

    protected void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener<GameCompletedEvent>(OnGameCompleted);
    }

    public SortedSet<HighScoreEntry> GetHighScores()
    {
        return _highScores;
    }

    private void OnGameCompleted(GameCompletedEvent ev)
    {
        if (!ev.Victory)
        {
            return;
        }

        _highScores.Add(new HighScoreEntry { playerName = GameController.Instance.PlayerName, score = ev.Score });
        SaveHighScores();        
    }

    private void SaveHighScores()
    {
        var highScoresToSave = new SortedSet<HighScoreEntry>(_highScores.Take(Constants.NumberOfHighScoresToSave));
        string highScoresJson = JsonUtility.ToJson(new HighScoreData(highScoresToSave));
        PlayerPrefs.SetString(Constants.PlayerPrefsHighScoreKey, highScoresJson);
        PlayerPrefs.Save();
    }

    private void LoadHighScores()
    {
        if (!PlayerPrefs.HasKey(Constants.PlayerPrefsHighScoreKey))
        {
            return;
        }
        
        string highScoresJson = PlayerPrefs.GetString(Constants.PlayerPrefsHighScoreKey);
        var highScoresList = JsonUtility.FromJson<HighScoreData>(highScoresJson).highScores;
        _highScores = new SortedSet<HighScoreEntry>(highScoresList);
    }
}