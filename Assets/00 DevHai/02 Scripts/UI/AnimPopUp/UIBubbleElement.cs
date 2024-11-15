using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIBubbleElement : MonoBehaviour
{
    public AnimationCurve AnimationCurve { get; set; }
    public AnimationCurve AnimationCurveHide { get; set; }
    Tweener _tweenShow;
    Tweener _tweenHide;

    private void Awake()
    {
        transform.localScale = Vector3.one * 0.15f;
    }
    public void Show()
    {
        float duration = Random.Range(0.55f, .85f);
        _tweenShow = transform.DOScale(Vector3.one, duration)
            .SetEase(AnimationCurve);
    }
    public void Hide()
    {
        float duration = Random.Range(0.55f, .85f);
        _tweenHide = transform.DOScale(Vector3.one * 0.5f, duration)
            .SetEase(AnimationCurveHide);
    }
    private void OnDisable()
    {
        DOTween.Kill(_tweenShow);
        DOTween.Kill(_tweenHide);
    }
}
