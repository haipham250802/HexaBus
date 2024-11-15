using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpDailyLogin : PopUpBase
{
    [SerializeField] Button _exitButton;
    [SerializeField] List<DailyRWElement> _rewardElements;
    [SerializeField] Transform _parent;
    private void Start()
    {
        InitButton();
        LoadDailyReward();
    }
    private void LoadDailyReward()
    {
        for (int i = 0; i < _rewardElements.Count; i++)
        {
            _rewardElements[i].InitReward(
                      GameController.Ins.DailyLoginController.GetIconReward(GameController.Ins.DailyLoginController.ListDailyLoginInfo[i].TypeReward),
                      GameController.Ins.DailyLoginController.ListDailyLoginInfo[i].QuantityReward,
                      GameController.Ins.DailyLoginController.ListDailyLoginInfo[i].Day,
                      GameController.Ins.DailyLoginController.ListDailyLoginInfo[i].TypeReward);
        }
    }
    private void InitButton()
    {
        _exitButton.onClick.AddListener(ExitClicked);
    }
    private void ExitClicked()
    {
        Hide();
    }
}
