using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;

    private float _inputH;
    private float _inputV;

    private void Update()
    {
        Move();
    }

    public void SetInput(float inputH, float inputV)
    {
        _inputH = inputH;
        _inputV = inputV;
    }

    private void Move()
    {
        Vector3 direction = new Vector3(_inputH, 0f, _inputV);

        transform.position += direction * Time.deltaTime * _speed;
    }

    public void GetPlayerMove(out Vector3 position)
    {
        position = transform.position;
    }
}
