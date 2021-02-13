using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hBlinkText : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private TMPro.TMP_Text text;
    private void Awake()
    {
        text = GetComponent<TMPro.TMP_Text>();
        StartCoroutine(Blink());
    }


    private IEnumerator Blink()
    {
        while(true)
        {
            if (text.alpha < .5f)
                while(text.alpha < .9f)
                {
                    text.alpha += Time.fixedDeltaTime * speed;
                    yield return new WaitForFixedUpdate();
                }
            else
                while (text.alpha > .1f)
                {
                    text.alpha -= Time.fixedDeltaTime * speed;
                    yield return new WaitForFixedUpdate();
                }
        }
    }

}
