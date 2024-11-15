using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpNoads : PopUpBase
{
    [SerializeField] Button _exitButton;
    [SerializeField] Button _breakButton;

    private void Start()
    {
        InitButton();
    }
    private void InitButton()
    {
        _exitButton.onClick.AddListener(ExitClicked);
    }
    private void ExitClicked()
    {
        Hide();
    }
}
