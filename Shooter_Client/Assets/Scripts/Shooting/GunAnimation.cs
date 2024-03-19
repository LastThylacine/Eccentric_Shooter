using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimation : MonoBehaviour
{
    private const string shoot = "Shoot";
    private const string deselect = "Deselect";
    private const string select = "Select";

    [SerializeField] private Gun _gun;
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

    public void Select()
    {
        _animator.SetTrigger(select);
    }

    public void Deselect()
    {
        _animator.SetTrigger(deselect);
    }

    public void ResetAnimator()
    {
        _animator.Rebind();
    }
}
