using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hMenuContainer : MonoBehaviour
{
    [SerializeField]
    private ScrollRect _scrollRect;
    [SerializeField]
    private LayoutElement[] _elements;
    [SerializeField]
    private Canvas _canvas;
    private IEnumerator _coroutine;
    [SerializeField]
    private float _speed;

    private static event DelInt _menuChangeStartEvent, _menuChangeEndEvent;

    public static event DelInt menuChangeStartEvent { add => _menuChangeStartEvent += value; remove => _menuChangeStartEvent -= value; }
    public static event DelInt menuChangeEndEvent { add => _menuChangeEndEvent += value; remove => _menuChangeEndEvent -= value; }

    private void Start()
    {
        for (int i = 0; i < _elements.Length; ++i)
        {
            _elements[i].preferredWidth = _canvas.GetComponent<RectTransform>().sizeDelta.x;
            _elements[i].preferredHeight = _canvas.GetComponent<RectTransform>().sizeDelta.y;
        }
    }

    public void GoTo(int index, bool isInstant = false)
    {
        var offset = _scrollRect.content.childCount;
        if (offset == 0)
            offset = 1;
        float x = index / (float)(offset - 1);

        if (isInstant)
        {
            _scrollRect.normalizedPosition = new Vector2(x, _scrollRect.normalizedPosition.y);
            _menuChangeEndEvent?.Invoke(index);
        }
        else
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = LerpMove(new Vector2(x, _scrollRect.normalizedPosition.y), () => _menuChangeEndEvent?.Invoke(index));
            StartCoroutine(_coroutine);
            _menuChangeStartEvent?.Invoke(index);
        }
    }

    private IEnumerator LerpMove(Vector2 pos, DelVoid del)
    {
        while (Vector2.Distance(_scrollRect.normalizedPosition, pos) > 0.1f)
        {
            _scrollRect.normalizedPosition = Vector2.Lerp(_scrollRect.normalizedPosition, pos, _speed * hTime.deltaTime);
            yield return null;
        }
        _scrollRect.normalizedPosition = pos;
        _coroutine = null;

        del?.Invoke();
    }
}
