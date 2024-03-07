using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimation : MonoBehaviour
{
    private const string shoot = "Shoot";

    [SerializeField] private PlayerGun _gun;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        _gun.shoot += Shoot;
    }

    private void OnDestroy()
    {
        _gun.shoot -= Shoot;
    }

    private void Shoot()
    {
        _animator.SetTrigger(shoot);
    }
}
