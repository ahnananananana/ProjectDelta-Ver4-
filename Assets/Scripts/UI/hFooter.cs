using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hFooter : MonoBehaviour
{
    [SerializeField]
    private hUIChangeOver _changeOver;
    [SerializeField]
    private GameObject _difficultyMenu;

    private void Awake()
    {
        hMenuContainer.menuChangeStartEvent += MenuChangeStart;
        hMenuContainer.menuChangeEndEvent += MenuChangeEnd;

        _difficultyMenu.SetActive(true);
    }

    private void OnDestroy()
    {
        hMenuContainer.menuChangeStartEvent -= MenuChangeStart;
        hMenuContainer.menuChangeEndEvent -= MenuChangeEnd;
    }

    private void MenuChangeStart(int index)
    {
        switch(index)
        {
            case 2:
                {
                    _changeOver.Hide();
                    break;
                }
        }
    }

    private void MenuChangeEnd(int index)
    {
        switch(index)
        {
            case 1:
                {
                    _difficultyMenu.SetActive(true);
                    _changeOver.Show();
                    break;
                }
        }
    }
}
