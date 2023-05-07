using System;
using UnityEngine;

public class CharacterHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 6;

    private int _currentHealth;

    protected void Start()
    {
        _currentHealth = maxHealth;
    }

    #region IDamageable

    public event Action OnDamageTaken;
    public event Action OnDurabilityThreshold;

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        OnDamageTaken?.Invoke();

        if (_currentHealth <= 0)
        {
            OnDurabilityThreshold?.Invoke();
        }
    }

    #endregion
}