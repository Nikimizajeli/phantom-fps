using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject weaponRoot;

    private IDamageable _healthComponent;

    protected void Awake()
    {
        _healthComponent = GetComponent<IDamageable>();
    }

    protected void OnEnable()
    {
        _healthComponent.OnDurabilityThreshold += OnPlayerDeath;
        EventDispatcher.Instance.AddListener<PlayerSpawnedEvent>(OnPlayerSpawned);
    }

    protected void OnDisable()
    {
        _healthComponent.OnDurabilityThreshold -= OnPlayerDeath;
        EventDispatcher.Instance.RemoveListener<PlayerSpawnedEvent>(OnPlayerSpawned);
    }

    public void EnableMovement(bool enable)
    {
        playerMovement.enabled = enable;
    }

    private void OnPlayerDeath()
    {
        Debug.Log("Player died.");
        playerMovement.enabled = false;
        weaponRoot.SetActive(false);
        EventDispatcher.Instance.Raise<PlayerDeathEvent>(new PlayerDeathEvent { PlayerObject = gameObject });
    }

    private void OnPlayerSpawned(PlayerSpawnedEvent ev)
    {
        // wait a second to simulate some spawning animation 
        StartCoroutine(UnlockPlayerMovement(1f));
    }
    
    private IEnumerator UnlockPlayerMovement(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerMovement.enabled = true;
        weaponRoot.SetActive(true);
    }
}