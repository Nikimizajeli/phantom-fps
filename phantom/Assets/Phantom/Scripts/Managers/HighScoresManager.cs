using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct HighScoreEntry
{
    public string PlayerName;
    public int Score;

    // public static bool operator >(HighScoreEntry entry1, HighScoreEntry entry2)
    // {
    //     return entry1.Score > entry2.Score;
    // }
    //
    // public static bool operator <(HighScoreEntry entry1, HighScoreEntry entry2)
    // {
    //     return entry1.Score < entry2.Score;
    // }
}

public class HighScoreEntryComparer : IComparer<HighScoreEntry>
{
    public int Compare(HighScoreEntry x, HighScoreEntry y)
    {
        if (x.Score < y.Score)
        {
            return -1;
        }

        return x.Score > y.Score ? 1 : 0;
    }
}

public class HighScoresManager : MonoBehaviour
{
    private readonly SortedSet<HighScoreEntry> _highScores = new SortedSet<HighScoreEntry>(new HighScoreEntryComparer());

    protected void Start()
    {
        _highScores.Add(new HighScoreEntry { PlayerName = "Player1", Score = 100 });
        _highScores.Add(new HighScoreEntry { PlayerName = "Player2", Score = 200 });
        _highScores.Add(new HighScoreEntry { PlayerName = "Player3", Score = 50 });
        _highScores.Add(new HighScoreEntry { PlayerName = "Player4", Score = 660 });
        _highScores.Add(new HighScoreEntry { PlayerName = "Player5", Score = 66 });
        _highScores.Add(new HighScoreEntry { PlayerName = "Player6", Score = 120 });
    }

    public SortedSet<HighScoreEntry> GetHighScores()
    {
        return _highScores;
    }
}