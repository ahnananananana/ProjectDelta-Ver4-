using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hBackBtn : hButton
{
    [SerializeField]
    private hMenuController menuController;

    public override void Click()
    {
        menuController.GoHomeMenu();
    }
}
