using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hNextBtn : hButton
{
    public override void Click()
    {
        if (_isClicked) return;
        _isClicked = true;
        //go next level
        hLevel nextLevel = new hLevel();
        bool isEnd = hSharedData.GetNextLevel(ref nextLevel);
        if(!isEnd)
        {
            hSharedData.nextSceneNum = 1;
        }
        else
        {
            hSharedData.nextSceneNum = 2;
            hSharedData.curLevel = nextLevel;
        }

        /*var levelList = hSharedData.levelList;

        int index = System.Array.FindIndex(levelList, level => level == hSharedData.curLevel); ;
        if (++index == levelList.Length)
            hSharedData.nextSceneNum = 0;
        else
        {
            hSharedData.nextSceneNum = 2;
            hSharedData.curLevel = hSharedData.levelList[index];
        }*/

        SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);

        //hAdManager.TryShow();
    }
}
