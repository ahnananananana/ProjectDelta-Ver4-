using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hHomeBtn : hButton
{
    public override void Click()
    {
        if (_isClicked) return;
        _isClicked = true;
        hSharedData.nextSceneNum = 1;
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);
    }
}
