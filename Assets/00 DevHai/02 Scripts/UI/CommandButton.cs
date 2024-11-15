using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandButton : MonoBehaviour
{
    Button _clickButton;
    private void Start()
    {
        _clickButton = GetComponent<Button>();
        _clickButton.onClick.AddListener(_buttonClicked);
    }
    private void _buttonClicked()
    {
        MMVibrationManager.Haptic(HapticTypes.RigidImpact, false, true, this);
        GameController.Ins.SoundManager.PlaySoundElement(TypeSound.CLICK_BUTTON);
    }
}
