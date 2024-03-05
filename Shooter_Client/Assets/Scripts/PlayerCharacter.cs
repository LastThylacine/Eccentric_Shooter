using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _speed = 2f;

    private float _inputH;
    private float _inputV;

    private void FixedUpdate()
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
        //Vector3 direction = new Vector3(_inputH, 0f, _inputV);
        //transform.position += direction * Time.deltaTime * _speed;

        Vector3 velocity = (transform.forward * _inputV + transform.right * _inputH).normalized * _speed;
        _rigidbody.velocity = velocity;
    }

    public void GetPlayerMove(out Vector3 position)
    {
        position = transform.position;
    }
}
