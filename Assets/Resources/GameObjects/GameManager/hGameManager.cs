using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

public class hGameManager : MonoBehaviour
{
    private static hGameManager _instance;
    public static hGameManager current
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<hGameManager>();
            return _instance;
        }
    }
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private hLevelSetter _levelSetter;
    [SerializeField]
    private bool _isDev;
    [SerializeField]
    private Difficulty _devDifficulty;
    [SerializeField]
    private int _devLevel;
    private hLevel _serializedLevel;
    [SerializeField]
    private hRecordTime recordTime;

    [SerializeField]
    private Transform _skyBox;
    [SerializeField]
    private Volume _globalVolume;
    [SerializeField]
    private ParticleSystem _backgroundDust;
    private bool _isPause, _isEnd, _isFirst = true;

    private hLevel _curLevel;
    private hGameBall _gameBall;

    private event DelVoid _pauseEvent, _resumeEvent, _loseEvent, _winEvent, _restartEvent;

    public event DelVoid pauseEvent { add { _pauseEvent += value; } remove { _pauseEvent -= value; } }
    public event DelVoid resumeEvent { add { _resumeEvent += value; } remove { _resumeEvent -= value; } }
    public event DelVoid loseEvent { add { _loseEvent += value; } remove { _loseEvent -= value; } }
    public event DelVoid winEvent { add { _winEvent += value; } remove { _winEvent -= value; } }
    public event DelVoid restartEvent { add { _restartEvent += value; } remove { _restartEvent -= value; } }

    public bool isPause { get => _isPause; set => _isPause = value; }
    public Transform skyBox => _skyBox;

    private void Awake()
    {
        _instance = this;
        //hSharedData.curLevel = hSharedData.GetLevels(_devDifficulty)[_devLevel];
        _serializedLevel = hSharedData.curLevel;
        if (_isDev)
        {
           /* StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(Application.dataPath);
            stringBuilder.Append("/SerializedLevels/");
            stringBuilder.Append(_devDifficulty);
            stringBuilder.Append("_");
            stringBuilder.Append(_devLevel);
            stringBuilder.Append(".bytes");

            FileStream stream = new FileStream(stringBuilder.ToString(), FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            _serializedLevel = (hLevel)formatter.Deserialize(stream);
            stream.Close();*/
        }


        InitGame();
    }

    private void Start()
    {
        var shape = _backgroundDust.shape;
        //var levelPos = _curLevel.transform.position + _curLevel.GetComponent<BoxCollider>().center;
        var dir = _camera.transform.forward;
        var dis = 10f;
        var offset = Vector3.Distance(_camera.transform.position, _curLevel.transform.position);
        _backgroundDust.transform.position = _camera.transform.position +  dir * (dis + offset);

        var ratio = Screen.width / (float)Screen.height;

        shape.scale = new Vector3(ratio * _camera.orthographicSize * 2, _camera.orthographicSize * 2, 1);
    }

    private void OnDestroy()
    {
        hWinZone.loseEvent -= Lose;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_isEnd)
        {
            if (_isPause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void InitGame()
    {
        _curLevel = _levelSetter.CreateLevel(_serializedLevel);
        _levelSetter.AdjustCamera(_camera, _curLevel, _skyBox);

        _gameBall = _curLevel.gameBall;
        _gameBall.deadEvent += Lose;
        _gameBall.goalEvent += PlayerGoal;

        hWinZone.loseEvent += Lose;

        _globalVolume.weight = 0;


        hTime.fixedDeltaTime = Time.fixedDeltaTime;
    }

    public void Restart()
    {
        ++hAdManager.current.clearTime;
        _curLevel.gameObject.SetActive(false);
        DestroyImmediate(_curLevel.gameObject);
        InitGame();
        ResumeGame();
        _restartEvent?.Invoke();
    }

    public void PauseGame()
    {
        _isPause = true;
        hTime.fixedDeltaTime = 0f;
        _pauseEvent?.Invoke();
    }

    public void ResumeGame()
    {
        _isPause = false;
        hTime.fixedDeltaTime = Time.fixedDeltaTime;
        _resumeEvent?.Invoke();
    }

    private void Lose()
    {
        //popup regame or not?
        PauseGame();
        _isEnd = true;
        _loseEvent?.Invoke();
    }

    private void PlayerGoal()
    {
        PauseGame();
        _isEnd = true;
        _winEvent?.Invoke();
        switch (hSharedData.curDifficulty)
        {
            case Difficulty.NORMAL:
                if(PlayerPrefs.GetInt("NormalLevel", 0) < hSharedData.curLevel.levelNum)
                    PlayerPrefs.SetInt("NormalLevel", hSharedData.curLevel.levelNum);
                break;
            case Difficulty.HARD:
                if (PlayerPrefs.GetInt("HardLevel", 0) < hSharedData.curLevel.levelNum)
                    PlayerPrefs.SetInt("HardLevel", hSharedData.curLevel.levelNum);
                break;
            case Difficulty.EXTREME:
                if (PlayerPrefs.GetInt("ExtremeLevel", 0) < hSharedData.curLevel.levelNum)
                    PlayerPrefs.SetInt("ExtremeLevel", hSharedData.curLevel.levelNum);
                break;
        }
        StartCoroutine(EnableBloom());
        float record = PlayerPrefs.GetFloat(_curLevel.difficulty.ToString() + _curLevel.levelNum, float.MaxValue);
        if (record == 0f || (record > recordTime.record))
            PlayerPrefs.SetFloat(_curLevel.difficulty.ToString() + _curLevel.levelNum, recordTime.record);
        //StartCoroutine(RefreshRecord());
    }

    private IEnumerator RefreshRecord()
    {
        yield return null;
        /*List<hUserRecord> records = new List<hUserRecord>();
        yield return hDatabase.current.GetLevelRecords(_curLevel.difficulty, _curLevel.levelNum, records);
        hUserRecord oldRecord;

        int index = records.FindIndex((data) => { if (data.id == hSharedData.userId && data.userName == hSharedData.userName) return true; return false; });
        if (index >= 0)
        {
            oldRecord = records[index];
            if(recordTime.record < oldRecord.record)
            {
                SendRecord(new hUserRecord(hSharedData.userId, hSharedData.userName, recordTime.record));
            }
        }
        else
        {
            var userRecord = new hUserRecord(hSharedData.userId, hSharedData.userName, recordTime.record);
            hDatabase.current.AddRecord(
                ((int)_curLevel.difficulty).ToString() + _curLevel.levelNum.ToString(),
                userRecord
                );
            SendRecord(userRecord);
        }*/
    }

    private void SendRecord(hUserRecord userRecord)
    {
        /*var url = hNetworkManager.serverURL + "/ranking/" + ((int)_curLevel.difficulty).ToString() + "/" + _curLevel.levelNum;
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        string data = userRecord.id + '`' + userRecord.userName + '`' + userRecord.record.ToString("F2");
        formData.Add(new MultipartFormDataSection(data));
        UnityWebRequest.Post(url, formData).SendWebRequest();*/
    }

    private IEnumerator EnableBloom()
    {
        _globalVolume.gameObject.SetActive(true);
        while (_globalVolume.weight < .95f)
        {
            _globalVolume.weight += Time.deltaTime;
            yield return null;
        }
        _globalVolume.weight = 1f;
    }
}
