using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hSetPopup : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_InputField inputField;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        inputField.text = hSharedData.userName;
    }
    public void Close()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }

    public void TextChange(string str)
    {
        hSharedData.userName = str;
    }
}
