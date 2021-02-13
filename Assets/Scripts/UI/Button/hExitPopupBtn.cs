using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hExitPopupBtn : hButton
{
    [SerializeField]
    private hLevelSelectPopup levelSelectPopup;

    public override void Click()
    {
        levelSelectPopup.Close();
    }
}
