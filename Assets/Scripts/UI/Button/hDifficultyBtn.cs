using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hDifficultyBtn : hButton
{
    [SerializeField]
    private Difficulty _difficulty;
    [SerializeField]
    private hMenuController _menuController;
    [SerializeField]
    private TMPro.TMP_Text _complete;
    [SerializeField]
    private Image _backImage, _lockImage;
    private bool _isActive;

    public bool isActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
            var color = _backImage.color;
            color.a = .275f;
            _backImage.color = color;
            if (_isActive)
            {
                _button.interactable = true;
                _lockImage.enabled = false;
                _complete.enabled = true;
            }
            else
            {
                _button.interactable = false;
                _lockImage.enabled = true;
                _complete.enabled = false;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();

        int levelNum = hSharedData.GetLevels(_difficulty).Count;

        int clearNum = 0;
        switch(_difficulty)
        {
            case Difficulty.NORMAL:
                {
                    clearNum = PlayerPrefs.GetInt("NormalLevel", 0);
                    isActive = true;
                }
                break;
            case Difficulty.HARD:
                {
                    clearNum = PlayerPrefs.GetInt("HardLevel", 0);
                    int previousLevelClearNum = hSharedData.GetLevels(Difficulty.NORMAL).Count;
                    if (previousLevelClearNum <= PlayerPrefs.GetInt("NormalLevel", 0))
                        isActive = true;
                    else
                        isActive = false;
                    break;
                }
            case Difficulty.EXTREME:
                {
                    clearNum = PlayerPrefs.GetInt("ExtremeLevel", 0);
                    int previousLevelClearNum = hSharedData.GetLevels(Difficulty.HARD).Count;
                    if (previousLevelClearNum <= PlayerPrefs.GetInt("HardLevel", 0))
                        isActive = true;
                    else
                        isActive = false;
                    break;
                }
        }
        if (levelNum == 0)
            _complete.text = "0";
        else
            _complete.text = "COMPLETE " + Mathf.Clamp((clearNum * 100 / levelNum), 0, 100) + "%";
    }

    public override void Click()
    {
        _menuController.GoLevelMenu(_difficulty);
        hColorManager.current.SetTheme(_difficulty);
    }
}
