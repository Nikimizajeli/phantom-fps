using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour, IWeapon
{
    [SerializeField] private int maxAmmo = 20;
    [SerializeField] private float reloadTime = 2.0f;
    [SerializeField] private float range = 40f;
    [SerializeField] private float damage = 2f;
    [SerializeField] private LayerMask targetsLayer;

    public bool CanFire => _currentAmmo > 0;

    private int _currentAmmo;
    private bool _reloading;

    protected void Start()
    {
        EventDispatcher.Instance.Raise<WeaponSelectedEvent>(new WeaponSelectedEvent { SelectedWeapon = this });
        SetCurrentAmmo(maxAmmo);
    }

    public void Fire()
    {
        if (_reloading)
        {
            return;
        }

        if (!CanFire)
        {
            Reload();
            return;
        }

        if (Camera.main == null)
        {
            return;
        }

        SetCurrentAmmo(_currentAmmo - 1);

        var mainCameraTransform = Camera.main.transform;
        if (Physics.Raycast(mainCameraTransform.position, mainCameraTransform.forward, out var hit, range,
                targetsLayer))
        {
            hit.transform.GetComponent<IDamageable>()?.TakeDamage(damage);
            Debug.Log($"Player hit {hit.transform.name} with {gameObject.name}. Ammo left: {_currentAmmo}");
        }
    }

    public void Reload()
    {
        _reloading = true;
        Invoke(nameof(OnFinishedReloading), reloadTime);
        Debug.Log("Player is reloading...");
    }

    public void OnFinishedReloading()
    {
        SetCurrentAmmo(maxAmmo);
        _reloading = false;
    }

    private void SetCurrentAmmo(int ammo)
    {
        _currentAmmo = ammo;
        EventDispatcher.Instance.Raise<PlayerAmmoUpdatedEvent>(
            new PlayerAmmoUpdatedEvent { CurrentAmmo = _currentAmmo });
    }
}