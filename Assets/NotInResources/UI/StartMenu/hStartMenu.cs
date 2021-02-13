using UnityEngine;

public class hStartMenu : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;
    private bool _isClicked;

    public void StartGame()
    {
        if (_isClicked) return;
        _isClicked = true;
        _canvas.sortingOrder = -1;
    }

    public void ExitGame()
    {
        if (_isClicked) return;
        _isClicked = true;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
