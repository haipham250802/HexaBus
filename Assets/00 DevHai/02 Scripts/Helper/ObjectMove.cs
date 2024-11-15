using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ObjectMove : MonoBehaviour
{
    public void OnActive()
    {
        transform.localScale = Vector3.one * 0.3f;
        transform.DOScale(Vector3.one * 0.9f, 0.2f).SetEase(Ease.InOutBack);
    }
    public void MoveToTarget(Vector3 pos)
    {
        float timeMove = Random.Range(0.2f,0.4f);
        transform.DOMove(pos, timeMove).SetEase(Ease.InOutSine).OnComplete(() => { OnDeactive(); });
    }
    public void OnDeactive()
    {
        gameObject.SetActive(false);
    }
}
