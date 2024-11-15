using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
public enum TypeColor
{
    NONE  = -1,
    ColorDown,
    ColorDown1,
    ColorDown2,

    ColorDownR1,
    ColorDownR2,
    ColorDownR3,

    ColorDownL1,
    ColorDownL2,
    ColorDownL3,
    
    ColorTop,
    ColorTop1,
    ColorTop2,

    colorTopR,
    colorTopR1,
    colorTopR2,

    ColorTopL,
    ColorTopL1,
    ColorTopL2,

}
public enum TypeDirHex
{
    NONE = -1,
    DOWN,
    TOP,
    TOP_LEFT,
    TOP_RIGHT,
    DOWN_LEFT,
    DOWN_RIGHT
}
public class HexElement : SerializedMonoBehaviour
{
    [Header("Fields")]
    [SerializeField] private TypeDirHex _typeDirHex;
    [SerializeField] private HexElementController _hexElementController;
    [SerializeField] private HexBroken _hexBrokenPrefabs;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private GameObject _hex;
    [SerializeField] Transform _posDirFake;
    [SerializeField] private bool _ignoreActiveArrow;
    [SerializeField] MeshRenderer _meshBorder;
    [SerializeField] MeshRenderer _meshGlass;
    [SerializeField] MeshRenderer _meshArrow;
    [SerializeField] Material _material;

    [Header("configs")]

    [SerializeField] private int _indexHex;
    [SerializeField] private float _duration;
    [SerializeField] private float _durationRotateAhead;
    [SerializeField] private float _dis;
    [SerializeField] private Vector3 _offSet;
    [SerializeField] Animator _animator;

    [Header("Condition")]
    [SerializeField] private int _indexPosStay;

    private int _countAhead = 0;
    private int _countBack = 0;

    private bool _isDone = false;
    private bool _isMove = false;

    private Tweener tweenRotateForward;
    private Tweener tweenRotateBackward;
    private Tweener tweenMoveBackward;
    private Tweener tweenMoveForward;
    private Tweener tweenFall;

    private Color _colorBorderCache;
    private Tweener _tweenColorOn;
    private Tweener _tweenColorEnd;

    private float timeCoolDown;
    public TypeDirHex TypeDirHex
    {
        get { return _typeDirHex; }
        set { _typeDirHex = value; }
    }
    public GameObject Arrow => _arrow;
    public bool IsSwaping { get; set; }
    public bool IsUp { get; set; }
    public Vector3 PosCacheStart { get; set; }
    public float PosZStart { get; set; }
    public HexElementController HexElementController => _hexElementController;
    public Transform PosDirFake => _posDirFake;
    private float timeCoolDownFall = 0;
    Tweener tweenMoveFo;
    Tweener tweenMoveFo1;
    private Sequence sequence;

