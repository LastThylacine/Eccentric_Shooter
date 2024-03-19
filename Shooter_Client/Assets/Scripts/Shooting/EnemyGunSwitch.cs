using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunSwitch : MonoBehaviour
{
    [SerializeField] private EnemyController _controller;
    [SerializeField] private List<EnemyGun> _guns;
    private GunAnimation _gunAnimation;

    private void Awake()
    {
        SetGun(0);
    }

    public void SetGun(int currentGunIndex)
    {
        for (int i = 0; i < _guns.Count; i++)
        {
            _guns[i].gameObject.SetActive(false);
        }

        EnemyGun currentGun = _guns[currentGunIndex];

        currentGun.gameObject.SetActive(true);

        _controller.SetGun(currentGun);

        _gunAnimation = _guns[currentGunIndex].GetComponent<GunAnimation>();
        _gunAnimation.Select();
    }
}
