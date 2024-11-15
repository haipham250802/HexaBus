using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/DataDailyLogin", fileName = "DataDailyLogin")]
public class DataDailyLoginReward : ScriptableObject
{
    [SerializeField] List<DataDailyModel> _listDataDaily = new();

    public DataDailyModel GetDataOfDay(int day)
    {
        foreach (var item in _listDataDaily)
        {
            if (item.Day == day)
                return item;
        }
        return null;
    }
}
[System.Serializable]
public class DataDailyModel
{
    public int Day;
    public TypeReward TypeReward;
    public int QuantityReward;
}
