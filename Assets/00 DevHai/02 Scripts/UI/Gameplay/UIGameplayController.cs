using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameplayController : MonoBehaviour
{
    public static UIGameplayController Ins;
    [SerializeField] UIHintController _uiHintController;
    [SerializeField] UITopGameplay _uiTopGameplay;
    [SerializeField] UIPopUp _uiPopUp;
    [SerializeField] AnimUIGamplayOn _animUIGameplayOn;
    public AnimUIGamplayOn AnimUIGamplayOn => _animUIGameplayOn;
    public UIHintController UIHintController => _uiHintController;
    public UITopGameplay UITopGameplay => _uiTopGameplay;
    public UIPopUp UIPopUp => _uiPopUp;
    private void Awake()
    {
        if (Ins == null)
            Ins = this;
        else
            Destroy(gameObject);
    }
    public void OnStart()
    {
        UITopGameplay.OnStart();
        _animUIGameplayOn.Show();
    }
    public void DosomethingEndGame()
    {
        UIHintController.DeInteractableButtonHint();
        _animUIGameplayOn.Hide();
    }
}
