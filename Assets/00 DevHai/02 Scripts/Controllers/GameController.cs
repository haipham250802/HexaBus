using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TypeScene
{
    NONE = -1,
    LOBBY,
    GAMEPLAY
}
public class GameController : MonoBehaviour
{
    public static GameController Ins;
    [SerializeField] GameConfig _gameConfig;
    [SerializeField] GameData _gameData;
    [SerializeField] DailyLoginController _dailyLoginController;
    [SerializeField] RankController _rankController;
    [SerializeField] SoundManager _soundManager;
    [SerializeField] UILoading _uiLoading;
    [SerializeField] LuckyWheelController _luckyWheelController;

    public GameData GameData => _gameData;
    public GameConfig GameConfig => _gameConfig;
    public DailyLoginController DailyLoginController => _dailyLoginController;
    public RankController RankController => _rankController;
    public SoundManager SoundManager => _soundManager;
    public LuckyWheelController LuckyWheelController => _luckyWheelController;
    public bool IsShowFirst;

    [Header("Level Test:")]
    public LevelController LevelTest;

    private void Awake()
    {
        if (Ins == null)
            Ins = this;
        else
            DestroyImmediate(gameObject);
        Loading(1);
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 300;
    }
    private void Start()
    {
    }
    public void Loading(int indexScene)
    {
        UILoading uiLoading = Instantiate(_uiLoading);
        if (!IsShowFirst)
        {
            IsShowFirst = true;
            uiLoading.Load(TypeLoading.FIRST_GAME, indexScene);
        }
        else
            uiLoading.Load(TypeLoading.IN_GAME, indexScene);
    }
    public bool IsOnMusic
    {
        get { return PlayerPrefs.GetInt("IsOnMusic", 1) == 1; }
        set
        {
            if (value)
                PlayerPrefs.SetInt("IsOnMusic", 1);
            else
                PlayerPrefs.SetInt("IsOnMusic", 0);
        }
    }
    public bool IsOnSound
    {
        get { return PlayerPrefs.GetInt("IsOnSound", 1) == 1; }
        set
        {
            if (value)
                PlayerPrefs.SetInt("IsOnSound", 1);
            else
                PlayerPrefs.SetInt("IsOnSound", 0);
        }
    }
    public bool IsOnVibrate
    {
        get { return PlayerPrefs.GetInt("IsOnVibrate", 1) == 1; }
        set
        {
            if (value)
                PlayerPrefs.SetInt("IsOnVibrate", 1);
            else
                PlayerPrefs.SetInt("IsOnVibrate", 0);
        }
    }

}
