using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hLevelStartBtn : hButton
{
    private hLevel _level;

    public hLevel level { get => _level; set => _level = value; }

    public override void Click()
    {
        if (_isClicked) return;
        _isClicked = true;
        hSharedData.nextSceneNum = 2;
        hSharedData.curLevel = _level;
        SceneManager.LoadScene(0, LoadSceneMode.Additive);
    }

}
