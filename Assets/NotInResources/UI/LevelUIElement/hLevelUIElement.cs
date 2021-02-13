using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class hLevelUIElement : hButton
{
    private hLevel _level;
    private Texture2D _thumbnail;
    private bool _isClear = false;
    private bool _isActive = false;

    [SerializeField]
    private Image _backImage, _lockImage;
    [SerializeField]
    private TMPro.TMP_Text _text;
    private hLevelSelectPopup _levelSelectPopup;

    public TMPro.TMP_Text text => _text;
    public hLevel level { get => _level; set => _level = value; }
    public bool isClear { get => _isClear; set => _isClear = value; }
    public bool isActive
    {
        get => _isActive; 
        set
        {
            _isActive = value;
            if (_isActive)
            {
                _button.interactable = true;
                var color = _backImage.color;
                color.a = .275f;
                _backImage.color = color;
                _lockImage.enabled = false;
                _text.enabled = true;
            }
            else
            {
                _button.interactable = false;
                var color = _backImage.color;
                color.a = .08f;
                _backImage.color = color;
                _lockImage.enabled = true;
                _text.enabled = false;
            }
        }
    }

    public hLevelSelectPopup levelSelectPopup { get => _levelSelectPopup; set => _levelSelectPopup = value; }

    public void Set(hLevel level)
    {
        _level = level;
        _text.text = level.levelNum.ToString();
    }

    public void SelectLevel()
    {
        if (_isClicked) return;
        _isClicked = true;
        hSharedData.nextSceneNum = 2;
        hSharedData.curLevel = _level;
        SceneManager.LoadScene(0, LoadSceneMode.Additive);
    }

    public override void Click()
    {
        _levelSelectPopup.SetUp(_level);
    }
}
