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
        positionField.text = position.ToString();
        nameField.text = entry.PlayerName;
        scoreField.text = entry.Score.ToString();
    }
}