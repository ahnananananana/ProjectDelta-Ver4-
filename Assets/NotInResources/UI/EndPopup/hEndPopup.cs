using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hEndPopup : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text _statgeName, _result;

    private void Awake()
    {
        hGameManager.current.loseEvent += () => { Set(false); };
        hGameManager.current.winEvent += () => { Set(true); };
        hGameManager.current.restartEvent += () => {
            _statgeName.alpha = 0f;
            _result.alpha = 0f;
        };
        _statgeName.alpha = 0f;
        _result.alpha = 0f;
    }

    public void Set(bool isClear)
    {
        StartCoroutine(FadeIn());
        switch ((Difficulty)hSharedData.curLevel.difficulty)
        {
            case Difficulty.NORMAL:
                _statgeName.text = "NORMAL ";
                break;
            case Difficulty.HARD:
                _statgeName.text = "HARD ";
                break;
            case Difficulty.EXTREME:
                _statgeName.text = "EXTREME ";
                break;
        }
        _statgeName.text += hSharedData.curLevel.levelNum.ToString();

        if (isClear)
            _result.text = "CLEAR";
        else
            _result.text = "FAIL";
    }

    private IEnumerator FadeIn()
    {
        while(_statgeName.alpha < .95f && _result.alpha < .95f)
        {
            _statgeName.alpha += hTime.deltaTime * 2;
            _result.alpha += hTime.deltaTime * 2;
            yield return null;
        }
        _statgeName.alpha = 1f;
        _result.alpha = 1f;
    }
}
