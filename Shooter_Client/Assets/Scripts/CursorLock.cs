using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLock : MonoBehaviour
{
    private bool _isLocked;

    private void Start()
    {
        _isLocked = false;
        Toggle();
    }

    private void Update()
    {
        if (_isLocked && Input.GetKeyDown(KeyCode.Escape)) Toggle();

        if(!_isLocked && Input.GetMouseButtonDown(0)) Toggle();
    }

    private void Toggle()
    {
        _isLocked = !_isLocked;

        Cursor.visible = _isLocked ? false : true;
        Cursor.lockState = _isLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
