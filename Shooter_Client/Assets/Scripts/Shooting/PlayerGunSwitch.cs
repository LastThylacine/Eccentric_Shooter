using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunSwitch : MonoBehaviour
{
    [SerializeField] private Controller _controller;
    [SerializeField] private List<PlayerGun> _guns;
    private int _currentGunIndex;
    private GunAnimation _gunAnimation;

    public bool IsCanShoot { get; private set; }

    private void Awake()
    {
        IsCanShoot = true;
        SetGun(0);
    }

    public IEnumerator RunSetGun(int currentGunIndex)
    {
        if (_currentGunIndex == currentGunIndex) yield break;

        IsCanShoot = false;

        _gunAnimation = _guns[_currentGunIndex].GetComponent<GunAnimation>();
        _gunAnimation.Deselect();

        yield return new WaitForSecondsRealtime(0.16f);

        _gunAnimation.ResetAnimator();

        SetGun(currentGunIndex);

        _gunAnimation = _guns[currentGunIndex].GetComponent<GunAnimation>();
        _gunAnimation.Select();

        yield return new WaitForSecondsRealtime(0.16f);

        IsCanShoot = true;
    }

    private void SetGun(int currentGunIndex)
    {
        _currentGunIndex = currentGunIndex;

        for (int i = 0; i < _guns.Count; i++)
        {
            _guns[i].gameObject.SetActive(false);
        }

        PlayerGun newGun = _guns[_currentGunIndex];

        newGun.gameObject.SetActive(true);
        _controller.SetGun(newGun);

        GunInfo info = new GunInfo();
        info.gunID = _currentGunIndex;
        SendGun(ref info);
    }

    private void SendGun(ref GunInfo gunInfo)
    {
        gunInfo.key = MultiplayerManager.Instance.GetSessionID();

        string json = JsonUtility.ToJson(gunInfo);

        MultiplayerManager.Instance.SendMessage("gun", json);
    }
}

public struct GunInfo
{
    public string key;
    public int gunID;
}