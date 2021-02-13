using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hLoading : MonoBehaviour
{
    private static bool _isStart = true;
    private Animator _animator;

    public static bool isStart { get => _isStart; set => _isStart = value; }

    private event DelVoid _completeFadeIn, _completeFadeOut;

    public event DelVoid completeFadeOut { add { _completeFadeOut += value; } remove { _completeFadeOut += value; } }
    public event DelVoid completeFadeIn { add { _completeFadeIn += value; } remove { _completeFadeIn += value; } }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if(!_isStart)
            _animator.SetTrigger("Fade out");
    }

    public void FirstLoading()
    {
        _isStart = false;
        var op = SceneManager.LoadSceneAsync(hSharedData.nextSceneNum, LoadSceneMode.Additive);
        op.completed += _ => { _animator.SetTrigger("Fade in"); };
    }

    public void CompleteFadeOut()
    {
        _completeFadeOut?.Invoke();
        var op = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        op.completed += LoadNextScene;
    } 

    private void LoadNextScene(AsyncOperation inOp)
    {
        var op = SceneManager.LoadSceneAsync(hSharedData.nextSceneNum, LoadSceneMode.Additive);
        op.completed += _ => { _animator.SetTrigger("Fade in"); };
    }

    public void CompleteFadeIn()
    {
        _completeFadeIn?.Invoke();
        SceneManager.UnloadSceneAsync(gameObject.scene.buildIndex);
    }
}
