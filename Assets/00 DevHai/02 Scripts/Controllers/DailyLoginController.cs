using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DailyLoginController : MonoBehaviour
{
    public static System.Action A_OnCheckNoti;
    [SerializeField] List<DailyLoginInfo> _listDailyLoginInfo = new();
    [SerializeField] DataDailyLoginReward _dataDaily;
    [SerializeField] Sprite[] _iconRewards;

    public List<DailyLoginInfo> ListDailyLoginInfo => _listDailyLoginInfo;

    private void Start()
    {
        LoadDataDailyLoginInfo();
    }
    private void LoadDataDailyLoginInfo()
    {
        for (int i = 1; i <= 7; i++)
        {
            DailyLoginInfo dailyInfo = new();
            DataDailyModel data = _dataDaily.GetDataOfDay(i);
            dailyInfo.Day = i;
            dailyInfo.QuantityReward = data.QuantityReward;
            dailyInfo.IsClaimed = GetClaimed(i);
            dailyInfo.TypeReward = data.TypeReward;
            _listDailyLoginInfo.Add(dailyInfo);
        }
    }
    public bool CheckCanClaimDaily()
    {
        System.DateTime lastDate = new System.DateTime(GameController.Ins.DailyLoginController.LastTimeClaim);
        System.DateTime currentDate = System.DateTime.Now;
        if (!GetClaimed(LastDayClaimed + 1) && lastDate.Date < currentDate.Date)
            return true;
        else
            return false;
    }
    public Sprite GetIconReward(TypeReward typeReward)
    {
        switch (typeReward)
        {
            case TypeReward.NONE:
                break;
            case TypeReward.COIN:
                return _iconRewards[0];
            case TypeReward.DIAMOND:
                return _iconRewards[1];
            case TypeReward.HAMMER:
                return _iconRewards[2];
            case TypeReward.BOMB:
                return _iconRewards[3];
            default:
                break;
        }
        return null;
    }
    #region GET/SET
    public void Claimed(int day)
    {
        PlayerPrefs.SetInt($"Day_{day}_Claimed", 1);
        _listDailyLoginInfo[day - 1].IsClaimed = true;
    }
    public bool GetClaimed(int day)
    {
        return PlayerPrefs.GetInt($"Day_{day}_Claimed", 0) == 1;
    }
    public long LastTimeClaim
    {
        get
        {
            string currentTime = $"{System.DateTime.Now.AddDays(-1).Ticks}";
            long valueTime = long.Parse(PlayerPrefs.GetString("LastTimeClaimedDaily", currentTime));
            return valueTime;
        }
        set
        {
            PlayerPrefs.SetString("LastTimeClaimedDaily", value.ToString());
        }
    }
    public int LastDayClaimed
    {
        get
        {
            return PlayerPrefs.GetInt("LastDayClaimedDaily", 0);
        }
        set
        {
            PlayerPrefs.SetInt("LastDayClaimedDaily", value);
        }
    }
    #endregion
}
[System.Serializable]
public class DailyLoginInfo
{
    public int Day;
    public TypeReward TypeReward;
    public int QuantityReward;
    public bool IsClaimed;
}
