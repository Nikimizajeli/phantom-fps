using System;
using System.Collections;
using UnityEngine;

public class CaptureTheFlagGameMode : MonoBehaviour, IGameMode
{
    [SerializeField] private PlayerController playerPrefab;
    [SerializeField] private int maxLives = 3;
    [SerializeField] private float respawnTimer = 2f;

    private GameObject _playerObject;
    private int _currentLives;
    private int _gameTimeS;
    private bool _gameFinished;
    private bool _respawning;

    protected void OnEnable()
    {
        EventDispatcher.Instance.AddListener<PlayerDeathEvent>(OnPlayerDeath);
        EventDispatcher.Instance.AddListener<LevelFlagEvent>(OnLevelFlagEvent);
    }

    protected void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener<PlayerDeathEvent>(OnPlayerDeath);
        EventDispatcher.Instance.RemoveListener<LevelFlagEvent>(OnLevelFlagEvent);
    }

    #region IGameMode

    public void StartGame()
    {
        _gameFinished = false;
        _currentLives = maxLives;
        _gameTimeS = 0;
        
        var playerController = Instantiate(playerPrefab);
        EventDispatcher.Instance.Raise<PlayerSpawnedEvent>(new PlayerSpawnedEvent
            { PlayerObject = playerController.gameObject, LivesLeft = _currentLives});
        
        StartCoroutine(CountTime());
    }

    #endregion

    private IEnumerator CountTime()
    {
        while (!_gameFinished)
        {
            yield return new WaitForSeconds(1f);
            if (!GameController.Instance.IsPaused)
            {
                _gameTimeS++;
                EventDispatcher.Instance.Raise(new ScoreUpdatedEvent { Score = _gameTimeS });
            }
        }
    }

    private void OnPlayerDeath(PlayerDeathEvent ev)
    {
        if (_respawning)
        {
            return;
        }
        
        _currentLives--;
        if (_currentLives > 0)
        {
            StartCoroutine(RespawnPlayer(ev.PlayerObject));
        }
        else
        {
            ProcessDefeat();
        }
    }

    private void OnLevelFlagEvent(LevelFlagEvent ev)
    {
        if (!ev.FlagReturned)
        {
            return;
        }

        ProcessVictory();
    }

    private void ProcessVictory()
    {
        _gameFinished = true;
        EventDispatcher.Instance.Raise(new GameCompletedEvent { Victory = true, Score = _gameTimeS });
    }

    private IEnumerator RespawnPlayer(GameObject playerObject)
    {
        _respawning = true;
        yield return new WaitForSeconds(respawnTimer);
        
        EventDispatcher.Instance.Raise<PlayerSpawnedEvent>(new PlayerSpawnedEvent
            { PlayerObject = playerObject, LivesLeft = _currentLives});
        _respawning = false;
    }

    private void ProcessDefeat()
    {
        _gameFinished = true;
        EventDispatcher.Instance.Raise(new GameCompletedEvent { Victory = false });
    }
}