    #region MONO
    private void Start()
    {
        PosCacheStart = transform.localPosition;
        PosZStart = transform.localPosition.z;
        timeCoolDown = _duration - 0.05f;
        _colorBorderCache = _meshBorder.material.color;
        UIHintController.A_OnUseHintBreak += AnimSuggestBreak;
        UIHintController.A_OnEndHintBreak += StopAnimSuggestBreak;
        SetDis();
        TypeDirHex = _hexElementController.TypeDirHex;
    }
    public void SetTexture(Texture texture)
    {
        _material.mainTexture = texture;
    }
    private void Update()
    {
        if (_isDone)
        {
            if (timeCoolDown > 0) timeCoolDown -= Time.deltaTime;
            else
            {
                if (isMoveTopDone)
                {
                    if (_rb.useGravity)
                        _rb.useGravity = false;
                    return;
                }
                _rb.useGravity = true;
                if (_rb.velocity.y < 0)
                {
                    HexaFallWithVelocity();
                }
            }
        }
    }
    private void SetDis()
    {
        switch (_hexElementController.TypeDirHex)
        {
            case TypeDirHex.NONE:
                break;
            case TypeDirHex.DOWN:
                _dis = 1.1f;
                break;
            case TypeDirHex.TOP:
                _dis = 1.1f;
                break;
            case TypeDirHex.TOP_LEFT:
                _dis = 1;
                break;
            case TypeDirHex.TOP_RIGHT:
                _dis = 1f;
                break;
            case TypeDirHex.DOWN_LEFT:
                _dis = .95f;
                break;
            case TypeDirHex.DOWN_RIGHT:
                _dis = .95f;
                break;
            default:
                break;
        }
    }
    #endregion
    [Button]
    public void SpawnFXBroken()
    {
        _hex.SetActive(false);
        _hexElementController.SetActiveCapacity(false);

        DestroyImmediate(_arrow);
        GetComponent<Collider>().enabled = false;
        RS();
        HexBroken hexBroken = Instantiate(_hexBrokenPrefabs);
        hexBroken.InitMaterial(_meshGlass.material, _meshBorder.material);
        hexBroken.transform.parent = transform;
        hexBroken.transform.localRotation = Quaternion.Euler(-60, -90, -90);
        hexBroken.transform.localPosition = Vector3.zero;
        hexBroken.transform.localScale = new Vector3(0.0075f, 0.0015f, 0.0075f);
    }
    #region MOVE FORWARD
    public void SetMaterial(Material mGlass, Material mBorder, Material arrow)
    {
        _meshBorder.material = mBorder;
        _meshGlass.material = mGlass;
        _meshArrow.material = arrow;
    }
    private void RotateAhead()
    {
        tweenRotateForward = DOTween.To(() => 0, _ =>
        {
            transform.localRotation = Quaternion.Euler(_, 0, 0);
        }, 180, _durationRotateAhead * 1.1f).SetEase(Ease.Linear);
    }

