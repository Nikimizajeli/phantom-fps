using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreMenuTemplate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI positionField;
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private TextMeshProUGUI scoreField;

    public void Init(int position, HighScoreEntry entry)
    {
        positionField.text = $"{position}.";
        nameField.text = entry.playerName;
        scoreField.text = entry.score.ToString();
    }
}