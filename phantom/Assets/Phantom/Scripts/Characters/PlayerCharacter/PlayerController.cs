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
    }

    protected void OnDisable()
    {
        _healthComponent.OnDurabilityThreshold -= OnPlayerDeath;
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
    }

    private void OnPlayerRespawn()
    {
        playerMovement.enabled = true;
        weaponRoot.SetActive(true);
    }

}
