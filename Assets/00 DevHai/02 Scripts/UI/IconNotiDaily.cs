using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconNotiDaily : MonoBehaviour
{
    [SerializeField] GameObject _gNoti;
    private void Start()
    {
        DailyLoginController.A_OnCheckNoti += OnValueChangeNoti;
        OnValueChangeNoti();
    }
    private void OnValueChangeNoti()
    {
        _gNoti.SetActive(GameController.Ins.DailyLoginController.CheckCanClaimDaily());
    }
    private void OnDisable()
    {
        DailyLoginController.A_OnCheckNoti -= OnValueChangeNoti;
    }
}
