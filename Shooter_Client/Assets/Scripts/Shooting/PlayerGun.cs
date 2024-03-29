using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : Gun
{
    [SerializeField] private int _damage;
    [SerializeField] private Transform _bulletPoint;
    [SerializeField] private float _bulletSpeed = 10f;
    [SerializeField] private float _shootDelay;
    private float _lastShootTime;
    private Notifications _notifications;

    private void Start()
    {
        _notifications = FindObjectOfType<Notifications>();
    }

    public bool TryShoot(out ShootInfo info)
    {
        info = new ShootInfo();

        if (Time.time - _lastShootTime < _shootDelay) return false;

        Vector3 position = _bulletPoint.position;
        Vector3 velocity = _bulletPoint.forward * _bulletSpeed;

        _lastShootTime = Time.time;
        Instantiate(_bulletPrefab, position, _bulletPoint.rotation).Init(velocity, _damage, _notifications);
        shoot?.Invoke();

        info.pX = position.x;
        info.pY = position.y;
        info.pZ = position.z;
        info.vX = velocity.x;
        info.vY = velocity.y;
        info.vZ = velocity.z;

        return true;
    }
}
