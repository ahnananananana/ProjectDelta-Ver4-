using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hLevelContainer : MonoBehaviour
{
    [SerializeField]
    private hLevelUIElement _stageElementPrefab;
    [SerializeField]
    private GameObject _normalLevels, _hardLevels, _extremeLevels;
    private bool _isClicked;
    [SerializeField]
    hLevelSelectPopup _levelSelectPopup;

    private void Awake()
    {
        InitLevel(Difficulty.NORMAL);
        InitLevel(Difficulty.HARD);
        InitLevel(Difficulty.EXTREME);

        _normalLevels.gameObject.SetActive(false);
        _hardLevels.gameObject.SetActive(false);
        _extremeLevels.gameObject.SetActive(false);
    }

    private void InitLevel(Difficulty difficulty)
    {
        List<hLevel> levels = null;
        Transform container = null;
        int curLevelNum = 0;

        switch (difficulty)
        {
            case Difficulty.NORMAL:
                levels = hSharedData.GetLevels(Difficulty.NORMAL);
                curLevelNum = PlayerPrefs.GetInt("NormalLevel", 0);
                container = _normalLevels.transform;
                break;
            case Difficulty.HARD:
                levels = hSharedData.GetLevels(Difficulty.HARD);
                curLevelNum = PlayerPrefs.GetInt("HardLevel", 0);
                container = _hardLevels.transform;
                break;
            case Difficulty.EXTREME:
                levels = hSharedData.GetLevels(Difficulty.EXTREME);
                curLevelNum = PlayerPrefs.GetInt("ExtremeLevel", 0);
                container = _extremeLevels.transform;
                break;
        }

        for (int i = 0; i < levels.Count; ++i)
        {
            var element = Instantiate(_stageElementPrefab);
            element.Set(levels[i]);
            if (i <= curLevelNum)
                element.isActive = true;
            element.transform.SetParent(container);
            element.transform.localScale = Vector3.one;
            element.transform.localPosition = Vector3.zero;
            element.transform.localRotation = Quaternion.identity;
            element.levelSelectPopup = _levelSelectPopup;
        }
    }

    public void SetLevels(Difficulty difficulty)
    {
        _normalLevels.SetActive(false);
        _hardLevels.SetActive(false);
        _extremeLevels.SetActive(false);
        switch (difficulty)
        {
            case Difficulty.NORMAL:
                _normalLevels.SetActive(true);
                break;
            case Difficulty.HARD:
                _hardLevels.SetActive(true);
                break;
            case Difficulty.EXTREME:
                _extremeLevels.SetActive(true);
                break;
        }
    }
}
