using UnityEngine;
using UnityEngine.UI;

public class PopUpUnlockHex : PopUpBase
{
    [SerializeField] Image _icon;
    [SerializeField] Image _iconPurchaseCoinButton;
    [SerializeField] Button _purchaseButtonAds;
    [SerializeField] Button _purchaseButtonCoin;
    [SerializeField] Button _exitButton;
    [SerializeField] DataPopUpUnlock _dataPopUpUnlock;
    public System.Action ActionButton;
    public DataPopUpUnlock DataPopUpUnlock => _dataPopUpUnlock;
    private void Start()
    {
        InitButton();
        InitView();
        ActionButton += Hide;
    }
    private void InitView()
    {
        if (GameController.Ins.GameData.CurrencyModel.Coin.Value < 300)
        {
            _purchaseButtonCoin.interactable = false;
            _iconPurchaseCoinButton.color = Color.gray;
        }
    }
    private void InitButton()
    {
        _purchaseButtonAds.onClick.AddListener(OnClickPurchaseButtonAds);
        _purchaseButtonCoin.onClick.AddListener(OnClickPurchaseButtonCoin);
        _exitButton.onClick.AddListener(OnClickButtonExit);
    }
    private void OnClickPurchaseButtonAds()
    {
        ActionButton?.Invoke();
        InitViewPurchased();
    }
    private void OnClickPurchaseButtonCoin()
    {
        ActionButton?.Invoke();
        InitViewPurchased();
    }
    private void OnClickButtonExit()
    {
        Hide();
    }
    public void SetIcon(Sprite icon)
    {
        _icon.sprite = icon;
        _icon.SetNativeSize();
    }
    private void InitViewPurchased()
    {
        _purchaseButtonAds.interactable = false;
        _purchaseButtonCoin.interactable = false;
    }
    private void OnDisable()
    {
        ActionButton -= Hide;
    }
}