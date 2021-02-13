using UnityEngine;


public class hPlayFooter : MonoBehaviour
{
    public enum State
    {
        WIN,
        PAUSE,
        LOSE,
    }

    [SerializeField]
    private hUIChangeOver _changeOver;
    [SerializeField]
    private GameObject _nextBtn, _continueBtn;

    private void Awake()
    {
        hGameManager.current.pauseEvent += ()=> { Show(State.PAUSE); };
        hGameManager.current.resumeEvent += Hide;
        hGameManager.current.loseEvent += () => { Show(State.LOSE); };
        hGameManager.current.winEvent += () => { Show(State.WIN); };

        gameObject.SetActive(false);
    }

    public void Show(State state)
    {
        gameObject.SetActive(true);
        _nextBtn.SetActive(false);
        _continueBtn.SetActive(false);
        switch (state)
        {
            case State.WIN:
                _nextBtn.SetActive(true);
                break;
            case State.PAUSE:
                _continueBtn.SetActive(true);
                break;
        }
        _changeOver.Show();
    }

    public void Hide()
    {
        _changeOver.Hide(() => { gameObject.SetActive(false); });
    }
}
