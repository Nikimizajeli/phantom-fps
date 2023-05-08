using System;
using System.Collections;
using System.Collections.Generic;
using Phantom.Scripts.Configuration;
using TMPro;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameOverHeader;
    [SerializeField] private TextMeshProUGUI gameOverScore;

    protected void Awake()
    {
        EventDispatcher.Instance.AddListener<GameCompletedEvent>(OnGameCompleted);
    }

    protected void OnDestroy()
    {
        EventDispatcher.Instance.RemoveListener<GameCompletedEvent>(OnGameCompleted);
    }

    private void OnGameCompleted(GameCompletedEvent ev)
    {
        gameOverHeader.text = ev.Victory ? TextConstants.Victory : TextConstants.Defeated;
        gameOverScore.text = ev.Score.ToString();
    }
}
