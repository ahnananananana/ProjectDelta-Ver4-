using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class hRetryBtn : hButton
{
    public override void Click()
    {
        if (_isClicked) return;
        _isClicked = true;
        hGameManager.current.Restart();
        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
