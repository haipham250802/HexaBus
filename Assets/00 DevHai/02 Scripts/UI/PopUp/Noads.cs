using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Noads : MonoBehaviour
{
    [SerializeField] Button _purchaseButton;
    private void Start()
    {
        InitButton();
    }
    private void InitButton()
    {
        _purchaseButton.onClick.AddListener(NoadsClicked);
    }
    private void NoadsClicked()
    {

    }
}
