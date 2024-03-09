using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Controller : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private PlayerGun _gun;
    [SerializeField] private float _mouseSensitivity = 2f;
    private MultiplayerManager _multiplayerManager;

    private void Start()
    {
        _multiplayerManager = MultiplayerManager.Instance;
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        bool isShoot = Input.GetMouseButton(0);

        bool space = Input.GetKeyDown(KeyCode.Space);

        bool isCrouching = Input.GetKey(KeyCode.LeftControl);

        _player.SetInput(h, v, mouseX * _mouseSensitivity, isCrouching);
        _player.RotateX(-mouseY * _mouseSensitivity);
        if (space) _player.Jump();

        if (isShoot && _gun.TryShoot(out ShootInfo shootInfo)) SendShoot(ref shootInfo);

        SendMove();
    }

    private void SendShoot(ref ShootInfo shootInfo)
    {
        shootInfo.key = _multiplayerManager.GetSessionID();

        string json = JsonUtility.ToJson(shootInfo);

        _multiplayerManager.SendMessage("shoot", json);
    }

    private void SendMove()
    {
        _player.GetPlayerMove(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY, out bool isCrouching);

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"pX", position.x },
            {"pY", position.y },
            {"pZ", position.z },
            {"vX", velocity.x },
            {"vY", velocity.y },
            {"vZ", velocity.z },
            {"rX", rotateX },
            {"rY", rotateY }, 
            {"iC", isCrouching }
        };

        MultiplayerManager.Instance.SendMessage("move", data);
    }
}

public struct ShootInfo
{
    public string key;
    public float pX;
    public float pY;
    public float pZ;
    public float vX;
    public float vY;
    public float vZ;
}
