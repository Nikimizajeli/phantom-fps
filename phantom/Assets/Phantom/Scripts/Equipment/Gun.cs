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

    private Transform _mainCameraTransform;

    private Transform MainCameraTransform
    {
        get
        {
            // TODO: Camera.main uses tags, find better way
            if (_mainCameraTransform == null && Camera.main != null)
            {
                _mainCameraTransform = Camera.main.transform;
            }

            return _mainCameraTransform;
        }
    }

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

        if (MainCameraTransform == null)
        {
            Debug.LogError("Cannot find targets without camera");
            return;
        }

        SetCurrentAmmo(_currentAmmo - 1);

        if (Physics.Raycast(MainCameraTransform.position, MainCameraTransform.forward, out var hit, range,
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