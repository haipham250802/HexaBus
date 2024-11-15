using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum E_STATE_GAME
{
    NONE = -1,
    WIN,
    LOSE
}
public class GameplayController : MonoBehaviour
{
    #region EVENT
    public static System.Action A_OnWin;
    #endregion
    public static GameplayController Ins;
    [SerializeField] E_STATE_GAME _currentStateGame = E_STATE_GAME.NONE;
    [SerializeField] HexGridController _hexGridController;
    [SerializeField] ResizeCamera _resizeCamera;
    [SerializeField] int _numberHexCanWin;
    [SerializeField] SpawnHexFillController _spawnHexFillController;
    public float _posLeftX;
    public float _posRightX;

    public float _posLeftY;
    public Transform _posYMoveUp;


    public PosPendingHandler PosPendingHandler;

    [Header("Game Configs")]
    public AnimationCurve AnimationCurve;
    public SpawnHexFillController SpawnHexFillController => _spawnHexFillController;
    public int NumebrHexCanWin
    {
        get { return _numberHexCanWin; }
        set
        {
            _numberHexCanWin = value;

        }
    }
    public E_STATE_GAME StateGame
    {
        get { return _currentStateGame; }
        set
        {
            _currentStateGame = value;
        }
    }
    public bool IsSetUpDone { get; set; }

    public HexGridController HexGridController
    {
        get { return _hexGridController; }
        set { _hexGridController = value; }
    }
    private void Awake()
    {
        if (Ins == null)
            Ins = this;
        else
            Destroy(gameObject);
    }
    public void OnStart()
    {
        IsSetUpDone = false;
        _hexGridController = FindObjectOfType<HexGridController>();
        _hexGridController.ResizeCameraWithLevel();
        _resizeCamera.OnResize();
        StartCoroutine(IE_delaySetUpOnGame());
        GameController.Ins.LuckyWheelController.ProgressSpinWheel++;
    }
    public void InitListTypeDirPending() // day la de lay het day du type tren map
    {
        if (_hexGridController == null)
            _hexGridController = FindObjectOfType<HexGridController>();
        List<HexGridElement> listHexGrid = new();
        List<HexGridElement> listHexGridSort = new();
        for (int i = 0; i < _hexGridController.ListHexaClone.Count; i++)
        {
            if (_hexGridController.ListHexaClone[i] != null)
            {
                if (_hexGridController.ListHexaClone[i].HexElementController != null)
                    listHexGrid.Add(_hexGridController.ListHexaClone[i]);
            }
        }
        foreach (var item in listHexGrid)
        {
            item.HexElementController.CheckCanExit();
        }
        //var danhSachSapXep = listHexGrid.OrderBy(h => h.GetHexElementController().ListHexMoveFowards.Count).ToList();
        for (int i = 0; i < listHexGrid.Count; i++)
        {
            for (int j = i + 1; j < listHexGrid.Count; j++)
            {
                if (listHexGrid[i].HexElementController.ListVisited.Count < listHexGrid[j].HexElementController.ListVisited.Count)
                {
                    HexGridElement hexTemp = listHexGrid[i];
                    listHexGrid[i] = listHexGrid[j];
                    listHexGrid[j] = hexTemp;
                }
            }
        }

        List<int> listHexGridSuffled = new();
        int count = 0;
        int sizeSuffle = 4;
        int capacitySuffle = 0;
        foreach (var item in listHexGrid)
        {
            if (!item) continue;
            if (item.HexElementController)
            {
                for (int i = 0; i < item.HexElementController.Capacity; i++)
                {
                    DirInfo dir = new();
                    dir.TypeDirHex = item.HexElementController.TypeDirHex;
                    dir.TypeColor = item.HexElementController.TypeColor;
                    SpawnHexFillController._listTypeDirHex.Add(dir);
                }
                capacitySuffle += item.HexElementController.Capacity;
                count++;
                if (count == sizeSuffle)
                {
                    listHexGridSuffled.Add(capacitySuffle);
                    count = 0;
                    capacitySuffle = 0;
                }
            }
        }
        
    }
    private void OnDisable()
    {
        HexGridController.OnChangeValueHexaFall -= MinusNumberHexCanWin;
    }
    private IEnumerator IE_delaySetUpOnGame()
    {
        ShowAnimHex();
        yield return new WaitForSecondsRealtime(1.8f);
        IsSetUpDone = true;
        HexGridController.OnChangeValueHexaFall += MinusNumberHexCanWin;
    }
    private void MinusNumberHexCanWin()
    {
        if (_numberHexCanWin > 0)
        {
            _numberHexCanWin--;
            if (_numberHexCanWin <= 0)
            {
                /* StateGame = E_STATE_GAME.WIN;
                 OnStateWin();*/
            }
        }
    }
    private void ShowAnimHex()
    {
        foreach (var item in _hexGridController.ListHexaClone)
        {
            if (item)
            {
                item.ActionAnim();
            }
        }
    }
    IEnumerator IE_DelayWin()
    {
        UIGameplayController.Ins.DosomethingEndGame();
        yield return new WaitForSecondsRealtime(1.2f);
        UIGameplayController.Ins.UIPopUp.ShowPopUpDropPiggyBank();
    }
    IEnumerator IE_DelayLose()
    {
        if (_numberHexCanWin > 0)
        {
            UIGameplayController.Ins.DosomethingEndGame();
        }
        yield return new WaitForSecondsRealtime(1.2f);
        if (_numberHexCanWin > 0)
        {
            UIGameplayController.Ins.UIPopUp.ShowPopUpLose();
        }
    }
    public void OnStateWin()
    {
        int currentLevel = TimerSystemIngame.Ins.LoadLevelController.CurrentLevel;
        TimerSystemIngame.Ins.LoadLevelController.CurrentLevel = currentLevel + 1;
        StartCoroutine(IE_DelayWin());
    }
    public void OnStateLose()
    {
        StateGame = E_STATE_GAME.LOSE;
        StartCoroutine(IE_DelayLose());
    }
}
