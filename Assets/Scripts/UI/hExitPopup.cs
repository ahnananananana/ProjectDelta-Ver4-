using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class hExitPopup : MonoBehaviour
{
    [SerializeField]
    private Image m_background;
    private IEnumerator m_curCoroutine;
    [SerializeField]
    private GameObject m_dialog;

    private void Awake()
    {
        var color = m_background.color;
        color.a = 0f;
        m_background.color = color;
        gameObject.SetActive(false);
    }

    public void Open()
    {
        if(!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            m_dialog.SetActive(true);
        }
        if (m_curCoroutine != null)
            StopCoroutine(m_curCoroutine);
        m_curCoroutine = FadeAlpha(.4f);
        StartCoroutine(m_curCoroutine);
    }

    public void Close()
    {
        m_dialog.SetActive(false);
        if (m_curCoroutine != null)
            StopCoroutine(m_curCoroutine);
        m_curCoroutine = FadeAlpha(0f, () => gameObject.SetActive(false)) ;
        StartCoroutine(m_curCoroutine);
    }

    private IEnumerator FadeAlpha(float targetAlpha, DelVoid del = null)
    {
        var color = m_background.color;
        while(Mathf.Abs(color.a - targetAlpha) > .1f)
        {
            color.a = Mathf.MoveTowards(color.a, targetAlpha, hTime.deltaTime * 2);
            m_background.color = color;
            yield return null;
        }
        color.a = targetAlpha;
        m_background.color = color;
        m_curCoroutine = null;
        del?.Invoke();
    }
}
