using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIBottomLobby : MonoBehaviour
{
    [SerializeField] Button _playButton;
    [SerializeField] Button _shopButton;
    [SerializeField] Button _rankButton;
    private void Start()
    {
        InitButton();
    }
    private void InitButton()
    {
        _playButton.onClick.AddListener(PlayClicked);
        _shopButton.onClick.AddListener(ShopClicked);
        _rankButton.onClick.AddListener(RankClicked);
    }
    private void PlayClicked()
    {
        GameController.Ins.Loading(2);
        GameController.Ins.SoundManager.PlaySoundBG(TypeScene.GAMEPLAY);
    }
    private void ShopClicked()
    {
        UIPopUp.Ins.ShowPopUpShop();
    }
    private void RankClicked()
    {
        UIPopUp.Ins.ShowPopUpRank();
    }
}
