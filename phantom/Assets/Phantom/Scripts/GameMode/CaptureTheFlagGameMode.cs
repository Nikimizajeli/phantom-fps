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
        var playerController = Instantiate(playerPrefab);
        EventDispatcher.Instance.Raise<PlayerSpawnedEvent>(new PlayerSpawnedEvent
            { PlayerObject = playerController.gameObject });

        _currentLives = maxLives;
    }

    #endregion

    private void OnPlayerDeath(PlayerDeathEvent ev)
    {
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
        EventDispatcher.Instance.Raise(new GameCompletedEvent{Victory = true});
    }

    private IEnumerator RespawnPlayer(GameObject playerObject)
    {
        yield return new WaitForSeconds(respawnTimer);
        EventDispatcher.Instance.Raise<PlayerSpawnedEvent>(new PlayerSpawnedEvent
            { PlayerObject = playerObject });
    }

    private void ProcessDefeat()
    {
        EventDispatcher.Instance.Raise(new GameCompletedEvent{Victory = false});  
    }
}