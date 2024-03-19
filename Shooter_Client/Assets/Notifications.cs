using System.Collections;
using UnityEngine;

public class Notifications : MonoBehaviour
{
    [SerializeField] private GameObject _headshot;

    private bool _isHeadshotRunning = false;

    public void RunHeadshot()
    {
        StartCoroutine(Headshot());
    }

    private IEnumerator Headshot()
    {
        if (_isHeadshotRunning) yield break;

        _isHeadshotRunning = true;

        _headshot.SetActive(true);

        yield return new WaitForSecondsRealtime(2.5f);

        _isHeadshotRunning = false;

        _headshot.SetActive(false);

        yield break;
    }
}
