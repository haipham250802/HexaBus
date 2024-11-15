using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEditor;

public enum TypeAxis
{
    NONE = -1,
    TWO,
    THREE
}
public enum TypeSwapAxisThree
{
    NONE = -1,
    TOP_DOWNRIGHT_DOWNLEFT,
    TOPLEFT_TOPRIGHT_DOWN,
    TOPRIGHT_DOWN_TOPLEFT,
    DOWN_TOPLEFT_TOPRIGHT,
    DOWNLEFT_TOP_DOWNRIGHT,
    DOWNRIGHT_DOWNLEFT_TOP
}
public enum TypeSwapAxisTwo
{
    NONE = -1,
    TOP_DOWN,
    TOP_LEFT_DOWN_RIGHT,
    TOP_RIGHT_DOWN_LEFT,
}
public class HexaSwapAxisController : MonoBehaviour
{
    [OnValueChanged("OnChangeValueAxis")]
    [SerializeField] TypeAxis _typeAxis;
    [SerializeField] HexaSwapTwo _hexSwapTwo;
    [SerializeField] HexSwapThree _hexSwapThree;
    public HexaSwapTwo HexaSwapTwo => _hexSwapTwo;
    public HexSwapThree HexaSwapThree => _hexSwapThree;
    public TypeAxis TypeAxis
    {
        get { return _typeAxis; }
        set {
            _typeAxis = value;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
    public void ButtonDown()
    {
        switch (_typeAxis)
        {
            case TypeAxis.NONE:
                break;
            case TypeAxis.TWO:
                _hexSwapTwo.ButtonDown();
                break;
            case TypeAxis.THREE:
                _hexSwapThree.ButtonDown();
                break;
            default:
                break;
        }
    }
    public void OnChangeValueAxis()
    {
        _hexSwapTwo.gameObject.SetActive(false);
        _hexSwapThree.gameObject.SetActive(false);

        switch (_typeAxis)
        {
            case TypeAxis.NONE:
                break;
            case TypeAxis.TWO:
                _hexSwapTwo.gameObject.SetActive(true);
                break;
            case TypeAxis.THREE:
                _hexSwapThree.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
}
