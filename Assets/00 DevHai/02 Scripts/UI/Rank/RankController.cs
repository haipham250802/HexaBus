using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System.Linq;
using Sirenix.OdinInspector;
public enum CountryCodeSupport
{
    NONE = -1,
    ZA,
    SA,
    ES,
    BY,
    BG,
    CN,
    CZ,
    DK,
    NL,
    US,
    EE,
    FO,
    FI,
    FR,
    DE,
    GR,
    IL,
    HU,
    IS,
    ID,
    IT,
    JP,
    KR,
    LV,
    LT,
    NO,
    PL,
    PT,
    RO,
    RU,
    RS,
    SK,
    SI,
    SE,
    TH,
    TR,
    UA,
    VN,
    TW
}
public class RankController : MonoBehaviour
{
    [field: SerializeField] public Color _colorWhite;
    [field: SerializeField] public Color _colorBlue;
    [field: SerializeField] public Color _colorYeallow;
    [field: SerializeField] public Color _colorOrange;
    [Header("Data")]
    [SerializeField] DataRanker _dataRanker;
    [SerializeField] List<RankInfo> _listRankInfo = new();
    [MinMaxSlider(5, 15, true)]
    [SerializeField] private Vector2 _rangeScoreBonusPerSecond = new(5, 15);
    public List<RankInfo> ListRankInfo => _listRankInfo;
    public string CountryIDThisUser { get; set; }
    [SerializeField] float timeCoolDown = 0;
    private void Start()
    {
        SetNameDefault();
        InitRank();
        LoadRank();
        if (GetCountTimeBonus() > 50)
            timeCoolDown = 120;
    }
    private void Update()
    {
        if (timeCoolDown < 5)
        {
            timeCoolDown += Time.deltaTime;
        }
        else
        {
            timeCoolDown = 0;
            AutoBonusScore();
        }
    }
    private void InitRank()
    {
        if (IsLoadRank) return;
        IsLoadRank = true;
        _dataRanker.Reload();
        CountTimeAutoBonus = System.DateTime.Now.Ticks;
    }
    private void LoadRank()
    {
        _listRankInfo = new();
        foreach (RankerModel item in _dataRanker.ListRankerModel)
        {
            RankInfo rankInfo = new();
            rankInfo.Name = item.name;
            rankInfo.Score = item.GetScore();
            _listRankInfo.Add(rankInfo);
        }
        SortRank();
    }
    private void SortRank()
    {
        _listRankInfo = _listRankInfo.OrderByDescending(num => num.Score).ToList();
        for (int i = 0; i < _listRankInfo.Count; i++)
        {
            _listRankInfo[i].IndexRank = i + 1;
        }
    }
    private void SetNameDefault()
    {
        switch (Application.systemLanguage)
        {
            case SystemLanguage.Afrikaans:
                CountryIDThisUser = "ZA"; // South Africa
                break;
            case SystemLanguage.Arabic:
                CountryIDThisUser = "SA"; // Saudi Arabia
                break;
            case SystemLanguage.Basque:
                CountryIDThisUser = "ES"; // Spain
                break;
            case SystemLanguage.Belarusian:
                CountryIDThisUser = "BY"; // Belarus
                break;
            case SystemLanguage.Bulgarian:
                CountryIDThisUser = "BG"; // Bulgaria
                break;
            case SystemLanguage.Catalan:
                CountryIDThisUser = "ES"; // Spain
                break;
            case SystemLanguage.Chinese:
                CountryIDThisUser = "CN"; // China
                break;
            case SystemLanguage.Czech:
                CountryIDThisUser = "CZ"; // Czech Republic
                break;
            case SystemLanguage.Danish:
                CountryIDThisUser = "DK"; // Denmark
                break;
            case SystemLanguage.Dutch:
                CountryIDThisUser = "NL"; // Netherlands
                break;
            case SystemLanguage.English:
                CountryIDThisUser = "US"; // United States
                break;
            case SystemLanguage.Estonian:
                CountryIDThisUser = "EE"; // Estonia
                break;
            case SystemLanguage.Faroese:
                CountryIDThisUser = "FO"; // Faroe Islands
                break;
            case SystemLanguage.Finnish:
                CountryIDThisUser = "FI"; // Finland
                break;
            case SystemLanguage.French:
                CountryIDThisUser = "FR"; // France
                break;
            case SystemLanguage.German:
                CountryIDThisUser = "DE"; // Germany
                break;
            case SystemLanguage.Greek:
                CountryIDThisUser = "GR"; // Greece
                break;
            case SystemLanguage.Hebrew:
                CountryIDThisUser = "IL"; // Israel
                break;
            case SystemLanguage.Hungarian:
                CountryIDThisUser = "HU"; // Hungary
                break;
            case SystemLanguage.Icelandic:
                CountryIDThisUser = "IS"; // Iceland
                break;
            case SystemLanguage.Indonesian:
                CountryIDThisUser = "ID"; // Indonesia
                break;
            case SystemLanguage.Italian:
                CountryIDThisUser = "IT"; // Italy
                break;
            case SystemLanguage.Japanese:
                CountryIDThisUser = "JP"; // Japan
                break;
            case SystemLanguage.Korean:
                CountryIDThisUser = "KR"; // South Korea
                break;
            case SystemLanguage.Latvian:
                CountryIDThisUser = "LV"; // Latvia
                break;
            case SystemLanguage.Lithuanian:
                CountryIDThisUser = "LT"; // Lithuania
                break;
            case SystemLanguage.Norwegian:
                CountryIDThisUser = "NO"; // Norway
                break;
            case SystemLanguage.Polish:
                CountryIDThisUser = "PL"; // Poland
                break;
            case SystemLanguage.Portuguese:
                CountryIDThisUser = "PT"; // Portugal
                break;
            case SystemLanguage.Romanian:
                CountryIDThisUser = "RO"; // Romania
                break;
            case SystemLanguage.Russian:
                CountryIDThisUser = "RU"; // Russia
                break;
            case SystemLanguage.SerboCroatian:
                CountryIDThisUser = "RS"; // Serbia
                break;
            case SystemLanguage.Slovak:
                CountryIDThisUser = "SK"; // Slovakia
                break;
            case SystemLanguage.Slovenian:
                CountryIDThisUser = "SI"; // Slovenia
                break;
            case SystemLanguage.Spanish:
                CountryIDThisUser = "ES"; // Spain
                break;
            case SystemLanguage.Swedish:
                CountryIDThisUser = "SE"; // Sweden
                break;
            case SystemLanguage.Thai:
                CountryIDThisUser = "TH"; // Thailand
                break;
            case SystemLanguage.Turkish:
                CountryIDThisUser = "TR"; // Turkey
                break;
            case SystemLanguage.Ukrainian:
                CountryIDThisUser = "UA"; // Ukraine
                break;
            case SystemLanguage.Vietnamese:
                CountryIDThisUser = "VN"; // Vietnam
                break;
            case SystemLanguage.ChineseSimplified:
                CountryIDThisUser = "CN"; // China (Simplified)
                break;
            case SystemLanguage.ChineseTraditional:
                CountryIDThisUser = "TW"; // Taiwan (Traditional)
                break;
            case SystemLanguage.Unknown:
                CountryIDThisUser = "Unknown"; // Unknown
                break;
            default:
                CountryIDThisUser = "Unknown"; // Default case
                break;
        }
    }
    public void AutoBonusScore()
    {
        if (GetCountTimeBonus() < 20)
        {
            return;
        }
        float rand = 0;
        foreach (var item in _listRankInfo)
        {
            if (item.Name == "me") continue;
            rand = Random.Range(_rangeScoreBonusPerSecond.x, _rangeScoreBonusPerSecond.y);
            item.Score += (int)(rand * (GetCountTimeBonus() / 20));
            item.SetScore(item.Score);
        }
        SortRank();
        ResetCountTimeAutoBonus();
    }
    public void PlusScoreMe()
    {
        ScoreMe++;
        GetRankUser().Score = ScoreMe;
    }
    public int ScoreMe
    {
        get { return PlayerPrefs.GetInt("me_score", 0); }
        set { PlayerPrefs.SetInt("me_score", value); }
    }
    public RankInfo GetRankUser()
    {
        foreach (RankInfo item in _listRankInfo)
        {
            if (item.Name == "me")
            {
                return item;
            }
        }
        return null;
    }
    public long CountTimeAutoBonus
    {
        get
        {
            long timeDefault = System.DateTime.Now.Ticks;
            return long.Parse(PlayerPrefs.GetString("count_time_auto_bonus_rank", timeDefault.ToString()));
        }
        set { PlayerPrefs.SetString("count_time_auto_bonus_rank", value.ToString()); }
    }
    private long GetCountTimeBonus()
    {
        System.DateTime date = new System.DateTime(CountTimeAutoBonus);
        System.TimeSpan timeSpan = System.DateTime.Now - date;
        return (long)timeSpan.TotalMinutes;
    }
    private void ResetCountTimeAutoBonus()
    {
        CountTimeAutoBonus = System.DateTime.Now.Ticks;
    }
    private bool IsLoadRank
    {
        get { return PlayerPrefs.GetInt("rank_loaded", 0) == 1; }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt("rank_loaded", 1);
            }
            else
                PlayerPrefs.SetInt("rank_loaded", 0);
        }
    }
}
[System.Serializable]
public class RankInfo
{
    public int IndexRank;
    public int Score;
    public string Name;

    public void SetScore(int score)
    {
        PlayerPrefs.SetInt($"{Name}_score", score);
    }
}
