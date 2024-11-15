using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyShopElement : MonoBehaviour
{
    [SerializeField] protected TypeElementShop _typeElementShop;
    [SerializeField] protected TypeReward _typeReward;
    [SerializeField] protected Text _quantityRewardText;
    [SerializeField] protected Text _priceText;
    [SerializeField] protected Button _purchaseButton;
    [SerializeField] protected DataShop _dataShop;
    [SerializeField] protected Image _icon;
    protected int _quantityReward;
    protected List<RewardInfo> listRewardInfo = new();
    public void OnStart()
    {
        InitData();
        InitButton();
    }
    private void InitData()
    {
        ShopInfo shopInfo = _dataShop.GetShopinfoOfType(_typeElementShop);
        _quantityRewardText.text = $"x{shopInfo.Quantity}";
        _quantityReward = shopInfo.Quantity;
    }
    private void InitButton()
    {
        _purchaseButton.onClick.AddListener(PurchaseClicked);
    }
    protected virtual void PurchaseClicked()
    {
        GiveReward();
    }
    protected virtual void GiveReward()
    {
        RewardInfo rewardInfo = new();
        rewardInfo.TypeReward = _typeReward;
        rewardInfo.Icon = _icon.sprite;
        rewardInfo.QuantityReward = _quantityReward;
        listRewardInfo = new();
        listRewardInfo.Add(rewardInfo);

        UIPopUp.Ins.ShowPopUpReward(listRewardInfo);
    }
}
