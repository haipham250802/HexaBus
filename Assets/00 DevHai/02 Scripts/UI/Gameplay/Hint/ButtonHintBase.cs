using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
public class ButtonHintBase : MonoBehaviour
{
    [SerializeField] TypeBooster _typeBooster;
    [SerializeField] Button _clickButton;
    [SerializeField] Text _quantityTxt;
    [SerializeField] protected GameObject _notiQuantity;
    [SerializeField] protected GameObject _notiPlusQuantity;
    protected int _quantityBooster;
    public Button ClickButton
    {
        get { return _clickButton; }
        set { _clickButton = value; }
    }
    protected virtual void Start()
    {
        _clickButton.onClick.AddListener(Clicked);
        InitBooster();
        GameController.Ins.GameData.BoosterModel.Bomb.Subscribe(_ => InitBooster()).AddTo(this);
        GameController.Ins.GameData.BoosterModel.Hammer.Subscribe(_ => InitBooster()).AddTo(this);
    }
    private void Clicked()
    {
        if (GameplayController.Ins.StateGame == E_STATE_GAME.WIN ||
            GameplayController.Ins.StateGame == E_STATE_GAME.LOSE)
            return;
        DoSomethingClicked();
    }
    protected virtual void DoSomethingClicked()
    {
    }
    protected virtual void InitBooster()
    {
        switch (_typeBooster)
        {
            case TypeBooster.NONE:
                break;
            case TypeBooster.BOMB:
                _quantityBooster = GameController.Ins.GameData.BoosterModel.Bomb.Value;
                break;
            case TypeBooster.HAMMER:
                _quantityBooster = GameController.Ins.GameData.BoosterModel.Hammer.Value;
                break;
            default:
                break;
        }
        if (_quantityBooster > 0)
            SetActiveNoti(true);
        else
            SetActiveNoti(false);
    }
    private void SetActiveNoti(bool value)
    {
        if (_notiQuantity)
            _notiQuantity.SetActive(value);
        if (_notiPlusQuantity)
            _notiPlusQuantity.SetActive(!value);
    }
}
