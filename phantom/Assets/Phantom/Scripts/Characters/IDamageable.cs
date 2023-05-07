using System;
using UnityEngine;

public interface IDamageable
{
    public event Action OnDamageTaken;
    public event Action OnDurabilityThreshold;
    public void TakeDamage(int damage);
}
