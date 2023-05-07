using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreCounter;
    
    protected void OnEnable()
    {
        EventDispatcher.Instance.AddListener<ScoreUpdatedEvent>(OnScoreUpdated);
    }

    protected void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener<ScoreUpdatedEvent>(OnScoreUpdated);
    }

    public void ShowHUD(bool show)
    {
        gameObject.SetActive(show);
    }

    private void OnScoreUpdated(ScoreUpdatedEvent ev)
    {
        scoreCounter.text = ev.Score.ToString();
    }
}
