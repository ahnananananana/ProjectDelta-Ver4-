using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class hButton : MonoBehaviour
{
    protected Button _button;
    protected bool _isClicked;

    private void OnEnable()
    {
        _isClicked = false;
    }

    protected virtual void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Click);
    }

    public abstract void Click();
}
