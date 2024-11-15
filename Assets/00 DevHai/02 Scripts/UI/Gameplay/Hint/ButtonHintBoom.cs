using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHintBoom : ButtonHintBase
{
    [SerializeField] Image _imgBoom;
    private bool _isUseHintBoom;
    protected override void Start()
    {
        base.Start();
        _imgBoom.color = Color.gray;
        _notiPlusQuantity.gameObject.SetActive(false);
        ClickButton.interactable = false;
        HexGridController.OnChangeValueHexaFall += UnlockHintBoom;
    }
    protected override void InitBooster()
    {
        base.InitBooster();
        if (_quantityBooster <= 0)
        {
            _imgBoom.color = Color.gray;
        }
    }
    private void UnlockHintBoom()
    {
        if (!_isUseHintBoom)
        {
            _isUseHintBoom = true;
            ClickButton.interactable = true;
            InitBooster();
            HexGridController.OnChangeValueHexaFall -= UnlockHintBoom;
        }
    }
    protected override void DoSomethingClicked()
    {
        UIGameplayController.Ins.UIPopUp.ShowPopUpUnlockHex(
            ()=> {
                UIGameplayController.Ins.UIHintController.TypeUseHint = TypeUseHint.BOMB;
                base.DoSomethingClicked();
                UIGameplayController.Ins.UIHintController.OnUseHint();
                InputController.Ins.SetTypeInput(TypeInput.BOOM);
            }, TypePopUpUnlock.BOMB);
    }
    private void OnDisable()
    {
        HexGridController.OnChangeValueHexaFall -= UnlockHintBoom;
    }
}
