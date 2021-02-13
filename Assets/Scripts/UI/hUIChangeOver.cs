using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hUIChangeOver : MonoBehaviour
{
    private RectTransform _rect;
    [SerializeField]
    private Vector2 _showPivot, _hidePivot;
    [SerializeField]
    private float _speed;
    private IEnumerator _coroutine;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _coroutine = null;
    }

    public void Show(DelVoid endDel = null)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        /*_rect.pivot = _hidePivot;
        _rect.anchoredPosition = Vector2.zero;*/

        _coroutine = GoZeroPos(_showPivot, endDel);
        StartCoroutine(_coroutine);
    }

    public void Hide(DelVoid endDel = null)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

       /* _rect.pivot = _showPivot;
        _rect.anchoredPosition = Vector2.zero;*/

        _coroutine = GoZeroPos(_hidePivot, endDel);
        StartCoroutine(_coroutine);
    }

    private IEnumerator GoZeroPos(Vector2 pivot, DelVoid endDel)
    {
        Vector2 size = _rect.rect.size;
        Vector2 deltaPivot = _rect.pivot - pivot;
        Vector2 deltaPosition = new Vector2(deltaPivot.x * size.x, deltaPivot.y * size.y);
        _rect.pivot = pivot;
        _rect.anchoredPosition -= deltaPosition;

        while (_rect.anchoredPosition.magnitude > 0)
        {
            _rect.anchoredPosition = Vector2.MoveTowards(_rect.anchoredPosition, Vector2.zero, _speed * Time.deltaTime);
            yield return null;
        }
        _rect.anchoredPosition = Vector2.zero;
        _coroutine = null;
        endDel?.Invoke();
    }
}
