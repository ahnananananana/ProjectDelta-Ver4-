using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hPlayHeader : MonoBehaviour
{
    [SerializeField]
    private hUIChangeOver _changeOver;

    private void Awake()
    {
        hGameManager.current.pauseEvent += () => { _changeOver.Hide(() => { gameObject.SetActive(false); }); };
        hGameManager.current.resumeEvent += () => { gameObject.SetActive(true); _changeOver.Show(); };
        hGameManager.current.loseEvent += () => { _changeOver.Hide(() => { gameObject.SetActive(false); }); };
        hGameManager.current.winEvent += () => { _changeOver.Hide(() => { gameObject.SetActive(false); }); };
    }
}
