using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void DelInt(int val);

public enum Menu
{
    TITLE,
    DIFFICULTYMENU,
    LEVELMENU,
}

public class hMenuController : MonoBehaviour
{
    private static Menu _curMenu = Menu.TITLE;
    private static Difficulty _lastDiff;

    [SerializeField]
    private hMenuContainer _menuContainer;
    [SerializeField]
    private hLevelContainer _levelContainer;
    [SerializeField]
    private hExitPopup _exitPopup;
    [SerializeField]
    private hSetPopup _setPopup;
    [SerializeField]
    private hLevelSelectPopup _levelSelectPopup;
    [SerializeField]
    private Material _groundMat;
    [SerializeField]
    private Vector2 _minmaxHeight;

    private void Start()
    {
        _groundMat.SetVector("_MinMaxHeight", _minmaxHeight);
        hTime.fixedDeltaTime = Time.fixedDeltaTime;
        hTime.deltaTime = Time.deltaTime;
        if (_curMenu != Menu.TITLE)
        {
            _menuContainer.GoTo((int)_curMenu, true);
            switch(_curMenu)
            {
                case Menu.LEVELMENU:
                    {
                        _levelContainer.SetLevels(_lastDiff);
                    }
                    break;
            }
        }
    }

    public void GoHomeMenu()
    {
        if (_curMenu == Menu.DIFFICULTYMENU) return;
        bool inst = _curMenu == Menu.TITLE ? true : false;
        _curMenu = Menu.DIFFICULTYMENU;
        _menuContainer.GoTo(1, inst);
    }

    public void GoLevelMenu(Difficulty difficulty)
    {
        if (_curMenu == Menu.LEVELMENU) return;
        _curMenu = Menu.LEVELMENU;
        _menuContainer.GoTo(2);
        _lastDiff = difficulty;
        _levelContainer.SetLevels(difficulty);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (_curMenu)
            {
                case Menu.DIFFICULTYMENU:
                    {
                        if (_exitPopup.gameObject.activeSelf)
                            _exitPopup.Close();
                        else
                            _exitPopup.Open();

                        break;
                    }
                case Menu.LEVELMENU:
                    {
                        if(!_levelSelectPopup.gameObject.activeSelf)
                            GoHomeMenu();
                        break;
                    }
            }
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