    public void RotateObstacleNear()
    {
        DOTween.To(() => 180, _ =>
        {
            transform.localRotation = Quaternion.Euler(_, 0, 0);
        }, 250, 0.15f).SetEase(Ease.Linear).OnComplete(() =>
        {
            DOTween.To(() => 250, _ =>
            {
                transform.localRotation = Quaternion.Euler(_, 0, 0);
            }, 180, 0.15f);
        });
        if (sequence != null)
            sequence.Kill(true);

        sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMove(new Vector3(0, -0.00022f, 0.00713f), 0.15f)
            .SetEase(Ease.Linear));
        sequence.Append(transform.DOLocalMove(Vector3.forward * 0.00221f, 0.15f)
              .SetEase(Ease.Linear)
              .OnComplete(() =>
              {
                  _hexElementController.ResetNearest();
                  ResetPosIdle();
                  _hexElementController.IsMoveForward = false;
              }));
    }

    private bool CheckObjectStopHasElement()
    {
        if (_hexElementController.ListHexMoveFowards[_countAhead].HexElementControllerStop)
        {
            if (_hexElementController.ListHexMoveFowards[_countAhead].HexElementControllerStop != _hexElementController)
                return true;
        }
        return false;
    }

    public void MoveForward()
    {
        if (_hexElementController.IsBoom) return;
        _hexElementController.CheckCurrentHexStop();
        if (_countAhead < _hexElementController.ListHexMoveFowards.Count)
        {
            if (_hexElementController.ListHexMoveFowards[_countAhead].TypeHexGrid == TypeHexGrid.STOP &&
             _hexElementController.ListHexMoveFowards[_countAhead].HexElementControllerStop == null)
            {
                if (_hexElementController.HexCache.TypeHexGrid != TypeHexGrid.STOP)
                    _hexElementController.HexCache.TypeHexGrid = TypeHexGrid.SAFE;
                _hexElementController.ListHexMoveFowards[_countAhead].HexElementControllerStop = _hexElementController;
            }
        }
        CheckOnStartMoveForward();
        #region OBSTACLE_NEAREST
        if (_hexElementController.IsShowAnimObstacleNear) return;
        if (_hexElementController.ListHexMoveFowards.Count > 0 && _countAhead < _hexElementController.ListHexMoveFowards.Count)
        {
            if (_hexElementController.ListHexMoveFowards[0].gameObject.activeInHierarchy)
            {
                if (IsCheckObstacle(0))
                {
                    if (!_hexElementController.IsShowAnimObstacleNear)
                    {
                        if (_hexElementController.ListHexMoveFowards[0].GetHexElementController())
                        {
                            if (IsCheckSpawnBrokenFroze(0))
                                _hexElementController.ListHexMoveFowards[0].GetHexElementController().SpawnFXBrokenFroze();
                        }
                        _hexElementController.IsShowAnimObstacleNear = true;
                        _hexElementController.DosomethingObstacleNearest();
                    }
                    return;
                }
            }
        }
        #endregion
        _hexElementController.SetIsDone();
        _hexElementController.ChangeStateHexGrid();
        _hexElementController.PlaySoundMove();
        RotateAhead();
        _hexElementController.DeactiveArrow();
        if (CheckFall())
        {
            if (!_hexElementController.HexCache) return;
            if (_hexElementController.HexCache.TypeHexGrid != TypeHexGrid.STOP)
            {
                if (_hexElementController.HexCache)
                    _hexElementController.HexCache.TypeHexGrid = TypeHexGrid.SAFE;
                else
                    _hexElementController.GetHexGrid().TypeHexGrid = TypeHexGrid.SAFE;
            }

            HexaFall();
            return;
        }
        if (_countAhead < _hexElementController.ListHexMoveFowards.Count)
        {
            Vector3 pos = _hexElementController.ListHexMoveFowards[_countAhead].PosMove.position/* + _offSet*/;
            tweenMoveForward = transform.DOMove(pos, _duration)
               .SetEase(Ease.Linear)
               .OnComplete(() =>
               {
                   if (_countAhead < _hexElementController.ListHexMoveBackWards.Count)
                   {
                       if (_hexElementController.ListHexMoveBackWards[_countAhead].TypeHexGrid == TypeHexGrid.BOOMB)
                           return;
                   }
                   if (_countAhead < _hexElementController.ListHexMoveFowards.Count)
                   {
                       if (_hexElementController.ListHexMoveFowards[_countAhead].HexElementControllerStop == _hexElementController)
                       {
                           if (_hexElementController.ListHexMoveFowards[_countAhead].TypeHexGrid == TypeHexGrid.STOP)
                           {
                               MMVibrationManager.Haptic(HapticTypes.SoftImpact, false, true, this);
                               GameController.Ins.SoundManager.PlaySoundElement(TypeSound.STOP_POINT);
                               if (_hexElementController.Sound)
                                   DestroyImmediate(_hexElementController.Sound.gameObject);
                               ResetPosWhenMoveForward();
                               _hexElementController.MoveDown();
                               _hexElementController.ResetStop(_duration * 0.8f, _hexElementController.ListHexMoveFowards[_countAhead].ParentContainerHex, _hexElementController.ListHexMoveFowards[_countAhead]);
                               _countAhead = 0;
                               _isMove = false;
                               return;
                           }
                       }
                   }

                   DosomethingMoveForwardComplete();

                   if (_countAhead < _hexElementController.ListHexMoveFowards.Count)
                   {

                       if (_hexElementController.ListHexMoveFowards[_countAhead].TypeHexGrid == TypeHexGrid.OBSTACLE)
                       {
                           if (!_hexElementController.IsObstacle)
                           {
                               _hexElementController.IsObstacle = true;
                               _hexElementController.IndexObstacle = _countAhead;
                           }
                       }

                       if (_hexElementController.IsObstacle && _countAhead == _hexElementController.IndexObstacle)
                       {
                           if (_hexElementController.ListHexMoveFowards[_hexElementController.IndexObstacle].GetHexElementController())
                           {
                               if (IsCheckSpawnBrokenFroze(_hexElementController.IndexObstacle))
                                   _hexElementController.ListHexMoveFowards[_hexElementController.IndexObstacle].GetHexElementController().SpawnFXBrokenFroze();
                           }
                           DoSomethingComeObstacle();
                           _isMove = false;
                           return;
                       }
                   }
                   else
                       _isMove = false;
                   if (_hexElementController.IndexObstacle > 0)
                   {
                       if (_countAhead >= _hexElementController.IndexObstacle)
                       {
                           return;
                       }
                   }
                   MoveForward();
               });
        }
    }
    private bool IsCheckSpawnBrokenFroze(int index)
    {
        if (!_hexElementController.ListHexMoveFowards[index].GetHexElementController().IsBrokenFroze
                        && _hexElementController.ListHexMoveFowards[index].GetHexElementController().ActiveShelFroze)
            return true;
        return false;
    }
    private bool IsCheckObstacle(int countAhead)
    {
        if (_hexElementController.ListHexMoveFowards[countAhead].HexElementController &&
           !_hexElementController.ListHexMoveFowards[countAhead].HexElementController.IsDone) return true;
        if (_hexElementController.ListHexMoveFowards[countAhead].TypeHexGrid == TypeHexGrid.OBSTACLE
                   || _hexElementController.ListHexMoveFowards[countAhead].TypeHexGrid == TypeHexGrid.SWITCH_SWAP
                   || _hexElementController.ListHexMoveFowards[countAhead].TypeHexGrid == TypeHexGrid.UNLOCK_ADS
                   || _hexElementController.ListHexMoveFowards[countAhead].TypeHexGrid == TypeHexGrid.UNLOCK_TURN_MOVE
                   || _hexElementController.ListHexMoveFowards[countAhead].TypeHexGrid == TypeHexGrid.ACTIVE_SHEL
                   || _hexElementController.ListHexMoveFowards[countAhead].TypeHexGrid == TypeHexGrid.ACTIVE_SHEL_FROZE
                   || CheckObjectStopHasElement())
            return true;
        return false;
    }
    private void ResetPosWhenMoveForward()
    {
        if (this == _hexElementController.HexTop)
        {
            Vector3 thePos = transform.localPosition;
            thePos.x = _hexElementController._PosCacheBottom.x;
            thePos.z = _hexElementController._PosCacheBottom.z;
            transform.localPosition = thePos;
            PosCacheStart = thePos;
        }
        else if (this == _hexElementController.HexBottom)
        {
            Vector3 thePos = transform.localPosition;
            thePos.x = _hexElementController._PosCacheTop.x;
            thePos.z = _hexElementController._PosCacheTop.z;
            transform.localPosition = thePos;
            PosCacheStart = thePos;
        }
        else
            ResetPosIdle();
        _hexElementController.ResetPos();
    }
    private void ResetPosWhenMoveBackward()
    {
        _countAhead = 0;
        ResetPosIdle();
    }
    private void CheckOnStartMoveForward()
    {
        if (!_isMove)
        {
            _isMove = true;
            _hexElementController.SetUpMoveForward();
        }
    }
    private void DosomethingMoveForwardComplete()
    {
        _countAhead++;
    }
    private void DoSomethingComeObstacle()
    {
        _countAhead--;
        _hexElementController.IndexMoveBackward = _countAhead;
        _hexElementController.MoveBackward();
        if (_hexElementController.Sound)
            DestroyImmediate(_hexElementController.Sound.gameObject);
        GameController.Ins.SoundManager.PlaySoundElement(TypeSound.HIT);
        /*   _hexElementController.HexElementTallest().*/
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.00117f);
    }
    #endregion
    #region MOVE BACKWARD
    private void RotateBack()
    {
        tweenRotateBackward = DOTween.To(() => 0, _ =>
        {
            transform.localRotation = Quaternion.Euler(_, 0, 0);
        }, -180, _duration).SetEase(Ease.Linear);
    }
    public void MoveBackward()
    {
        if (_hexElementController.IsBoom) return;
        tweenMoveForward?.Kill(true);
        Vector3 pos = _hexElementController.ListHexMoveBackWards[_countBack].transform.position + _offSet;
        if (_hexElementController.IsOverlap)
            pos += new Vector3(0, 0.9f, 0.3f);
        RotateBack();
        tweenMoveBackward = transform.DOMove(pos, _duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                /* if (!_ignoreActiveArrow)
                     _arrow.SetActive(true);*/
                if (_hexElementController.ListHexMoveBackWards[_countBack].TypeHexGrid == TypeHexGrid.STOP)
                {
                    ResetPosWhenMoveBackward();
                    return;
                }
                _countBack++;
                if (_countBack < _hexElementController.ListHexMoveBackWards.Count)
                    MoveBackward();
                else
                {
                    ResetPosWhenMoveBackward();
                }
            });
    }
    #endregion
    #region HEXA FALL
    public Vector3 GetPosVirtual()
    {
        Vector3 dir = Vector3.zero;
        if (_hexElementController.ListHexMoveFowards.Count <= 0)
            dir = _hexElementController.HexCache.GetEndPos(_hexElementController.TypeDirHex) - transform.position;
        else
            dir = _hexElementController.ListHexMoveFowards[_countAhead - 1].GetEndPos(_hexElementController.TypeDirHex) - transform.position;

        return transform.position + dir * _dis;
    }
    private bool CheckFall()
    {
        if (_countAhead >= _hexElementController.ListHexMoveFowards.Count ||
            !_hexElementController.ListHexMoveFowards[_countAhead].gameObject.activeInHierarchy)
        {
            return true;
        }
        return false;
    }
    private bool _isRotate = false;
    private void RotatingWhenMoveUp()
    {
        if (_isRotate) return;
        _isRotate = true;
        Debug.Log("Rotating");
        DOTween.To(() => 0f, _ =>
        {
            transform.localRotation = Quaternion.Euler(0, _, 0);
        }, 180f, 0.2f).SetEase(Ease.InOutSine);
    }
    private bool isMoveTopDone;
    private void MoveUpWhenMoveUp()
    {
        if (_isRotate) return;

        transform.parent = null;
        _isRotate = true;
        List<Vector3> lisPos = new();

        lisPos.Add(transform.position);
        if(_hexElementController.TypeMove == TypeMoveDir.NONE)
        {
            if (transform.position.x < 0)
            {
                _hexElementController.TypeMove = TypeMoveDir.LEFT;
                lisPos.Add(new Vector3(GameplayController.Ins._posLeftX, transform.position.y, transform.position.z));
                lisPos.Add(new Vector3(GameplayController.Ins._posLeftX, GameplayController.Ins._posYMoveUp.position.y, GameplayController.Ins._posYMoveUp.position.z));
            }
            else
            {
                _hexElementController.TypeMove = TypeMoveDir.RIGHT;

                lisPos.Add(new Vector3(GameplayController.Ins._posRightX, transform.position.y, transform.position.z));
                lisPos.Add(new Vector3(GameplayController.Ins._posRightX, GameplayController.Ins._posYMoveUp.position.y, GameplayController.Ins._posYMoveUp.position.z));
            }
        }
        else
        {
            switch (_hexElementController.TypeMove)
            {
                case TypeMoveDir.NONE:
                    break;
                case TypeMoveDir.LEFT:
                    lisPos.Add(new Vector3(GameplayController.Ins._posLeftX, transform.position.y, transform.position.z));
                    lisPos.Add(new Vector3(GameplayController.Ins._posLeftX, GameplayController.Ins._posYMoveUp.position.y, GameplayController.Ins._posYMoveUp.position.z));
                    break;
                case TypeMoveDir.RIGHT:
                    lisPos.Add(new Vector3(GameplayController.Ins._posRightX, transform.position.y, transform.position.z));
                    lisPos.Add(new Vector3(GameplayController.Ins._posRightX, GameplayController.Ins._posYMoveUp.position.y, GameplayController.Ins._posYMoveUp.position.z));
                    break;
                default:
                    break;
            }
        }
        _hexElementController.PosHexStay._listHexMakeBox.Add(gameObject);
        lisPos.Add(_hexElementController.PosHexStay.PosBettwen.position);
        transform.DORotate(new Vector3(180, 0, 0), 0.2f).SetEase(Ease.InOutSine).SetLoops(3, LoopType.Yoyo);
        transform.DORotate(new Vector3(310, 0, 0), 0.43f + _indexHex * 0.018f).SetEase(Ease.InOutSine).SetDelay(0.6f)
            .OnComplete(() =>
            {
                if (_indexHex == 3)
                {
                    _hexElementController.PosHexStay.ActiveBox(TypeDirHex, _hexElementController.TypeColor, _hexElementController.Capacity);
                }
                if (_indexHex == 4)
                {
                    _hexElementController.PosHexStay.DeativeHex();

                }
            });
        transform.DOPath(lisPos.ToArray(), .7f, PathType.CatmullRom, PathMode.Full3D)
             .SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
                transform.DOMove(_hexElementController.PosHexStay.GetPos(_indexPosStay).position, 0.2f)
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
                {
                   
                })
                .SetEase(Ease.OutSine);
                // isMoveTopDone = true;
            });
    }
    private void HexaFallWithVelocity()
    {
        if (!isMoveTopDone)
        {
            transform.position += Vector3.down * 3 * Time.deltaTime;
            _rb.velocity += Camera.main.transform.up * Physics.gravity.y * 3f * Time.deltaTime;
            if (_rb.velocity.y < -2.8f)
            {
                MoveUpWhenMoveUp();
                isMoveTopDone = true;
                _rb.constraints = RigidbodyConstraints.FreezePosition;
                return;
            }
            if (_rb.velocity.y < -5)
            {
                if (_hexElementController.Sound)
                    DestroyImmediate(_hexElementController.Sound.gameObject);
            }

            if (_rb.velocity.y < -50)
            {
                DestroyImmediate(_hexElementController.gameObject);
            }
        }

    }
    [SerializeField] private Vector3 offsetEnd;
    private void HexaFall()
    {
        //   _hexElementController.Hexes.localRotation = Quaternion.Euler(0, 0, 150);
        _hexElementController.InitPosStay();
        timeCoolDownFall = _hexElementController.TimeCoolDownFall.Pop();
        _isDone = true;
        RotateAhead();
        Vector3 posEnd = GetPosVirtual() + offsetEnd;
        tweenFall = transform.DOMove(posEnd, _duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                transform.localRotation = Quaternion.Euler(180, 0, 0);
            });
    }
    #endregion
    #region RESET
    public void ResetPosIdle()
    {
        Vector3 thePos = transform.localPosition;
        thePos.x = PosCacheStart.x;
        thePos.z = PosCacheStart.z;
        transform.localPosition = thePos;
        PosCacheStart = thePos;
        /* _hexElementController.SetActiveArrow();
         _hexElementController.IsMoveForward = false;
         _hexElementController.IsObstacle = false;
         _hexElementController.isPlaySoundMove = false;*/
        _hexElementController.ResetPos();
        if (!_hexElementController.ListHexElement.OrderByDescending(p => p.transform.localPosition.z).FirstOrDefault().Arrow.activeInHierarchy)
            _hexElementController.ListHexElement.OrderByDescending(p => p.transform.localPosition.z).FirstOrDefault().Arrow.SetActive(true);
        _hexElementController.IsDeactiveArrow = false;
        if (_indexHex == 4)
            _hexElementController.SetActiveCapacity(true);
    }
    public void rsBack()
    {
        _countBack = 0;
    }
    #endregion
    private void AnimSuggestBreak()
    {
        _meshBorder.material.color = GameController.Ins.GameConfig.ColorSuggestBreakMin;
        TransitionAnimSuggestBreak();
    }
    private void TransitionAnimSuggestBreak()
    {
        _tweenColorOn = _meshBorder.material.DOColor(GameController.Ins.GameConfig.ColorSuggestBreakMax, 0.85f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _tweenColorEnd = _meshBorder.material.DOColor(GameController.Ins.GameConfig.ColorSuggestBreakMin, 0.85f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    TransitionAnimSuggestBreak();
                });
            });
    }
    private void StopAnimSuggestBreak()
    {
        _tweenColorOn?.Kill();
        _tweenColorEnd?.Kill();
        _meshBorder.material.color = _colorBorderCache;
    }
    public void RS()
    {
        tweenRotateForward?.Kill();
        tweenRotateBackward?.Kill();
        tweenMoveBackward?.Kill();
        tweenMoveForward?.Kill();
        tweenFall?.Kill();

        UIHintController.A_OnUseHintBreak -= AnimSuggestBreak;
        UIHintController.A_OnEndHintBreak -= StopAnimSuggestBreak;
    }
    private void OnDisable()
    {
        RS();
    }
}
