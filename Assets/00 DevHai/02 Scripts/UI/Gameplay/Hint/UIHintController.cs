using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TypeUseHint
{
    NONE = -1,
    BOMB,
    BREAK
}
public class UIHintController : MonoBehaviour
{
    public static System.Action A_OnUseHintBreak;
    public static System.Action A_OnEndHintBreak;

    public static System.Action A_OnUseHintBomb;
    public static System.Action A_OnEndHintBomb;

    [SerializeField] TypeUseHint _currentTypeHintUsing;
    [SerializeField] List<ButtonHintBase> _listHint = new();
    [SerializeField] GameObject _gTextGuidHintBomb;
    [SerializeField] GameObject _gTextGuidHintBreak;
    [SerializeField] GameObject _gBGHint;
    public TypeUseHint TypeUseHint
    {
        get { return _currentTypeHintUsing; }
        set { _currentTypeHintUsing = value; }
    }
    public void OnUseHint()
    {
        foreach (var item in _listHint)
        {
            item.gameObject.SetActive(false);
        }
        _gBGHint.SetActive(false);
        ActiveDesc(true);
    }
    public void OnEndHint()
    {
        foreach (var item in _listHint)
        {
            item.gameObject.SetActive(true);
        }
        _gBGHint.SetActive(true);
        ActiveDesc(false);
        _currentTypeHintUsing = TypeUseHint.NONE;
        InputController.Ins.SetTypeInput(TypeInput.NORMAL);
    }
    private void ActiveDesc(bool value)
    {
        switch (TypeUseHint)
        {
            case TypeUseHint.NONE:
                break;
            case TypeUseHint.BOMB:
                _gTextGuidHintBomb.gameObject.SetActive(value);
                break;
            case TypeUseHint.BREAK:
                _gTextGuidHintBreak.gameObject.SetActive(value);
                break;
            default:
                break;
        }
    }
    public void DeInteractableButtonHint()
    {
        foreach (var item in _listHint)
        {
            item.ClickButton.interactable = false;
        }
    }
    public void OnInteractableButtonHint()
    {
        foreach (var item in _listHint)
        {
            item.ClickButton.interactable = true;
        }
    }
}
