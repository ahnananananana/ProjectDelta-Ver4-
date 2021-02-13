using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hColorManager : MonoBehaviour
{
    private static hColorManager instance;
    private static string prefabPath = "GameObjects/ColorManager";
    public static hColorManager current
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<hColorManager>();
            if(instance == null)
            {
                var gob = Instantiate(Resources.Load(prefabPath)) as GameObject;
                instance = gob.GetComponent<hColorManager>();
            }
            return instance;
        }
    }
    [SerializeField]
    private Color[] m_themeColors;
    private Color m_curColor;
    [SerializeField]
    private Material m_backgroundMat;
    [SerializeField]
    private List<Material> m_groundMats;
    [SerializeField]
    private float m_transitionTime;
    private IEnumerator m_curCoroutine;

    public Color curColor  => m_curColor;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            m_curColor = m_backgroundMat.color;
            m_groundMats.AddRange(hDatabase.current.groundMats);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }


    public void SetTheme(Difficulty difficulty, bool isInstant = false)
    {
        m_curColor = m_themeColors[(int)difficulty];
        if (isInstant)
        {
            m_backgroundMat.color = m_curColor;
            for (int i = 0; i < m_groundMats.Count; ++i)
                m_groundMats[i].color = m_curColor;
        }
        else
        {
            if (m_curCoroutine != null)
                StopCoroutine(m_curCoroutine);
            m_curCoroutine = TransBackgroundTheme(m_curColor);
            StartCoroutine(m_curCoroutine);
        }
    } 

    public void SetTheme(Color color, bool isInstant = false)
    {
        m_curColor = color;
        if (isInstant)
        {
            m_backgroundMat.color = m_curColor;
            for (int i = 0; i < m_groundMats.Count; ++i)
                m_groundMats[i].color = m_curColor;
        }
        else
        {
            if (m_curCoroutine != null)
                StopCoroutine(m_curCoroutine);
            m_curCoroutine = TransBackgroundTheme(m_curColor);
            StartCoroutine(m_curCoroutine);
        }
    } 

    private IEnumerator TransBackgroundTheme(Color targetColor)
    {
        Vector4 targetVec4 = targetColor * 255f;
        Vector4 matVec4 = m_backgroundMat.color * 255f;
        var speed = Vector4.Distance(targetVec4, matVec4) / m_transitionTime;
        while (Vector4.Distance(targetVec4, matVec4) > .1f)
        {
            matVec4 = Vector4.MoveTowards(matVec4, targetVec4, hTime.deltaTime * speed);
            m_backgroundMat.color = matVec4 / 255f;
            for (int i = 0; i < m_groundMats.Count; ++i)
                m_groundMats[i].color = matVec4 / 255f;
            yield return null;
        }
        m_backgroundMat.color = targetColor;
        for (int i = 0; i < m_groundMats.Count; ++i)
            m_groundMats[i].color = targetColor;
        m_curCoroutine = null;
    }
}
