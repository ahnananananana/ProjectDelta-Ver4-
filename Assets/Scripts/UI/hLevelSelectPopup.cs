using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hLevelSelectPopup : MonoBehaviour
{
    [SerializeField]
    private GameObject _clickMask;
    [SerializeField]
    private hLeaderBoard m_leaderboard;
    private hLevel _selectedLevel;
    [SerializeField]
    private hLevelStartBtn _levelStartBtn;
    [SerializeField]
    private Image _thumbnail;
    [SerializeField]
    private TMPro.TMP_Text _bestRecordID, _bestRecord, _myRecord;

    private void Awake()
    {
        gameObject.SetActive(false);
        _thumbnail.preserveAspect = true;
    }

    private void OnEnable() => _clickMask.SetActive(true);
    private void OnDisable() => _clickMask.SetActive(false);


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            StartCoroutine(Close());
    }

    public void OpenLeaderBoard() => m_leaderboard.Open(_selectedLevel);

    public void SetUp(hLevel level)
    {
        gameObject.SetActive(true);
        _selectedLevel = level;
        _levelStartBtn.level = level;

        /*Texture2D texture = new Texture2D((int)_thumbnail.rectTransform.rect.width, (int)_thumbnail.rectTransform.rect.height, TextureFormat.RGBA32, false);
        texture.LoadImage(System.IO.File.ReadAllBytes(Application.persistentDataPath + "/Thumbnails/" + ((Difficulty)level.difficulty).ToString() + "_" + level.levelNum.ToString() + ".png"));
        
        _thumbnail.texture = texture;*/
        _thumbnail.sprite = level.Thumbnail;
        StartCoroutine(SetRecords(level));
    }

    public IEnumerator Close()
    {
        yield return null;
        gameObject.SetActive(false);
    }

    private IEnumerator SetRecords(hLevel level = default)
    {
        float record = PlayerPrefs.GetFloat(level.difficulty.ToString() + level.levelNum, 0f);
        _bestRecord.text = string.Format("{0:0.00}", record);
        if (level.levelNum == -1)
            level = _selectedLevel;
        if (level.levelNum == -1)
        {
            //Debug.LogError("No level!");
            yield break;
        }
        yield break;

        /*List<hUserRecord> userRecords = new List<hUserRecord>();
        var op = hDatabase.current.GetLevelRecords((Difficulty)level.difficulty, level.levelNum, userRecords);
        while (op != null && !op.isDone)
            yield return null;

        if (userRecords.Count > 0)
        {
            _bestRecordID.text = userRecords[0].userName;
            _bestRecord.text = userRecords[0].record.ToString("F2");
            var userData = userRecords.Find((data) => { if (data.id == hSharedData.userId && data.userName == hSharedData.userName) return true; return false; });
            if (userData.id == default)
                _myRecord.text = " - ";
            else
                _myRecord.text = userData.record.ToString("F2");
        }
        else
        {
            _bestRecordID.text = " - ";
            _bestRecord.text = " - ";
            _myRecord.text = " - ";
        }*/
    }
}
