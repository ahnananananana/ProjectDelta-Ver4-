using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hTotalComplete : MonoBehaviour
{
    private TMPro.TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMPro.TMP_Text>();

        int totalStageNum = hSharedData.GetLevels(Difficulty.NORMAL).Count +
            hSharedData.GetLevels(Difficulty.HARD).Count +
            hSharedData.GetLevels(Difficulty.EXTREME).Count;

        int clearStageNum = PlayerPrefs.GetInt("NormalLevel", 0) +
            PlayerPrefs.GetInt("HardLevel", 0) +
            PlayerPrefs.GetInt("ExtremeLevel", 0);

        _text.text = "COMPLETE " + Mathf.Clamp((clearStageNum * 100 / totalStageNum), 0, 100) + "%";
    }
}
