using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpReward : PopUpBase
{
    [SerializeField] Button _claimButton;
    [SerializeField] Sprite[] _sprIcon;
    [SerializeField] Transform _parent;
    [SerializeField] RewardElement _rewardElement;
    private void Start()
    {
        _claimButton.onClick.AddListener(ClaimClicked);
    }
    public void ShowReward(List<RewardInfo> listRWInfo)
    {
        for (int i = 0; i < listRWInfo.Count; i++)
        {
            RewardElement reward = Instantiate(_rewardElement);
            reward.transform.SetParent(_parent, false);
            reward.transform.localPosition = Vector3.zero;
            reward.transform.localScale = Vector3.one;
            Sprite icon = listRWInfo[i].Icon ? listRWInfo[i].Icon : GetIconRWOfType(listRWInfo[i].TypeReward);
        //    reward.InitReward(icon, listRWInfo[i].QuantityReward);
            listRWInfo[i].SetRW();
        }
    }
    private Sprite GetIconRWOfType(TypeReward typeReward)
    {
        switch (typeReward)
        {
            case TypeReward.NONE:
                break;
            case TypeReward.COIN:
                return _sprIcon[0];
            case TypeReward.DIAMOND:
                return _sprIcon[1];
            case TypeReward.HAMMER:
                return _sprIcon[2];
            case TypeReward.BOMB:
                return _sprIcon[3];
        }
        return null;
    }
    private void ClaimClicked()
    {
        Hide();
    }
}
public class RewardInfo
{
    public TypeReward TypeReward;
    public int QuantityReward;
    public Sprite Icon;
    public void SetRW()
    {
        switch (TypeReward)
        {
            case TypeReward.NONE:
                break;
            case TypeReward.COIN:
                GameController.Ins.GameData.CurrencyModel.Coin.Value += QuantityReward;
                break;
            case TypeReward.DIAMOND:
                GameController.Ins.GameData.CurrencyModel.Diamond.Value += QuantityReward;
                break;
            case TypeReward.HAMMER:
                GameController.Ins.GameData.BoosterModel.Hammer.Value += QuantityReward;
                break;
            case TypeReward.BOMB:
                GameController.Ins.GameData.BoosterModel.Hammer.Value += QuantityReward;
                break;
            default:
                break;
        }
    }
}
