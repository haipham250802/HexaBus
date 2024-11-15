using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using Sirenix.OdinInspector;
public class PopUpWin : PopUpBase
{
    [SerializeField] ObjectMove _coinFly;
    [SerializeField] Transform _posSpawn;
    [SerializeField] Transform _posTarget;
    [SerializeField] Button _nextButton;
    [SerializeField] Button _bonusX2;
    [SerializeField] GameObject[] _gEmojies;
    List<ObjectMove> _listCoin = new();
    private void OnEnable()
    {
        int rand = Random.Range(0, _gEmojies.Length);
        _gEmojies[rand].SetActive(true);
        GameController.Ins.RankController.PlusScoreMe();
        GameController.Ins.LuckyWheelController.ProgressSpinWheel++;
    }
    private void Start()
    {
        InitButton();
        GameController.Ins.SoundManager.PlaySoundElement(TypeSound.WIN);
    }
    private void InitButton()
    {
        _nextButton.onClick.AddListener(NextClicked);
        _bonusX2.onClick.AddListener(BonusX2Clicked);
    }
    private void BonusX2Clicked()
    {
        LockButton();
        StartCoroutine(IE_delaySpawnCoin(true));
    }
    private void NextClicked()
    {
        LockButton();
        StartCoroutine(IE_delaySpawnCoin());
    }
    private void LockButton()
    {
        _nextButton.interactable = false;
        _bonusX2.interactable = false;
    }
    IEnumerator IE_delaySpawnCoin(bool isX2Bonus = false)
    {
        for (int i = 0; i < 10; i++)
        {
            ObjectMove obj = Instantiate(_coinFly);
            obj.transform.SetParent(transform, false);
            Vector3 pos = Random.insideUnitSphere * 0.8f + _posSpawn.position;
            obj.transform.position = pos;
            obj.OnActive();
            _listCoin.Add(obj);
            yield return null;
        }
        yield return new WaitForSecondsRealtime(.3f);
        for (int i = 0; i < _listCoin.Count; i++)
        {
            _listCoin[i].MoveToTarget(_posTarget.position);
            yield return new WaitForSecondsRealtime(Random.Range(0.04f, .06f));
        }
        if (isX2Bonus)
            GameController.Ins.GameData.CurrencyModel.Coin.Value += 20;
        else
            GameController.Ins.GameData.CurrencyModel.Coin.Value += 10;
        yield return new WaitForSecondsRealtime(0.5f);
        GameController.Ins.Loading(2);
    }
}
