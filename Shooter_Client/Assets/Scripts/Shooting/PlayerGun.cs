using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _bulletPoint;
    [SerializeField] private float _bulletSpeed = 10f;
    [SerializeField] private float _shootDelay;
    private float _lastShootTime;

    public Action shoot;

    public bool TryShoot(out ShootInfo info)
    {
        info = new ShootInfo();

        if (Time.time - _lastShootTime < _shootDelay) return false;

        Vector3 position = _bulletPoint.position;
        Vector3 direction = _bulletPoint.forward;

        _lastShootTime = Time.time;
        Instantiate(_bulletPrefab, position, _bulletPoint.rotation).Init(direction, _bulletSpeed);
        shoot?.Invoke();

        direction *= _bulletSpeed;

        info.pX = position.x;
        info.pY = position.y;
        info.pZ = position.z;
        info.vX = direction.x;
        info.vY = direction.y;
        info.vZ = direction.z;

        return true;
    }
}
