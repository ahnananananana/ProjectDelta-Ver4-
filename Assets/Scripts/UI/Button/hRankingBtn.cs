using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hRankingBtn : hButton
{
    [SerializeField]
    private hLevelSelectPopup levelSelectPopup;

    public override void Click()
    {
        levelSelectPopup.OpenLeaderBoard();
    }
}
