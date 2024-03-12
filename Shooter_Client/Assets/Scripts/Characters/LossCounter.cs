using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LossCounter : MonoBehaviour
{
    [SerializeField] private Text _text;
    private int _enemyLoss;
    private int _playerLoss;

    public void SetEnemyLoss(int value)
    {
        _enemyLoss = value;
        UpdateLossText();
    }

    public void SetPlayerLoss(int value)
    {
        _playerLoss = value;
        UpdateLossText();
    }

    private void UpdateLossText()
    {
        _text.text = $"{_playerLoss} : {_enemyLoss}";
    }
}
