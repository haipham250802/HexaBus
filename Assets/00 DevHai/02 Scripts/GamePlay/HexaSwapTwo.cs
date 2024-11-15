using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HexaSwapTwo : MonoBehaviour
{
    [OnValueChanged("OnChangeValueAngleOfTypeTwo")]
    [SerializeField] TypeSwapAxisTwo _typeSwapAxis;
    [SerializeField] HexGridElement _currentHex;
    [SerializeField] HexGridElement _hexGridSwap01;
    [SerializeField] HexGridElement _hexGridSwap02;

    [SerializeField] HexElementController _hexController01;
    [SerializeField] HexElementController _hexController02;

    [SerializeField] Transform _AxisAroundArow;
    [SerializeField] Transform _point1;
    [SerializeField] Transform _point2;
    [SerializeField] Transform _button;
    public TypeSwapAxisTwo TypeSwapAxisTwo
    {
        get { return _typeSwapAxis; }
        set { _typeSwapAxis = value; }
    }
    bool isSwapping = false;
    bool isSwapped = false;
    private void InitHex()
    {
        switch (_typeSwapAxis)
        {
            case TypeSwapAxisTwo.NONE:
                break;
            case TypeSwapAxisTwo.TOP_DOWN:
                _hexGridSwap01 = _currentHex.GetHexNeighborOfType(TypeDirHex.TOP);
                _hexGridSwap02 = _currentHex.GetHexNeighborOfType(TypeDirHex.DOWN);
                break;
            case TypeSwapAxisTwo.TOP_LEFT_DOWN_RIGHT:
                _hexGridSwap01 = _currentHex.GetHexNeighborOfType(TypeDirHex.TOP_LEFT);
                _hexGridSwap02 = _currentHex.GetHexNeighborOfType(TypeDirHex.DOWN_RIGHT);
                break;
            case TypeSwapAxisTwo.TOP_RIGHT_DOWN_LEFT:
                _hexGridSwap01 = _currentHex.GetHexNeighborOfType(TypeDirHex.TOP_RIGHT);
                _hexGridSwap02 = _currentHex.GetHexNeighborOfType(TypeDirHex.DOWN_LEFT);
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
    }
    private void InitHexController()
    {
        if (_hexGridSwap01.GetHexElementControllerSwap())
        {
            _hexController01 = _hexGridSwap01.GetHexElementControllerSwap();
        }
        else
            _hexController01 = null;
        if (_hexGridSwap02.GetHexElementControllerSwap())
        {
            _hexController02 = _hexGridSwap02.GetHexElementControllerSwap();
        }
        else
            _hexController02 = null;
    }
    public void ButtonDown()
    {
        DoSomethingAxisTwoClicked();
    }
    private void DoSomethingAxisTwoClicked()
    {
        if (isSwapped)
            return;
        GameplayController.Ins.HexGridController.MinusMove();
        isSwapped = true;
        InitHexController();
        Vector3 posNormal = new Vector3(0, 0.01f, 0);
        _button.transform.localPosition = posNormal;
        if (_hexController01 == null && _hexController02 == null)
        {
            if (_hexGridSwap02.TypeHexGrid != TypeHexGrid.STOP)
                _hexGridSwap02.TypeHexGrid = TypeHexGrid.SAFE;

            if (_hexGridSwap01.TypeHexGrid != TypeHexGrid.STOP)
                _hexGridSwap01.TypeHexGrid = TypeHexGrid.SAFE;

            _button.transform.DOLocalMoveY(0.005f, 0.15f).OnComplete(() =>
            {
                _button.transform.DOLocalMoveY(posNormal.y + 0.01f, 0.15f).OnComplete(() =>
                {
                    float Yaxis = _button.localRotation.eulerAngles.y;

                    DOTween.To(() => Yaxis, _ =>
                    {
                        _button.transform.localRotation = Quaternion.Euler(_button.localRotation.eulerAngles.x, _, _button.localRotation.eulerAngles.z);
                    }, Yaxis + 180, 0.65f)
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


            if (_hexController01)
            {
                if (_hexGridSwap02.TypeHexGrid != TypeHexGrid.STOP)
                    _hexGridSwap02.TypeHexGrid = TypeHexGrid.OBSTACLE;
            }
            if (_hexController02)
            {
                if (_hexGridSwap01.TypeHexGrid != TypeHexGrid.STOP)
                    _hexGridSwap01.TypeHexGrid = TypeHexGrid.OBSTACLE;
            }

            _button.transform.DOLocalMoveY(0.005f, 0.15f).OnComplete(() =>
            {
                if (_hexController01)
                {
                    if (_hexController01.IsDone)
                        _hexController01 = null;
                }
                if (_hexController02)
                {
                    if (_hexController02.IsDone)
                        _hexController02 = null;
                }
                if (_hexController01)
                    _hexController01.SetSwapping(true);
                if (_hexController02)
                    _hexController02.SetSwapping(true);

                if (!isSwapping)
                {
                    if (_hexController01)
                    {
                        if (!_hexController01.IsDone)
                        {
                            if (_hexController01)
                                _hexController01.SetParentNew(_point1);
                        }
                    }
                    if (_hexController02)
                    {
                        if (!_hexController02.IsDone)
                        {
                            if (_hexController02)
                                _hexController02.SetParentNew(_point2);
                        }
                    }

                }
                else
                {
                    if (_hexController01)
                    {
                        if (!_hexController01.IsDone)
                        {
                            if (_hexController01)
                                _hexController01.SetParentNew(_point2);
                        }
                    }
                    if (_hexController02)
                    {
                        if (!_hexController02.IsDone)
                        {
                            if (_hexController02)
                                _hexController02.SetParentNew(_point1);
                        }
                    }
                }

                _button.transform.DOLocalMoveY(posNormal.y + 0.01f, 0.15f).OnComplete(() =>
                {
                    float Yaxis = _button.localRotation.eulerAngles.y;
                    if (_hexController01)
                    {
                        _hexController01.IsSwapping = true;
                        if (_hexController01.HexCache.TypeHexGrid != TypeHexGrid.STOP)
                            _hexController01.HexCache.TypeHexGrid = TypeHexGrid.SAFE;
                    }
                    if (_hexController02)
                    {
                        _hexController02.IsSwapping = true;
                        if (_hexController02.HexCache.TypeHexGrid != TypeHexGrid.STOP)
                            _hexController02.HexCache.TypeHexGrid = TypeHexGrid.SAFE;
                    }
                    DOTween.To(() => Yaxis, _ =>
                    {
                        _button.transform.localRotation = Quaternion.Euler(_button.localRotation.eulerAngles.x, _, _button.localRotation.eulerAngles.z);
                    }, Yaxis + 180, 0.65f)
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
                                    if (_hexController02)
                                    {
                                        if (_hexController02.HexCache.TypeHexGrid != TypeHexGrid.STOP)
                                            _hexGridSwap02.TypeHexGrid = TypeHexGrid.OBSTACLE;
                                    }

                                    _hexController01.ResetScan();
                                }
                            }
                            if (_hexController02)
                            {
                                if (!_hexController02.IsDone)
                                {
                                    _hexController02.SetParentNew(_hexGridSwap01.ParentContainerHex);
                                    _hexController02.HexCache = _hexGridSwap01;
                                    if (_hexController01)
                                    {
                                        if (_hexController01.HexCache.TypeHexGrid != TypeHexGrid.STOP)
                                            _hexGridSwap01.TypeHexGrid = TypeHexGrid.OBSTACLE;
                                    }

                                    _hexController02.ResetScan();
                                }
                            }

                            ResetHexController();
                            _hexGridSwap01.CheckState();
                            _hexGridSwap02.CheckState();
                            isSwapped = false;
                        });
                    });
                });
            });
        }
    }
    private void ResetHexController()
    {
        if (_hexController01)
        {
            _hexController01.ResetPosY();
            _hexController01.IsSwapping = false;
            _hexController01.SetSwapping(false);
        }
        if (_hexController02)
        {
            _hexController02.ResetPosY();
            _hexController02.IsSwapping = false;
            _hexController02?.SetSwapping(false);
        }
    }
    public void OnChangeValueAngleOfTypeTwo()
    {
        switch (_typeSwapAxis)
        {
            case TypeSwapAxisTwo.NONE:
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
                break;
        }
        InitHex();
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
}
