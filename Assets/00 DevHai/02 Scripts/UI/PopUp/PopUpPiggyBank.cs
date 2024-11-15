using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpPiggyBank : PopUpBase
{
    [SerializeField] Button _exitButton;
    [SerializeField] Button _purchaseButton;
    [SerializeField] Image _fillProgress;
    [SerializeField] Text _progressTxt;

    
    private void Start()
    {
        InitButton();
        InitView();
    }
    private void InitButton()
    {
        _exitButton.onClick.AddListener(ExitClicked);
    }
    private void ExitClicked()
    {
        Hide();
    }
    private void InitView()
    {
        int value = GameController.Ins.GameData.FeatureModel.GetQuantityPiggyBank();
        
    }
}
