using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpShop : PopUpBase
{
    [SerializeField] UIStarterPack _uiStarterPack;
    [SerializeField] List<CurrencyShopElement> _listCurrencyShopElent = new();
    [SerializeField] Button _exit;

    private void Start()
    {
        _uiStarterPack.OnStart();
        _exit.onClick.AddListener(OnClickButtonExit);
        foreach (CurrencyShopElement item in _listCurrencyShopElent)
        {
            item.OnStart();
        }
    }
    private void OnClickButtonExit()
    {
        Hide();
    }
}
