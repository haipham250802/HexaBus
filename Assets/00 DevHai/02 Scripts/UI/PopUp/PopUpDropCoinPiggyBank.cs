using System.Collections;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.UI;
public class PopUpDropCoinPiggyBank : PopUpBase
{
    [SerializeField] int _quantityCoinDrop;
    [SerializeField] GameObject _coinPrefab;
    [SerializeField] Animator _animScalePig;
    [SerializeField] Transform _posEndCoin;
    [SerializeField] Transform _posOnCoin;
    [SerializeField] Image _progress;
    [SerializeField] Text _progresstxt;
    Coroutine _cor;
    int _currentValue;
    private void Start()
    {
        _currentValue = GameController.Ins.GameData.FeatureModel.GetQuantityPiggyBank();
        _progress.fillAmount = GetProgress(_currentValue);
        SetProgressTxt(_currentValue);
        ShowAnimFall();
    }
    private void ShowAnimFall()
    {
        _cor = StartCoroutine(IE_delayDropCoin());
    }
    IEnumerator IE_delayDropCoin()
    {
        for (int i = 0; i < _quantityCoinDrop; i++)
        {
            GameObject coinDropClone = Instantiate(_coinPrefab);
            coinDropClone.transform.SetParent(transform, false);
            coinDropClone.transform.position = _posOnCoin.position;
            bool showAnim = false;
            coinDropClone.transform.DOMoveY(_posEndCoin.position.y, 0.2f)
                .OnUpdate(() =>
                {
                    if (!showAnim)
                    {
                        if (coinDropClone.transform.position.y > _posEndCoin.position.y * 0.3f)
                        {
                            showAnim = true;
                            _animScalePig.SetTrigger("Drop");
                        }
                    }
                })
                .OnComplete(() =>
                {
                    coinDropClone.SetActive(false);
                });
            if (i + 1 >= _quantityCoinDrop)
            {
                yield return new WaitForSecondsRealtime(0.2f);
                _animScalePig.speed = 0;
                yield return new WaitForSecondsRealtime(0.3f);
                FillProgress();
                yield return new WaitForSecondsRealtime(1f);
                UIGameplayController.Ins.UIPopUp.ShowPopUpWin();
                StopCoroutine(_cor);
                Destroy(gameObject);
                yield break;
            }
            yield return new WaitForSecondsRealtime(0.18f);
        }
    }
    private void FillProgress()
    {
        int newVal = _currentValue + 200;
        GameController.Ins.GameData.FeatureModel.SetQuantityPiggyBank(newVal);
        _progress.DOFillAmount(GetProgress(newVal), 0.5f).OnComplete(() =>
        {
            SetProgressTxt(newVal);
        });
    }
    private float GetProgress(int value)
    {
        return (float)(value / 2000f);
    }
    private void SetProgressTxt(int value)
    {
        _progresstxt.text = $"{value}/2000";
    }
}
