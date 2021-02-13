using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hSoundBtn : hButton
{
    [SerializeField]
    private Image _img;
    [SerializeField]
    private Sprite _muteImg, _unmuteImg;
    private bool _isMute;

    private bool isMute
    {
        get => _isMute;
        set
        {
            _isMute = value;
            if(_isMute)
            {
                _img.sprite = _muteImg;
                hBGMController.current.Stop();
                PlayerPrefs.SetInt("IsMute", 1);
            }
            else
            {
                _img.sprite = _unmuteImg;
                hBGMController.current.Play();
                PlayerPrefs.SetInt("IsMute", 0);
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        isMute = System.Convert.ToBoolean(PlayerPrefs.GetInt("IsMute", 0));
    }

    public override void Click() => isMute = !_isMute;
}
