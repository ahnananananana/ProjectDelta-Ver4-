using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class hRecordLine : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text m_rank, m_userName, m_record;
    [SerializeField]
    private Image m_background;

    public TMP_Text rank => m_rank;
    public TMP_Text userName => m_userName;
    public TMP_Text record => m_record;
    public Image background => m_background;
}
