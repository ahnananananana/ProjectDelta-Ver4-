using System.Collections;
using UnityEngine;

public class hRecordTime : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text _time;
    private float _record;
    private IEnumerator _coroutine;
    private bool _isStart;

    public float record => _record;

    private void Awake()
    {
        _record = 0;
        _time.text = _record.ToString("F2");

        hGameManager.current.pauseEvent += Stop;
        hGameManager.current.resumeEvent += Launch;
        hGameBall.flyEvent += () => { if (!_isStart) { _isStart = true; Launch(); } };
        hGameManager.current.restartEvent += () => { Stop(); hGameBall.flyEvent += () => { if (!_isStart) { _isStart = true; Launch(); } }; _isStart = false; _record = 0; _time.text = _record.ToString("F2"); };
    }

    private IEnumerator StartRecord()
    {
        while (true)
        {
            _record += hTime.fixedDeltaTime;
            _record = (float)System.Math.Round(_record, 2);
            _time.text = _record.ToString("F2");
            yield return new WaitForFixedUpdate();
        }
    }

    public void Launch()
    {
        if (!_isStart) return;
        _coroutine = StartRecord();
        StartCoroutine(_coroutine);
    }

    public void Stop()
    {
        if (!_isStart) return;
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}
