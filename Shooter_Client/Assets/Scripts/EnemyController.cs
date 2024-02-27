using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _lerpRate = 15f;

    private Vector3 _nextPosition;

    internal void OnChange(List<DataChange> changes)
    {
        _nextPosition = transform.position;

        foreach (var dataChange in changes)
        {
            switch (dataChange.Field)
            {
                case "x":
                    _nextPosition.x = (float)dataChange.Value;
                    break;
                case "y":
                    _nextPosition.z = (float)dataChange.Value;
                    break;
                default:
                    Debug.LogWarning("Не обрабатывается поле с именем: " + dataChange.Field);
                    break;
            }
        }
    }

    private void Update()
    {
        if (_nextPosition != Vector3.zero)
            transform.position = Vector3.Lerp(transform.position, _nextPosition, _lerpRate * Time.deltaTime);
    }
}
