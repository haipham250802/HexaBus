using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UICurrency : MonoBehaviour
{
    [SerializeField] bool _isCurrenPopUpShop = false;
    [SerializeField] Button _addCoin;
    [SerializeField] Button _addDiamond;

    [SerializeField] Text _coinText;
    [SerializeField] Text _diamondText;

    private void Start()
    {
        Initialize();
        InitButton();
    }
    private void Initialize()
    {
        GameController.Ins.GameData.CurrencyModel.Coin.Subscribe(_ => _coinText.text = _.ToString()).AddTo(this);
        GameController.Ins.GameData.CurrencyModel.Diamond.Subscribe(_ => _diamondText.text = _.ToString()).AddTo(this);
    }
    private void InitButton()
    {
        if (_isCurrenPopUpShop)
        {
            _addCoin.interactable = false;
            _addDiamond.interactable = false;
        }
        _addCoin.onClick.AddListener(AddCoinClicked);
        _addDiamond.onClick.AddListener(AddDiamondClicked);
    }
    private void AddCoinClicked()
    {
        UIPopUp.Ins.ShowPopUpShop();
    }
    private void AddDiamondClicked()
    {
        UIPopUp.Ins.ShowPopUpShop();
    }
    private void OnDisable()
    {
       
    }
}
