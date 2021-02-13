using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hHeader : MonoBehaviour
{
    [SerializeField]
    private hUIChangeOver _changeOver;
    [SerializeField]
    private GameObject _difficultyMenu, _levelMenu;

    private void Awake()
    {
        hMenuContainer.menuChangeStartEvent += MenuChangeStart;
        hMenuContainer.menuChangeEndEvent += MenuChangeEnd;

        _difficultyMenu.SetActive(true);
        _levelMenu.SetActive(false);
    }

    private void OnDestroy()
    {
        hMenuContainer.menuChangeStartEvent -= MenuChangeStart;
        hMenuContainer.menuChangeEndEvent -= MenuChangeEnd;
    }

    private void MenuChangeStart(int index)
    {
        switch (index)
        {
            case 0:
            case 1:
            case 2:
                {
                    _changeOver.Hide();
                    break;
                }
        }
    }

    private void MenuChangeEnd(int index)
    {
        switch (index)
        {
            case 1:
                {
                    _difficultyMenu.SetActive(true);
                    _levelMenu.SetActive(false);
                    _changeOver.Show();
                    break;
                }
            case 2:
                {
                    _difficultyMenu.SetActive(false);
                    _levelMenu.SetActive(true);
                    _changeOver.Show();
                    break;
                }
        }
    }
}
