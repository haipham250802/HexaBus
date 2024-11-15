using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHintReplay : ButtonHintBase
{
    protected override void DoSomethingClicked()
    {
        base.DoSomethingClicked();
        GameController.Ins.Loading(2);
    }
}
