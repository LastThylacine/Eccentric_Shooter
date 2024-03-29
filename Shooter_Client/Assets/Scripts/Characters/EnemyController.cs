using Colyseus.Schema;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyGunSwitch _gunSwitch;
    [SerializeField] private EnemyCharacter _character;
    private EnemyGun _gun;
    private List<float> _receiveTimeInterval = new List<float> { 0, 0, 0, 0, 0 };
    private float AverageInterval
    {
        get
        {
            int receiveTimeIntervalCount = _receiveTimeInterval.Count;
            float sum = 0;
            for (int i = 0; i < receiveTimeIntervalCount; i++)
            {
                sum += _receiveTimeInterval[i];
            }

            return sum / receiveTimeIntervalCount;
        }
    }
    private float _lastReceiveTime = 0;
    private Player _player;

    public void Init(string key, Player player)
    {
        _character.Init(key);

        _player = player;
        _character.SetSpeed(player.speed);
        _character.SetMaxHP(player.maxHP);
        _character.SetCrouchScaleFactor(player.crouch);
        player.OnChange += OnChange;
    }

    public void Shoot(in ShootInfo info)
    {
        Vector3 position = new Vector3(info.pX, info.pY, info.pZ);
        Vector3 velocity = new Vector3(info.vX, info.vY, info.vZ);

        _gun.Shoot(position, velocity);
    }

    public void Destroy()
    {
        _player.OnChange -= OnChange;
        Destroy(gameObject);
    }

    private void SaveReceiveTime()
    {
        float interval = Time.time - _lastReceiveTime;
        _lastReceiveTime = Time.time;

        _receiveTimeInterval.Add(interval);
        _receiveTimeInterval.RemoveAt(0);
    }

    internal void OnChange(List<DataChange> changes)
    {
        SaveReceiveTime();

        Vector3 position = transform.position;
        //Vector3 position = _character.targetPosition;
        Vector3 velocity = _character.velocity;
        bool isCrouching = _character.isCrouching;

        foreach (var dataChange in changes)
        {
            switch (dataChange.Field)
            {
                case "loss":
                    MultiplayerManager.Instance._lossCounter.SetEnemyLoss((byte)dataChange.Value);
                    break;
                case "currentHP":
                    if ((sbyte)dataChange.Value > (sbyte)dataChange.PreviousValue)
                        _character.RestoreHP((sbyte)dataChange.Value);
                    break;
                case "pX":
                    position.x = (float)dataChange.Value;
                    break;
                case "pY":
                    position.y = (float)dataChange.Value;
                    break;
                case "pZ":
                    position.z = (float)dataChange.Value;
                    break;
                case "vX":
                    velocity.x = (float)dataChange.Value;
                    break;
                case "vY":
                    velocity.y = (float)dataChange.Value;
                    break;
                case "vZ":
                    velocity.z = (float)dataChange.Value;
                    break;
                case "rX":
                    _character.SetRotateX((float)dataChange.Value);
                    break;
                case "rY":
                    _character.SetRotateY((float)dataChange.Value);
                    break;
                case "iC":
                    isCrouching = (bool)dataChange.Value;
                    break;
                default:
                    Debug.LogWarning("�� �������������� ���� � ������: " + dataChange.Field);
                    break;
            }
        }

        _character.SetMovement(position, velocity, AverageInterval, isCrouching);
    }

    public void SetGun(EnemyGun gun)
    {
        _gun = gun;
    }
}
