using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class PopUpSetting : PopUpBase
{
    [SerializeField] Button _exitButton;
    [FoldoutGroup("Music")] [SerializeField] Button _stateMusicButton;
    [FoldoutGroup("Music")] [SerializeField] GameObject _gOnMusic;
    [FoldoutGroup("Music")] [SerializeField] GameObject _gOffMusic;

    [FoldoutGroup("Sound")] [SerializeField] Button _stateSoundButton;
    [FoldoutGroup("Sound")] [SerializeField] GameObject _gOnSound;
    [FoldoutGroup("Sound")] [SerializeField] GameObject _gOffSound;

    [FoldoutGroup("Vibrate")] [SerializeField] Button _stateVibrateButton;
    [FoldoutGroup("Vibrate")] [SerializeField] GameObject _gOnVibrate;
    [FoldoutGroup("Vibrate")] [SerializeField] GameObject _gOffVibrate;

    private void Start()
    {
        InitStart();
        InitButton();
    }
    private void InitStart()
    {
        InitMusic();
        InitSound();
        InitVibrate();
    }
    private void InitButton()
    {
        _stateMusicButton.onClick.AddListener(StateMusicClicked);
        _stateSoundButton.onClick.AddListener(StateSoundClicked);
        _stateVibrateButton.onClick.AddListener(StateVibrateClicked);
        _exitButton.onClick.AddListener(ExitClicked);
    }
    private void InitMusic()
    {
        _gOnMusic.SetActive(GameController.Ins.IsOnMusic);
        _gOffMusic.SetActive(!GameController.Ins.IsOnMusic);
    }
    private void InitSound()
    {
        _gOnSound.SetActive(GameController.Ins.IsOnSound);
        _gOffSound.SetActive(!GameController.Ins.IsOnSound);
    }
    private void InitVibrate()
    {
        _gOnVibrate.SetActive(GameController.Ins.IsOnVibrate);
        _gOffVibrate.SetActive(!GameController.Ins.IsOnVibrate);
    }
    private void StateMusicClicked()
    {
        GameController.Ins.IsOnMusic = !GameController.Ins.IsOnMusic;
        InitMusic();
    }
    private void StateSoundClicked()
    {
        GameController.Ins.IsOnSound = !GameController.Ins.IsOnSound;
        InitSound();
    }
    private void StateVibrateClicked()
    {
        GameController.Ins.IsOnVibrate = !GameController.Ins.IsOnVibrate;
        InitVibrate();
    }
    private void ExitClicked()
    {
        Hide();
    }
}
