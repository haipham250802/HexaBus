using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;
public enum E_STATE_SHOW_POPUP
{
    NONE = -1,
    SCALE,
    FADE,
    BUBBLE
}
public class PopUpBase : MonoBehaviour
{
    [FoldoutGroup("Base")] [SerializeField] E_STATE_SHOW_POPUP _stateShowPopUp;
    [FoldoutGroup("Base")] [SerializeField] Transform _content;
    [FoldoutGroup("Base")] [SerializeField] Transform _bgShadow;
    [FoldoutGroup("Base")] [SerializeField] List<GameObject> _listAnimBubble = new();
    [FoldoutGroup("Base")] [SerializeField] AnimationCurve _animationCurveBubble;
    [FoldoutGroup("Base")] [SerializeField] AnimationCurve _animationCurveContent;
    [FoldoutGroup("Base")] [SerializeField] AnimationCurve _animationCurveHide;
    Tweener tween;
    Tweener tween2;
    CanvasGroup can2;
    private void OnEnable()
    {
        if(_bgShadow)
        {
            if (can2 == null)
                can2 = _bgShadow.gameObject.AddComponent<CanvasGroup>();
            can2.alpha = 0;
            FadeOfCanvasGroup(can2, 0, 1, 1, _animationCurveContent);
        }
     
        if (_content)
            _content.transform.localScale = Vector3.one * 0.55f;
        Show();
    }
    private void Show()
    {
        if (_content)
            tween2 = _content.DOScale(Vector3.one, .45f).SetEase(_animationCurveContent);
        OnStateShowBubble();
    }
    private void OnStateShowBubble()
    {
        foreach (var item in _listAnimBubble)
        {
            UIBubbleElement uiBubbleElement = item.AddComponent<UIBubbleElement>();
            uiBubbleElement.AnimationCurve = _animationCurveBubble;
            uiBubbleElement.Show();
        }
    }
    protected void Hide()
    {
        foreach (var item in _listAnimBubble)
        {
            UIBubbleElement uiBubbleElement = item.GetComponent<UIBubbleElement>();
            if (uiBubbleElement == null)
                uiBubbleElement = item.AddComponent<UIBubbleElement>();
            uiBubbleElement.AnimationCurveHide = _animationCurveHide;
            uiBubbleElement.Hide();
        }
        Destroy(gameObject, .55f);
        tween2?.Kill();
        if (_content)
            tween2 = _content.transform.DOScale(Vector3.one * 0.5f, 0.8f).SetEase(_animationCurveHide);
    }
    private void OnDisable()
    {
        tween?.Kill();
        tween2?.Kill();
    }
    private Tweener FadeOfCanvasGroup(CanvasGroup can, float startValue, float endValue, float duration, AnimationCurve curve, System.Action callback = null)
    {
        return DOTween.To(() => startValue, _ =>
         {
             can.alpha = _;
         }, endValue, duration).SetEase(curve).OnComplete(() =>
         {
             callback?.Invoke();
         });
    }
}
