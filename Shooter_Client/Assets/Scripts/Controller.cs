using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private float _mouseSensitivity = 2f;

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        bool space = Input.GetKeyDown(KeyCode.Space);

        _player.SetInput(h, v, mouseX * _mouseSensitivity);
        _player.RotateX(-mouseY * _mouseSensitivity);
        if (space) _player.Jump();

        SendMove();
    }

    private void SendMove()
    {
        _player.GetPlayerMove(out Vector3 position, out Vector3 velocity);

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"pX", position.x },
            {"pY", position.y },
            {"pZ", position.z },
            {"vX", velocity.x },
            {"vY", velocity.y },
            {"vZ", velocity.z }
        };

        MultiplayerManager.Instance.SendMessage("move", data);
    }
}
