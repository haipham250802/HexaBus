using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class PopUpLuckyWheel : PopUpBase
{
    [SerializeField] Button _spinButton;
    [SerializeField] Button _exitButton;
    [SerializeField] PickerWheel _pickerWheel;
    [SerializeField] Image _imgButton;
    [SerializeField] Image _fill;
    [SerializeField] Text _progressText;
    float value = 0;
    private void Start()
    {
        InitButton();
        CheckCanSpin();
    }
    private void InitButton()
    {
        _spinButton.onClick.AddListener(OnClickButtonSpin);
        _exitButton.onClick.AddListener(ExitClicked);
    }
    private void CheckCanSpin()
    {
        InitViewProgress();
        if (value < 1)
        {
            _imgButton.color = Color.gray;
            _spinButton.interactable = false;
        }
    }
    private void InitViewProgress()
    {
        if (GameController.Ins.LuckyWheelController.ProgressSpinWheel > 10)
            GameController.Ins.LuckyWheelController.ProgressSpinWheel = 10;
        value = (float)(GameController.Ins.LuckyWheelController.ProgressSpinWheel / 10f);
        _fill.DOFillAmount(value, 0.3f);
        _progressText.text = $"{GameController.Ins.LuckyWheelController.ProgressSpinWheel}/{10}";
    }
    private void OnClickButtonSpin()
    {
        _pickerWheel.Spin(OnStartSpin, OnEndSpin);
    }
    private void OnStartSpin()
    {
        _imgButton.color = Color.gray;
        _spinButton.interactable = false;
        GameController.Ins.LuckyWheelController.ProgressSpinWheel = 0;
        InitViewProgress();
    }
    private void OnEndSpin()
    {
        _imgButton.color = Color.white;
        _spinButton.interactable = true;
    }
    private void ExitClicked()
    {
        Hide();
    }
}
