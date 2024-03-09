using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [field: SerializeField] public float speed { get; protected set; } = 2f;
    [field: SerializeField] public float crouchScaleFactor { get; protected set; } = 0.6f;
    public Vector3 velocity { get; protected set; }
    public bool isCrouching { get; protected set; }
}
