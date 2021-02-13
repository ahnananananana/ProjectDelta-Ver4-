using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hPauseBtn : hButton
{
    public override void Click()
    {
        if (_isClicked) return;
        _isClicked = true;
        hGameManager.current.PauseGame();
    }
}
