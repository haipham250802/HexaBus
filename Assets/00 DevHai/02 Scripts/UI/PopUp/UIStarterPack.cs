using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStarterPack : MonoBehaviour
{
    [SerializeField] Text _quantityCoin;
    [SerializeField] Text _quantityBomb;
    [SerializeField] Text _quantityHammer;
    [SerializeField] Button _purchaseButton;
    [SerializeField] DataStarterPack _dataStarterPack;
    [SerializeField] Image _iconCoin;
    [SerializeField] Image _iconBoom;
    [SerializeField] Image _iconHammer;

    public void OnStart()
    {
        InitData();
        InitButton();
    }
    private void InitData()
    {
        _quantityCoin.text = $"x{_dataStarterPack.QuantityCoin}";
        _quantityBomb.text = $"x{_dataStarterPack.QuantityBomb}";
        _quantityHammer.text = $"x{_dataStarterPack.QuantityHammer}";
    }
    private void InitButton()
    {
        _purchaseButton.onClick.AddListener(PurchaseClicked);
    }
    private void PurchaseClicked()
    { 
        List<RewardInfo> listRewardInfo = new();
        RewardInfo rewardInfo = new();
        rewardInfo.TypeReward = TypeReward.COIN;
        rewardInfo.Icon = _iconCoin.sprite;
        rewardInfo.QuantityReward = _dataStarterPack.QuantityCoin;

        RewardInfo rewardInfo1 = new();
        rewardInfo1.TypeReward = TypeReward.BOMB;
        rewardInfo1.Icon = _iconBoom.sprite;
        rewardInfo1.QuantityReward = _dataStarterPack.QuantityBomb;

        RewardInfo rewardInfo2 = new();
        rewardInfo2.TypeReward = TypeReward.HAMMER;
        rewardInfo2.Icon = _iconHammer.sprite;
        rewardInfo2.QuantityReward = _dataStarterPack.QuantityHammer;

        listRewardInfo.Add(rewardInfo);
        listRewardInfo.Add(rewardInfo1);
        listRewardInfo.Add(rewardInfo2);

        UIPopUp.Ins.ShowPopUpReward(listRewardInfo);
    }
}
