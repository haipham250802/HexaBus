using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
public class PickerWheel : MonoBehaviour
{
    [SerializeField] Transform _wheelSpin;
    [SerializeField] WheelElement[] _wheelElement;
    [SerializeField] AnimationCurve _curveSpin;
    public void Spin(System.Action OnStartSpin = null, System.Action OnEndSpin = null)
    {
        OnStartSpin?.Invoke();
        int rand = Random.Range(0, _wheelElement.Length);
        int indexWheelElement = _wheelElement[rand].ID;
        _wheelSpin.transform.DOLocalRotate(new Vector3(0, 0, -360 * 5 + (indexWheelElement * 45 + Random.Range(-18, 18))), 5f, RotateMode.FastBeyond360)
            .SetEase(_curveSpin)
            .OnComplete(() =>
            {
                List<RewardInfo> listRWInfo = new();
                RewardInfo rw = new RewardInfo();
                rw.TypeReward = _wheelElement[rand].TypeReward;
                rw.QuantityReward = 1;
                listRWInfo.Add(rw);
                UIPopUp.Ins.ShowPopUpReward(listRWInfo);
                OnEndSpin?.Invoke();
            });
    }
}
