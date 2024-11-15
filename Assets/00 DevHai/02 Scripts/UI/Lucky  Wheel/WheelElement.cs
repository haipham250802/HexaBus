using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WheelElement : MonoBehaviour
{
    [SerializeField] TypeReward _typeReward;
    [SerializeField] int _id;
    public int ID => _id;
    public TypeReward TypeReward => _typeReward;
    public void GiveReward()
    {
        switch (_typeReward)
        {
            case TypeReward.NONE:
                break;
            case TypeReward.COIN:
                int newCoin = GameController.Ins.GameData.CurrencyModel.Coin.Value + 1;
                GameController.Ins.GameData.CurrencyModel.Coin.Value = newCoin;
                break;
            case TypeReward.DIAMOND:
                int newDiamond = GameController.Ins.GameData.CurrencyModel.Diamond.Value + 1;
                GameController.Ins.GameData.CurrencyModel.Diamond.Value = newDiamond;
                break;
            case TypeReward.HAMMER:
                int newHammer = GameController.Ins.GameData.BoosterModel.Hammer.Value + 1;
                GameController.Ins.GameData.BoosterModel.Hammer.Value = newHammer;
                break;
            case TypeReward.BOMB:
                int newBomb = GameController.Ins.GameData.BoosterModel.Bomb.Value + 1;
                GameController.Ins.GameData.BoosterModel.Bomb.Value = newBomb;
                break;
            default:
                break;
        }
    }
}
