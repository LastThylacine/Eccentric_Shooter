using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField] private Transform _head;
    [SerializeField] private float _rotationSmoothness = 25f;
    [SerializeField] private float _crouchSmoothness = 15f;
    public Vector3 targetPosition { get; private set; } = Vector3.zero;
    private float _velocityMagnitude = 0;
    private float _targetRotationX = 0;
    private float _targetRotationY = 0;

    private void Start()
    {
        targetPosition = transform.position;
        _targetRotationX = _head.localEulerAngles.x;
        _targetRotationY = transform.localEulerAngles.y;
    }

    private void Update()
    {
        if (_velocityMagnitude > 0.1f)
        {
            float maxDistance = _velocityMagnitude * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxDistance);
        }
        else
        {
            transform.position = targetPosition;
        }

        float crouchingLerpRate = _rotationSmoothness * Time.deltaTime;

        Vector3 newHeadRotation = _head.localEulerAngles;
        newHeadRotation.x = Mathf.LerpAngle(_head.localEulerAngles.x, _targetRotationX, crouchingLerpRate);
        _head.localEulerAngles = newHeadRotation;

        Vector3 newBodyRotation = transform.localEulerAngles;
        newBodyRotation.y = Mathf.LerpAngle(transform.localEulerAngles.y, _targetRotationY, crouchingLerpRate);
        transform.localEulerAngles = newBodyRotation;

        if (isCrouching)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * crouchScaleFactor, _crouchSmoothness * Time.deltaTime);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, _crouchSmoothness * Time.deltaTime);
        }
    }

    public void SetCrouchScaleFactor(float value) => crouchScaleFactor = value;

    public void SetSpeed(float value) => speed = value;

    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval, in bool isCrouching)
    {
        targetPosition = position + (velocity * averageInterval);
        _velocityMagnitude = velocity.magnitude;

        base.velocity = velocity;
        base.isCrouching = isCrouching;
    }

    public void SetRotateX(float value)
    {
        _targetRotationX = value;
    }

    public void SetRotateY(float value)
    {
        _targetRotationY = value;
    }
}
