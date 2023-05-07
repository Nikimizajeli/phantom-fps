using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreCounter;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI ammoCounter;
    [SerializeField] private TextMeshProUGUI livesCounter;
    [SerializeField] private Image flagImage;
    
    protected void OnEnable()
    {
        EventDispatcher.Instance.AddListener<ScoreUpdatedEvent>(OnScoreUpdated);
        EventDispatcher.Instance.AddListener<LevelFlagEvent>(OnLevelFlagEvent);
        EventDispatcher.Instance.AddListener<PlayerAmmoUpdatedEvent>(OnPlayerAmmoUpdatedEvent);
    }

    protected void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener<ScoreUpdatedEvent>(OnScoreUpdated);
        EventDispatcher.Instance.RemoveListener<LevelFlagEvent>(OnLevelFlagEvent);
        EventDispatcher.Instance.RemoveListener<PlayerAmmoUpdatedEvent>(OnPlayerAmmoUpdatedEvent);
    }

    public void ShowHUD(bool show)
    {
        gameObject.SetActive(show);
    }

    private void OnScoreUpdated(ScoreUpdatedEvent ev)
    {
        scoreCounter.text = ev.Score.ToString();
    }

    private void OnLevelFlagEvent(LevelFlagEvent ev)
    {
        flagImage.gameObject.SetActive(ev.FlagPickedUp);
    }

    private void OnPlayerAmmoUpdatedEvent(PlayerAmmoUpdatedEvent ev)
    {
        ammoCounter.text = $"Ammo:\n{ev.CurrentAmmo}";
    }
}
