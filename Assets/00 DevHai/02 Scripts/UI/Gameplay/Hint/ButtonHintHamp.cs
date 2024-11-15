using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHintHamp : ButtonHintBase
{
    [SerializeField] Image _imgHammer;
    protected override void DoSomethingClicked()
    {
        UIGameplayController.Ins.UIPopUp.ShowPopUpUnlockHex(
           () => {
               UIGameplayController.Ins.UIHintController.TypeUseHint = TypeUseHint.BREAK;
               base.DoSomethingClicked();
               UIGameplayController.Ins.UIHintController.OnUseHint();
               InputController.Ins.SetTypeInput(TypeInput.HAMMER);
           }, TypePopUpUnlock.HAMMER);
     
    }

    protected override void InitBooster()
    {
        base.InitBooster();
        if (_quantityBooster <= 0)
        {
            _imgHammer.color = Color.gray;
        }
    }
}
