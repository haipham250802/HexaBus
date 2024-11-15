using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HexSwapThree : MonoBehaviour
{
    [OnValueChanged("OnChangeValueAngleOfTypeThree")]
    [SerializeField] TypeSwapAxisThree _typeSwapAxisThree;
    [SerializeField] HexGridElement _currentHex;
    [SerializeField] HexGridElement _hexGridSwap01;
    [SerializeField] HexGridElement _hexGridSwap02;
    [SerializeField] HexGridElement _hexGridSwap03;

    [SerializeField] HexElementController _hexController01;
    [SerializeField] HexElementController _hexController02;
    [SerializeField] HexElementController _hexController03;

    [SerializeField] Transform _AxisAroundArow;
    [SerializeField] Transform _point1;
    [SerializeField] Transform _point2;
    [SerializeField] Transform _point3;
    [SerializeField] Transform _button;
    bool isSwapping = false;
    bool isSwapped = false;

    public TypeSwapAxisThree TypeSwapAxisThree
    {
        get { return _typeSwapAxisThree; }
        set { _typeSwapAxisThree = value; }
    }
    private void InitHex()
    {
        switch (_typeSwapAxisThree)
        {
            case TypeSwapAxisThree.NONE:
                break;
            case TypeSwapAxisThree.TOP_DOWNRIGHT_DOWNLEFT:
                _hexGridSwap01 = _currentHex.GetHexNeighborOfType(TypeDirHex.TOP);
                _hexGridSwap02 = _currentHex.GetHexNeighborOfType(TypeDirHex.DOWN_RIGHT);
                _hexGridSwap03 = _currentHex.GetHexNeighborOfType(TypeDirHex.DOWN_LEFT);
                break;
            case TypeSwapAxisThree.TOPLEFT_TOPRIGHT_DOWN:
                _hexGridSwap01 = _currentHex.GetHexNeighborOfType(TypeDirHex.TOP_LEFT);
                _hexGridSwap02 = _currentHex.GetHexNeighborOfType(TypeDirHex.TOP_RIGHT);
                _hexGridSwap03 = _currentHex.GetHexNeighborOfType(TypeDirHex.DOWN);
                break;
            case TypeSwapAxisThree.TOPRIGHT_DOWN_TOPLEFT:
                _hexGridSwap01 = _currentHex.GetHexNeighborOfType(TypeDirHex.TOP_RIGHT);
                _hexGridSwap02 = _currentHex.GetHexNeighborOfType(TypeDirHex.DOWN);
                _hexGridSwap03 = _currentHex.GetHexNeighborOfType(TypeDirHex.TOP_LEFT);
                break;
            case TypeSwapAxisThree.DOWN_TOPLEFT_TOPRIGHT:
                _hexGridSwap01 = _currentHex.GetHexNeighborOfType(TypeDirHex.DOWN);
                _hexGridSwap02 = _currentHex.GetHexNeighborOfType(TypeDirHex.TOP_LEFT);
                _hexGridSwap03 = _currentHex.GetHexNeighborOfType(TypeDirHex.TOP_RIGHT);
                break;
            case TypeSwapAxisThree.DOWNLEFT_TOP_DOWNRIGHT:
                _hexGridSwap01 = _currentHex.GetHexNeighborOfType(TypeDirHex.DOWN_LEFT);
                _hexGridSwap02 = _currentHex.GetHexNeighborOfType(TypeDirHex.TOP);
                _hexGridSwap03 = _currentHex.GetHexNeighborOfType(TypeDirHex.DOWN_RIGHT);
                break;
            case TypeSwapAxisThree.DOWNRIGHT_DOWNLEFT_TOP:
                _hexGridSwap01 = _currentHex.GetHexNeighborOfType(TypeDirHex.DOWN_RIGHT);
                _hexGridSwap02 = _currentHex.GetHexNeighborOfType(TypeDirHex.DOWN_LEFT);
                _hexGridSwap03 = _currentHex.GetHexNeighborOfType(TypeDirHex.TOP);
                break;
            default:
                break;
        }
        if (_hexGridSwap01)
        {
            if (_hexGridSwap01.GetHexElementControllerSwap())
                _hexController01 = _hexGridSwap01.GetHexElementControllerSwap();
        }
        if (_hexGridSwap02)
        {
            if (_hexGridSwap02.GetHexElementControllerSwap())
                _hexController02 = _hexGridSwap02.GetHexElementControllerSwap();
        }
        if (_hexGridSwap03)
        {
            if (_hexGridSwap03.GetHexElementControllerSwap())
                _hexController03 = _hexGridSwap03.GetHexElementControllerSwap();
        }
    }
    private void InitHexController()
    {
        if (_hexGridSwap01.GetHexElementControllerSwap())
            _hexController01 = _hexGridSwap01.GetHexElementControllerSwap();
        else
            _hexController01 = null;
        if (_hexGridSwap02.GetHexElementControllerSwap())
            _hexController02 = _hexGridSwap02.GetHexElementControllerSwap();
        else
            _hexController02 = null;
        if (_hexGridSwap03.GetHexElementControllerSwap())
            _hexController03 = _hexGridSwap03.GetHexElementControllerSwap();
        else
            _hexController03 = null;
    }
    public void ButtonDown()
    {
        DoSomethingAxisThreeClicked();
    }
    private void DoSomethingAxisThreeClicked()
    {
        if (isSwapped)
            return;
        GameplayController.Ins.HexGridController.MinusMove();
        isSwapped = true;
        InitHexController();
        Vector3 posNormal = new Vector3(0, 0.01f, 0);
        _button.transform.localPosition = posNormal;
        if (_hexController01 == null && _hexController02 == null && _hexController03 == null)
        {
            if (_hexGridSwap01.TypeHexGrid != TypeHexGrid.STOP)
                _hexGridSwap01.TypeHexGrid = TypeHexGrid.SAFE;
            if (_hexGridSwap02.TypeHexGrid != TypeHexGrid.STOP)
                _hexGridSwap02.TypeHexGrid = TypeHexGrid.SAFE;
            if (_hexGridSwap03.TypeHexGrid != TypeHexGrid.STOP)
                _hexGridSwap03.TypeHexGrid = TypeHexGrid.SAFE;
            _button.transform.DOLocalMoveY(0.005f, 0.15f).OnComplete(() =>
            {
                _button.transform.DOLocalMoveY(posNormal.y + 0.01f, 0.15f).OnComplete(() =>
                {
                    float Yaxis = _button.localRotation.eulerAngles.y;

                    DOTween.To(() => Yaxis, _ =>
                    {
                        _button.transform.localRotation = Quaternion.Euler(_button.localRotation.eulerAngles.x, _, _button.localRotation.eulerAngles.z);
                    }, Yaxis + 120, 0.65f)
                    .SetDelay(0.1f)
                    .OnComplete(() =>
                    {
                        _button.transform.DOLocalMoveY(0.01f, 0.15f)
                        .SetDelay(0.1f)
                        .OnComplete(() =>
                        {
                            isSwapped = false;
                        });
                    });
                });
            });
        }
        else
        {
            {
                if (_hexController01)
                {
                    if (_hexGridSwap01.TypeHexGrid != TypeHexGrid.STOP)
                        _hexGridSwap01.TypeHexGrid = TypeHexGrid.OBSTACLE;
                }
                else
                {
                    if (_hexGridSwap01.TypeHexGrid == TypeHexGrid.OBSTACLE)
                        _hexGridSwap01.TypeHexGrid = TypeHexGrid.SAFE;
                }
            }
            {
                if (_hexController02)
                {
                    if (_hexGridSwap02.TypeHexGrid != TypeHexGrid.STOP)
                        _hexGridSwap02.TypeHexGrid = TypeHexGrid.OBSTACLE;
                }
                else
                {
                    if (_hexGridSwap02.TypeHexGrid == TypeHexGrid.OBSTACLE)
                        _hexGridSwap02.TypeHexGrid = TypeHexGrid.SAFE;
                }
            }
            {
                if (_hexController03)
                {
                    if (_hexGridSwap03.TypeHexGrid != TypeHexGrid.STOP)
                        _hexGridSwap03.TypeHexGrid = TypeHexGrid.OBSTACLE;
                }
                else
                {
                    if (_hexGridSwap03.TypeHexGrid == TypeHexGrid.OBSTACLE)
                        _hexGridSwap03.TypeHexGrid = TypeHexGrid.SAFE;
                }
            }
            _button.transform.DOLocalMoveY(0.005f, 0.15f).OnComplete(() =>
            {
                if (_hexController01)
                    _hexController01.SetSwapping(true);
                if (_hexController02)
                    _hexController02.SetSwapping(true);
                if (_hexController03)
                    _hexController03.SetSwapping(true);
                if (!isSwapping)
                {
                    if (_hexController01)
                    {
                        if (!_hexController01.IsDone)
                        {
                            _hexController01.SetParentNew(_point1);

                        }
                    }
                    if (_hexController02)
                    {
                        if (!_hexController02.IsDone)
                        {

                            _hexController02.SetParentNew(_point2);
                        }
                    }
                    if (_hexController03)
                    {
                        if (!_hexController03.IsDone)
                        {
                            _hexController03.SetParentNew(_point3);
                        }
                    }
                }
                else
                {
                    if (_hexController01)
                    {
                        if (!_hexController01.IsDone)
                        {
                            _hexController01.SetParentNew(_point2);
                        }
                    }
                    if (_hexController02)
                    {
                        if (!_hexController02.IsDone)
                        {
                            _hexController02.SetParentNew(_point1);
                        }
                    }
                    if (_hexController03)
                    {
                        if (!_hexController03.IsDone)
                        {
                            _hexController03.SetParentNew(_point3);
                        }
                    }

                }

                _button.transform.DOLocalMoveY(posNormal.y + 0.01f, 0.15f).OnComplete(() =>
                {
                    float Yaxis = _button.localRotation.eulerAngles.y;
                    if (_hexController01)
                    {
                        _hexController01.IsSwapping = true;
                        if (_hexGridSwap01.TypeHexGrid != TypeHexGrid.STOP)
                            _hexController01.HexCache.TypeHexGrid = TypeHexGrid.SAFE;
                    }
                    if (_hexController02)
                    {
                        _hexController02.IsSwapping = true;
                        if (_hexGridSwap02.TypeHexGrid != TypeHexGrid.STOP)
                            _hexController02.HexCache.TypeHexGrid = TypeHexGrid.SAFE;
                    }
                    if (_hexController03)
                    {
                        _hexController03.IsSwapping = true;
                        if (_hexGridSwap03.TypeHexGrid != TypeHexGrid.STOP)
                            _hexController03.HexCache.TypeHexGrid = TypeHexGrid.SAFE;
                    }
                    DOTween.To(() => Yaxis, _ =>
                    {
                        _button.transform.localRotation = Quaternion.Euler(_button.localRotation.eulerAngles.x, _, _button.localRotation.eulerAngles.z);
                    }, Yaxis + 120, 0.65f)
                    .SetDelay(0.1f)
                    .OnComplete(() =>
                    {
                        _button.transform.DOLocalMoveY(0.01f, 0.15f)
                        .SetDelay(0.1f)
                        .OnComplete(() =>
                        {
                            if (_hexController01)
                            {
                                if (!_hexController01.IsDone)
                                {
                                    _hexController01.SetParentNew(_hexGridSwap02.ParentContainerHex);
                                    _hexController01.HexCache = _hexGridSwap02;
                                    if (_hexGridSwap02.TypeHexGrid != TypeHexGrid.STOP)
                                        _hexGridSwap02.TypeHexGrid = TypeHexGrid.OBSTACLE;
                                    _hexController01.ResetScan();
                                }
                            }
                            if (_hexController02)
                            {
                                if (!_hexController02.IsDone)
                                {
                                    _hexController02.SetParentNew(_hexGridSwap03.ParentContainerHex);
                                    _hexController02.HexCache = _hexGridSwap03;
                                    if (_hexGridSwap03.TypeHexGrid != TypeHexGrid.STOP)
                                        _hexGridSwap03.TypeHexGrid = TypeHexGrid.OBSTACLE;
                                    _hexController02.ResetScan();
                                }
                            }
                            if (_hexController03)
                            {
                                if (!_hexController03.IsDone)
                                {
                                    _hexController03.SetParentNew(_hexGridSwap01.ParentContainerHex);
                                    _hexController03.HexCache = _hexGridSwap01;
                                    if (_hexGridSwap01.TypeHexGrid != TypeHexGrid.STOP)
                                        _hexGridSwap01.TypeHexGrid = TypeHexGrid.OBSTACLE;
                                    _hexController03.ResetScan();
                                }
                            }
                            ResetHexController();
                            _hexGridSwap01.CheckState();
                            _hexGridSwap02.CheckState();
                            _hexGridSwap03.CheckState();
                            isSwapped = false;
                        });
                    });
                });
            });
        }
    }
    private bool CheckCanResetSafe(HexGridElement hex)
    {
        if (hex.TypeHexGrid == TypeHexGrid.UNLOCK_TURN_MOVE ||
            hex.TypeHexGrid == TypeHexGrid.UNLOCK_ADS ||
            hex.TypeHexGrid == TypeHexGrid.STOP)
            return false;
        return true;
    }
    private void ResetHexController()
    {
        if (_hexController01)
        {
            _hexController01.ResetPosY();
            _hexController01.IsSwapping = false;
            _hexController01.SetSwapping(false);
            _hexController01.IsMoveForward = false;
        }
        if (_hexController02)
        {
            _hexController02.ResetPosY();
            _hexController02.IsSwapping = false;
            _hexController02?.SetSwapping(false);
            _hexController02.IsMoveForward = false;
        }
        if (_hexController03)
        {
            _hexController03.ResetPosY();
            _hexController03.IsSwapping = false;
            _hexController03?.SetSwapping(false);
            _hexController03.IsMoveForward = false;
        }
    }
    public void OnChangeValueAngleOfTypeThree()
    {
        switch (_typeSwapAxisThree)
        {
            case TypeSwapAxisThree.NONE:
                break;
            case TypeSwapAxisThree.TOP_DOWNRIGHT_DOWNLEFT:
                _AxisAroundArow.localRotation = Quaternion.Euler(0, 60, 0);
                break;
            case TypeSwapAxisThree.TOPLEFT_TOPRIGHT_DOWN:
                _AxisAroundArow.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case TypeSwapAxisThree.TOPRIGHT_DOWN_TOPLEFT:
                _AxisAroundArow.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case TypeSwapAxisThree.DOWN_TOPLEFT_TOPRIGHT:
                _AxisAroundArow.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case TypeSwapAxisThree.DOWNLEFT_TOP_DOWNRIGHT:
                _AxisAroundArow.localRotation = Quaternion.Euler(0, 60, 0);
                break;
            case TypeSwapAxisThree.DOWNRIGHT_DOWNLEFT_TOP:
                _AxisAroundArow.localRotation = Quaternion.Euler(0, 60, 0);
                break;
                /* case TypeSwapAxisTwo.NONE:
break;
case TypeSwapAxisTwo.TOP_DOWN:
_AxisAroundArow.localRotation = Quaternion.Euler(0, 90, 0);
break;
case TypeSwapAxisTwo.TOP_LEFT_DOWN_RIGHT:
_AxisAroundArow.localRotation = Quaternion.Euler(0, 30, 0);
break;
case TypeSwapAxisTwo.TOP_RIGHT_DOWN_LEFT:
_AxisAroundArow.localRotation = Quaternion.Euler(0, 150, 0);
break;
default:
break;*/
        }
        InitHex();
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
}
