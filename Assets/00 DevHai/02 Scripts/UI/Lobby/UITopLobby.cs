using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITopLobby : MonoBehaviour
{
    [SerializeField] Button _settingButton;
    [SerializeField] Button _piggyBankButton;
    [SerializeField] Button _noAds;
    [SerializeField] Button _luckyWheelButton;
    [SerializeField] Button _dailyLogin7dButton;
    private void Start()
    {
        InitButton();
    }
    private void InitButton()
    {
        _settingButton.onClick.AddListener(SettingClicked);
        _piggyBankButton.onClick.AddListener(PiggyBankClicked);
        _noAds.onClick.AddListener(NoadsClicked);
        _luckyWheelButton.onClick.AddListener(LuckyWheelClicked);
        _dailyLogin7dButton.onClick.AddListener(DailyLogin7dClicked);
    }
    private void NoadsClicked()
    {
       UIPopUp.Ins.ShowPopUpNoAds();
    }
    private void SettingClicked()
    {
        UIPopUp.Ins.ShowPopUpSetting();
    }
    private void PiggyBankClicked()
    {
        UIPopUp.Ins.ShowPopUpPiggyBank();
    }
    private void LuckyWheelClicked()
    {
        UIPopUp.Ins.ShowPopUpLuckyWheel();
    }
    private void DailyLogin7dClicked()
    {
        UIPopUp.Ins.ShowPopUpDailyLogin();
    }
}
