using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
public class AnimUIGamplayOn : MonoBehaviour
{
    [SerializeField] RectTransform _topContainer;
    [SerializeField] RectTransform _bottomContainer;
    [FoldoutGroup("Base")] [SerializeField] List<GameObject> _listAnimBubble = new();
    [FoldoutGroup("Base")] [SerializeField] AnimationCurve _animationCurveBubble;

    private void Awake()
    {
        _topContainer.anchoredPosition = new Vector3(0, 300, 0);
        _bottomContainer.anchoredPosition = new Vector3(0, -300, 0);
    }
    public void Show()
    {
        _topContainer.DOAnchorPosY(-110, 0.65f).SetEase(Ease.InOutSine);
        _bottomContainer.DOAnchorPosY(110, 0.5f).SetEase(Ease.InOutSine);
        StartCoroutine(IE_delayActiveAnim());
    }
    IEnumerator IE_delayActiveAnim()
    {
        yield return new WaitForSecondsRealtime(0.35f);
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
    [Button]
    public void Hide()
    {
        _topContainer.DOAnchorPosY(300, 0.65f).SetEase(Ease.InOutSine);
        _bottomContainer.DOAnchorPosY(-300, 0.5f).SetEase(Ease.InOutSine);
    }
}
