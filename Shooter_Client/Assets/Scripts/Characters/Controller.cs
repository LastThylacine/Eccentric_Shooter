using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Controller : MonoBehaviour
{
    [SerializeField] private float _restartDelay = 3f;
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private PlayerGunSwitch _gunSwitch;
    [SerializeField] private float _mouseSensitivity = 2f;
    private MultiplayerManager _multiplayerManager;
    private bool _hold;
    private bool _hideCursor;
    private PlayerGun _gun;
    private KeyCode[] keyCodes = {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
    };

    private void Start()
    {
        _multiplayerManager = MultiplayerManager.Instance;
        _hideCursor = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _hideCursor = !_hideCursor;
            Cursor.lockState = _hideCursor ? CursorLockMode.Locked : CursorLockMode.None;
        }

        if (_hold) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float mouseX = 0;
        float mouseY = 0;
        bool isShoot = false;

        if (_hideCursor)
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            isShoot = Input.GetMouseButton(0);
        }

        bool space = Input.GetKeyDown(KeyCode.Space);

        bool isCrouching = Input.GetKey(KeyCode.LeftControl);

        _player.SetInput(h, v, mouseX * _mouseSensitivity, isCrouching);
        _player.RotateX(-mouseY * _mouseSensitivity);
        if (space) _player.Jump();

        SendMove();

        if (!_gunSwitch.IsCanShoot) return;

        if (isShoot && _gun.TryShoot(out ShootInfo shootInfo)) SendShoot(ref shootInfo);

        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                StartCoroutine(_gunSwitch.RunSetGun(i));
            }
        }
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

    public void Restart(int index)
    {
        _multiplayerManager._spawnPoints.GetPoint(index, out Vector3 position, out Vector3 rotation);
        StartCoroutine(Hold());

        _player.transform.position = position;
        rotation.z = 0;
        rotation.x = 0;
        _player.transform.eulerAngles = rotation;
        _player.SetInput(0, 0, 0);
        SendMove();

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"pX", position.x },
            {"pY", position.y },
            {"pZ", position.z },
            {"vX", 0 },
            {"vY", 0 },
            {"vZ", 0 },
            {"rX", 0 },
            {"rY", rotation.y }
        };

        MultiplayerManager.Instance.SendMessage("move", data);
    }

    private IEnumerator Hold()
    {
        _hold = true;
        yield return new WaitForSecondsRealtime(_restartDelay);
        _hold = false;
    }

    public void SetGun(PlayerGun gun)
    {
        _gun = gun;
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

public struct RestartInfo
{
    public float x;
    public float y;
    public float z;
}
