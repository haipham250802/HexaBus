using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PopUpLose : PopUpBase
{
    [SerializeField] Button _claimMoreMoveAdsBtn;
    [SerializeField] Button _claimMoreMoveCoinBtn;
    [SerializeField] Button _retryBtn;
    [SerializeField] Image _iconClaimMoreCoinBtn;
    [SerializeField] GameObject[] _gEmojies;
    [SerializeField] Animator _animatorButtonCoin;
    [SerializeField] Animator _animatorButtonAds;
    private void OnEnable()
    {
        int rand = Random.Range(0, _gEmojies.Length);
        _gEmojies[rand].SetActive(true);
        _retryBtn.gameObject.SetActive(false);
        InitView();
        StartCoroutine(IE_delayActiveButtonRetry());
    }
    IEnumerator IE_delayActiveButtonRetry()
    {
        yield return new WaitForSecondsRealtime(3);
        _retryBtn.gameObject.SetActive(true);
    }
    private void InitView()
    {
        int currentCoin = GameController.Ins.GameData.CurrencyModel.Coin.Value;
        if(currentCoin >= 300)
        {
            _animatorButtonAds.speed = 0;
            _animatorButtonCoin.speed = 1;
        }
        else
        {
            _iconClaimMoreCoinBtn.color = Color.gray;
            _claimMoreMoveCoinBtn.interactable = false;
            _animatorButtonAds.speed = 1;
            _animatorButtonCoin.speed = 0;
        }
    }
    private void Start()
    {
        InitButton();
        GameController.Ins.SoundManager.PlaySoundElement(TypeSound.LOSE);
    }
    private void InitButton()
    {
        _claimMoreMoveAdsBtn.onClick.AddListener(OnClickButtonClaimMoreMove);
        _retryBtn.onClick.AddListener(OnClickButtonRetry);
    }
    private void OnClickButtonClaimMoreMove()
    {
        GameplayController.Ins.HexGridController.CountMove += 5;
        UITopGameplay.A_OnChangeValueCountMove?.Invoke(GameplayController.Ins.HexGridController.CountMove);
        UIGameplayController.Ins.AnimUIGamplayOn.Show();
        UIGameplayController.Ins.UIHintController.OnInteractableButtonHint();
        GameplayController.Ins.StateGame = E_STATE_GAME.NONE;
        Destroy(gameObject);
    }
    private void OnClickButtonRetry()
    {
        GameController.Ins.Loading(2);
    }
}
