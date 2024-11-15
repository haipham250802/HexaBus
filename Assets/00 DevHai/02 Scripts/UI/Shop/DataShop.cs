using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/DataShop",fileName = "DataShop")]
public class DataShop : ScriptableObject
{
    [SerializeField] ShopInfo[] shopInfos;
    public ShopInfo GetShopinfoOfType(TypeElementShop typeElementShop)
    {
        foreach (var item in shopInfos)
        {
            if (item.TypeElementShop == typeElementShop)
                return item;
        }
        return null;
    }
}
[System.Serializable]
public class ShopInfo
{
    public TypeElementShop TypeElementShop;
    public int Quantity;
}

