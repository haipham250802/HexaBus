using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class GameData : MonoBehaviour
{
    private CurrencyModel _currencyModel;
    private BoosterModel _boosterModel;
    private FeatureModel _featureModel;
    public CurrencyModel CurrencyModel
    {
        get
        {
            if (_currencyModel == null)
                _currencyModel = new();
            return _currencyModel;
        }
    }
    public BoosterModel BoosterModel
    {
        get
        {
            if (_boosterModel == null)
                _boosterModel = new();
            return _boosterModel;
        }
    }
    public FeatureModel FeatureModel
    {
        get
        {
            if (_featureModel == null)
                _featureModel = new();
            return _featureModel;
        }
    }
}
public class CurrencyModel
{
    public ReactiveProperty<int> Coin { get; set; }
    public ReactiveProperty<int> Diamond { get; set; }
    public CurrencyModel()
    {
        Coin = new ReactiveProperty<int>(GetCoin());
        Coin.Subscribe(_ => SetCoin(_));

        Diamond = new ReactiveProperty<int>(GetDiamond());
        Diamond.Subscribe(_ => SetDiamond(_));
    }
    private int GetCoin()
    {
        return PlayerPrefs.GetInt("coin", 0);
    }
    private void SetCoin(int value)
    {
        PlayerPrefs.SetInt("coin", value);
    }
    private int GetDiamond()
    {
        return PlayerPrefs.GetInt("diamond", 0);
    }
    private void SetDiamond(int value)
    {
        PlayerPrefs.SetInt("diamond", value);
    }
}
public class BoosterModel
{
    public ReactiveProperty<int> Hammer { get; set; }
    public ReactiveProperty<int> Bomb { get; set; }
    public BoosterModel()
    {
        Hammer = new ReactiveProperty<int>(GetHammer());
        Hammer.Subscribe(_ => SetHammer(_));

        Bomb = new ReactiveProperty<int>(GetBomb());
        Bomb.Subscribe(_ => SetBomb(_));
    }
    private int GetHammer()
    {
        return PlayerPrefs.GetInt("Hammer", 0);
    }
    private void SetHammer(int value)
    {
        PlayerPrefs.SetInt("Hammer", value);
    }
    private int GetBomb()
    {
        return PlayerPrefs.GetInt("Bomb", 0);
    }
    private void SetBomb(int value)
    {
        PlayerPrefs.SetInt("Bomb", value);
    }
}
public class FeatureModel
{
    public ReactiveProperty<int> QuantityCoinPiggyBank { get; set; }
    public FeatureModel()
    {
        QuantityCoinPiggyBank = new ReactiveProperty<int>(GetQuantityPiggyBank());
        QuantityCoinPiggyBank.Subscribe(_ => SetQuantityPiggyBank(_));
    }
    public int GetQuantityPiggyBank()
    {
        return PlayerPrefs.GetInt("Quantity_Coin_PiggyBank", 0);
    }
    public void SetQuantityPiggyBank(int value)
    {
        PlayerPrefs.SetInt("Quantity_Coin_PiggyBank", value);
    }
}
