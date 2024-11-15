using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyController : MonoBehaviour
{
    public static UILobbyController Ins;
    [SerializeField] UITopLobby _uiTopLobby;
    [SerializeField] UIBottomLobby _uiBottomLobby;
    [SerializeField] Text _levelText;

    public UITopLobby UITopLobby => _uiTopLobby;
    public UIBottomLobby UIBottomLobby => _uiBottomLobby;
    private void Awake()
    {
        if (Ins == null)
            Ins = this;
        else
            DestroyImmediate(Ins);
        InitLevel();
    }
    private void InitLevel()
    {
        _levelText.text = $"{PlayerPrefs.GetInt("_current_level") + 1}";
    }
}
