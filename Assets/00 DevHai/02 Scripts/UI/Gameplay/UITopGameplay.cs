using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
public class UITopGameplay : MonoBehaviour
{
    public static System.Action<int> A_OnChangeValueCountMove;

    [SerializeField] Text _levelTxt;
    [SerializeField] Text _countMoveTxt;
    [SerializeField] Button _settingButton;
    [SerializeField] Button _homeButton;
    [SerializeField] Button _noAds;


    Tweener _tweenZoomIn;
    Tweener _tweenZoomOut;
    public void OnStart()
    {
        OnChangeValueCountMove(GameplayController.Ins.HexGridController.CountMove);
        A_OnChangeValueCountMove += OnChangeValueCountMove;
        InitButton();
    }
    private void InitButton()
    {
        _settingButton.onClick.AddListener(SettingClicked);
        _homeButton.onClick.AddListener(HomeClicked);
        _noAds.onClick.AddListener(NoadsClicked);

    }
    private void NoadsClicked()
    {
        UIPopUp.Ins.ShowPopUpNoAds();
    }
    private void OnChangeValueCountMove(int count)
    {
      //  _countMoveTxt.text = $"move : {count}";
        if (count <= 5)
        {
            ScaleTextWarning();
          //  _countMoveTxt.color = Color.red;
        }
        else
        {
            _tweenZoomIn?.Kill();
            _tweenZoomOut?.Kill();
       //     _countMoveTxt.color = Color.blue;
        //    _countMoveTxt.transform.localScale = Vector3.one;
        }
    }
    private void ScaleTextWarning()
    {
        _tweenZoomIn?.Kill();
        _tweenZoomOut?.Kill();
        _tweenZoomIn = _countMoveTxt.transform.DOScale(Vector3.one * 1.1f, .8f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _tweenZoomOut = _countMoveTxt.transform.DOScale(Vector3.one, .8f)
                .SetEase(Ease.Linear)
                  .OnComplete(() =>
                  {
                      ScaleTextWarning();
                  });
            });
    }
    private void HomeClicked()
    {
        GameController.Ins.Loading(1);
    }
    private void SettingClicked()
    {
        UIGameplayController.Ins.UIPopUp.ShowPopUpSetting();
    }
    private void OnDisable()
    {
        A_OnChangeValueCountMove -= OnChangeValueCountMove;
    }
}
