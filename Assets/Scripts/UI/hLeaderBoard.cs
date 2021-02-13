using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hLeaderBoard : MonoBehaviour
{
    [SerializeField]
    private hRecordLine m_recordLinePrefab;
    [SerializeField]
    private Transform m_scrollViewContent;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Close();
    }

    public void Open(hLevel inLevel)
    {
        gameObject.SetActive(true);
        StartCoroutine(DisplayRecords(inLevel));
    }

    private IEnumerator DisplayRecords(hLevel inLevel)
    {
        yield return null;
        /*List<hUserRecord> userRecords = new List<hUserRecord>();
        yield return hDatabase.current.GetLevelRecords(inLevel.difficulty, inLevel.levelNum, userRecords);

        for (int i = 0; i < userRecords.Count; ++i)
        {
            hRecordLine recordLine = Instantiate(m_recordLinePrefab, m_scrollViewContent);
            recordLine.rank.text = (i + 1).ToString();
            recordLine.userName.text = userRecords[i].userName;
            recordLine.record.text = userRecords[i].record.ToString("F2");

            Color color = recordLine.background.color;
            if (i % 2 == 1)
                color.a = .25f;
            else
                color.a = .75f;
            recordLine.background.color = color;
        }*/
    }

    public void Close()
    {
        for (int i = 0; i < m_scrollViewContent.transform.childCount; ++i)
        {
            var child = m_scrollViewContent.transform.GetChild(0).gameObject;
            DestroyImmediate(child);
        }
        gameObject.SetActive(false);
    }
}
