using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hContinueBtn : hButton
{

    public override void Click()
    {
        if (_isClicked) return;
        _isClicked = true;

        hGameManager.current.ResumeGame();
    }
}
