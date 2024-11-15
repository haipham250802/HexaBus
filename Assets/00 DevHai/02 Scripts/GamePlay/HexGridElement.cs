using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using DG.Tweening;
public enum TypeHexGrid
{
    NONE = -1,
    SAFE,
    OBSTACLE,
    STOP,
    SWITCH_SWAP,
    BOOMB,
    UNLOCK_TURN_MOVE,
    UNLOCK_ADS,
    ACTIVE_SHEL,
    ACTIVE_SHEL_FROZE
}
public class HexGridElement : SerializedMonoBehaviour
{
    [Header("Type dir")]
    [OnValueChanged("OnChangeValueTypeDirHex")]
    [FoldoutGroup("Configs Hex")] [SerializeField] TypeDirHex _typeDirHex;
    [OnValueChanged("OnChangeValueTypeHex")]
    [Header("Type hex")]
    [FoldoutGroup("Configs Hex")] [SerializeField] TypeHexGrid _typeHexGrid;
    [OnValueChanged("OnValueChangeTypeAxis")]
    [FoldoutGroup("Configs Hex")] [SerializeField] [ShowIf("IsShowTypeAxis")] TypeAxis _typeAxis;

    [OnValueChanged("OnValueChangeAxisThree")]
    [FoldoutGroup("Configs Hex")] [SerializeField] [ShowIf("IsShowTypeAxisThree")] TypeSwapAxisThree _typeAxisThree;
    [OnValueChanged("OnValueChangeAxisTwo")]
    [FoldoutGroup("Configs Hex")] [SerializeField] [ShowIf("IsShowTypeAxisTwo")] TypeSwapAxisTwo _typeAxisTwo;
    [OnValueChanged("OnValueChangeStepRemaining")]
    [FoldoutGroup("Configs Hex")] [SerializeField] [ShowIf("IsShowActiveShel")] int _stepRemaning;
    [OnValueChanged("OnValueChangeStepUnlockMove")]
    [FoldoutGroup("Configs Hex")] [SerializeField] [ShowIf("IsShowUnlockMove")] int _stepUnlockMove;

    [FoldoutGroup("List")] [SerializeField] List<HexGridElement> _listHexNeighbor = new();
    [FoldoutGroup("List")] [SerializeField] List<HexGridInfo> _listHexInfo = new();
    [FoldoutGroup("Field ref")] [SerializeField] HexElementController _hexController;
    [FoldoutGroup("Field ref")] [SerializeField] HexGroundUnlock _hexGroundUnlock;
    [FoldoutGroup("Field ref")] [SerializeField] HexaSwapAxisController _hexaSwap;
    [FoldoutGroup("Field ref")] [SerializeField] HexUnlockTurnMove _hexUnlockTurnMove;

    [FoldoutGroup("Field ref")] [SerializeField] Transform _parentContainerHex;
    [FoldoutGroup("Field ref")] [SerializeField] Boom _boomPrefab;
    [FoldoutGroup("Field ref")] [SerializeField] GameObject _gStop;
    [FoldoutGroup("Field ref")] [SerializeField] GameObject _gBoom;
    [FoldoutGroup("Field ref")] [SerializeField] MeshRenderer _meshRenderer;
    [FoldoutGroup("Field ref")] [SerializeField] Material _mStop;
    [FoldoutGroup("Field ref")] [SerializeField] Material _mBG;

    [FoldoutGroup("Field ref")] [SerializeField] Transform[] _endPoses;
    public Transform PosMove;
    private Stack<Transform> _listTransformOverlap = new();
    private Vector3 _posCache;
    private Vector3 _scaleCache;
    public int StepRemaining
    {
        get { return _stepRemaning; }
        set { _stepRemaning = value; }
    }
    public List<HexGridElement> ListHexNeighBor
    {
        get { return _listHexNeighbor; }
        set { _listHexNeighbor = value; }
    }
    public HexElementController HexElementControllerStop { get; set; }
    public HexElementController HexElementController
    {
        get { return _hexController; }
        set { _hexController = value; }
    }
    public Transform ParentContainerHex => _parentContainerHex;
    public HexElementController HexElementControllerCache;
    public TypeHexGrid TypeHexGrid
    {
        get { return _typeHexGrid; }
        set { _typeHexGrid = value; }
    } public TypeDirHex TypeDirHex
    {
        get { return _typeDirHex; }
        set { _typeDirHex = value; }
    }
    public TypeHexGrid TypeHexGridCache { get; set; }
    private void Awake()
    {
        _posCache = transform.localPosition;
        _scaleCache = transform.localScale;
        transform.localPosition -= new Vector3(0, -.5f, -.5f);
        transform.localScale *= 0.001f;
    }
    private void OnChangeValueTypeDirHex()
    {
        if (_hexController)
        {
            _hexController.TypeDirHex = _typeDirHex;
            _hexController.SetDirArrow();
        }

    }
    private void InstantiateHexControllerCache()
    {
        if (HexElementController) return;
#if UNITY_EDITOR
        _hexController = PrefabUtility.InstantiatePrefab(HexElementControllerCache) as HexElementController;
#endif
        _hexController.transform.SetParent(ParentContainerHex);
        _hexController.transform.localPosition = Vector3.zero;
        _hexController.transform.localRotation = Quaternion.identity;
        _hexController.transform.localScale = Vector3.one;
        TransitionState(TypeHexGrid.OBSTACLE);
        _hexController.SetHexGrid(this);
        _hexController.HexCache = this;
        _hexController.SetDirGenerate();
        HexElementController = _hexController;

    }
    public Vector3 GetEndPos(TypeDirHex typeDirHex)
    {
        switch (typeDirHex)
        {
            case TypeDirHex.NONE:
                break;
            case TypeDirHex.DOWN:
                return _endPoses[3].position;
            case TypeDirHex.TOP:
                return _endPoses[0].position;
            case TypeDirHex.TOP_LEFT:
                return _endPoses[5].position;
            case TypeDirHex.TOP_RIGHT:
                return _endPoses[1].position;
            case TypeDirHex.DOWN_LEFT:
                return _endPoses[4].position;
            case TypeDirHex.DOWN_RIGHT:
                return _endPoses[2].position;
            default:
                break;
        }
        return Vector3.zero;
    }
    public void ActionAnim()
    {
        float duration = UnityEngine.Random.Range(.8f, 1.2f);
        transform.DOLocalMove(_posCache, duration).SetEase(GameplayController.Ins.HexGridController.CurHexOnStart);
        transform.DOScale(_scaleCache, duration).SetEase(GameplayController.Ins.HexGridController.CurHexOnStart);
    }
    private void Start()
    {
        if (_hexController)
        {
            if (!_hexController.gameObject.activeInHierarchy &&
           _typeHexGrid != TypeHexGrid.STOP && TypeHexGrid != TypeHexGrid.UNLOCK_TURN_MOVE
           && _typeHexGrid != TypeHexGrid.UNLOCK_ADS)
            {
                if (_typeHexGrid == TypeHexGrid.SWITCH_SWAP)
                    return;
                _typeHexGrid = TypeHexGrid.SAFE;
            }
        }

        TypeHexGridCache = TypeHexGrid;
    }
    private bool IsShowTypeAxis()
    {
        return TypeHexGrid == TypeHexGrid.SWITCH_SWAP;
    }
    private bool IsShowTypeAxisTwo()
    {
        return TypeHexGrid == TypeHexGrid.SWITCH_SWAP && _typeAxis == TypeAxis.TWO;
    }
    private bool IsShowTypeAxisThree()
    {
        return TypeHexGrid == TypeHexGrid.SWITCH_SWAP && _typeAxis == TypeAxis.THREE;
    }
    private void OnValueChangeTypeAxis()
    {
        _hexaSwap.TypeAxis = _typeAxis;
        _hexaSwap.OnChangeValueAxis();

    }
    private void OnValueChangeAxisTwo()
    {
        _hexaSwap.HexaSwapTwo.TypeSwapAxisTwo = _typeAxisTwo;
        _hexaSwap.HexaSwapTwo.OnChangeValueAngleOfTypeTwo();
    }
    private void OnValueChangeAxisThree()
    {
        _hexaSwap.HexaSwapThree.TypeSwapAxisThree = _typeAxisThree;
        _hexaSwap.HexaSwapThree.OnChangeValueAngleOfTypeThree();
    }
    private bool IsShowActiveShel()
    {
        return _typeHexGrid == TypeHexGrid.ACTIVE_SHEL;
    }
    private void OnValueChangeStepRemaining()
    {
        _hexController.SetQuantityUnlockShel(_stepRemaning);
    }
    private void OnValueChangeTypeActiveShelFroze()
    {

    }
    private bool IsShowUnlockMove()
    {
        return _typeHexGrid == TypeHexGrid.UNLOCK_TURN_MOVE;
    }
    private void OnValueChangeStepUnlockMove()
    {
        _hexUnlockTurnMove.SetQuantityTurnMove(_stepUnlockMove);
    }
    private bool IsShowActiveShelFroze()
    {
        return _typeHexGrid == TypeHexGrid.ACTIVE_SHEL_FROZE;
    }
    [Button("Spawn Hex Overlap")]
    private void SpawnHexContrllerElement()
    {
        Transform transHex = Instantiate(_hexController.transform);
        transHex.SetParent(_parentContainerHex);
        transHex.localScale = Vector3.one;
        transHex.localRotation = Quaternion.identity;
        transHex.localPosition = Vector3.zero;
        transHex.localPosition += new Vector3(0, 0, 0.0024f * (_listTransformOverlap.Count + 1));
        _listTransformOverlap.Push(transHex);
    }
    [Button("Despawn Hex Overlap")]
    private void DespawnHexControllerElement()
    {
        Transform trans = _listTransformOverlap.Pop();
        DestroyImmediate(trans.gameObject);
    }
    public void CheckState()
    {
        if (_hexController)
        {
            if (TypeHexGrid != TypeHexGrid.STOP)
                TypeHexGrid = TypeHexGrid.OBSTACLE;
            return;
        }

        if (TypeHexGrid == TypeHexGrid.OBSTACLE)
        {
            if (_hexController == null || _hexController.IsDone)
                TypeHexGrid = TypeHexGrid.SAFE;
        }

    }
    public void SpawnBoom()
    {
        Boom boom = Instantiate(_boomPrefab);
        boom.transform.SetParent(_parentContainerHex);
        boom.transform.localPosition = Vector3.zero;
        boom.transform.localRotation = Quaternion.Euler(Vector3.zero);
        boom.transform.localScale = Vector3.one;
        boom.ActionBoom(this);
    }
    public void CheckStateObstacle()
    {
        if (TypeHexGrid == TypeHexGrid.OBSTACLE)
        {
            if (_hexController == null)
            {
                TypeHexGrid = TypeHexGrid.SAFE;
            }
        }
    }
    private void OnChangeValueTypeHex()
    {
        _gBoom.SetActive(false);
        _gStop.gameObject.SetActive(false);
        _hexaSwap.gameObject.SetActive(false);
        _parentContainerHex.gameObject.SetActive(true);
        _hexGroundUnlock.gameObject.SetActive(false);

        if (_hexController)
        {
            _hexController.ActiveShel = false;
            _hexController.ActiveShelFroze = false;
            _hexController.OnchangeActiveSelFroze();
            _hexController.OnChangeActiveShel();
        }

        _meshRenderer.material = _mBG;
        switch (_typeHexGrid)
        {
            case TypeHexGrid.NONE:
                break;
            case TypeHexGrid.SAFE:
                if (_hexController)
                    DestroyImmediate(_hexController.gameObject);
                _meshRenderer.material = _mBG;
                _gStop.gameObject.SetActive(false);

                break;
            case TypeHexGrid.OBSTACLE:
                InstantiateHexControllerCache();
                _hexController.gameObject?.SetActive(true);
                break;
            case TypeHexGrid.STOP:
                if (_hexController)
                    DestroyImmediate(_hexController.gameObject);
                _gStop.gameObject.SetActive(true);
                _meshRenderer.material = _mStop;
                break;
            case TypeHexGrid.SWITCH_SWAP:
                if (_hexController)
                    DestroyImmediate(_hexController.gameObject);
                _hexaSwap.gameObject.SetActive(true);
                break;
            case TypeHexGrid.BOOMB:
                if (_hexController)
                    DestroyImmediate(_hexController.gameObject);
                _gBoom.SetActive(true);
                break;
            case TypeHexGrid.UNLOCK_TURN_MOVE:
                if (_hexController)
                    DestroyImmediate(_hexController.gameObject);
                _hexGroundUnlock.gameObject.SetActive(true);
                _hexGroundUnlock.SetUnlock(TypeHexGrid.UNLOCK_TURN_MOVE);
                break;
            case TypeHexGrid.UNLOCK_ADS:
                if (_hexController)
                    DestroyImmediate(_hexController.gameObject);
                _hexGroundUnlock.gameObject.SetActive(true);
                _hexGroundUnlock.SetUnlock(TypeHexGrid.UNLOCK_ADS);
                break;
            case TypeHexGrid.ACTIVE_SHEL:
                InstantiateHexControllerCache();
                if (_hexController)
                    _hexController.ActiveShel = true;
                _hexController.gameObject?.SetActive(true);
                break;
            case TypeHexGrid.ACTIVE_SHEL_FROZE:
                InstantiateHexControllerCache();
                _hexController.gameObject?.SetActive(true);
                if (_hexController)
                    _hexController.ActiveShelFroze = true;
                _hexController?.OnchangeActiveSelFroze();
                break;
            default:
                break;
        }
    }

    public void SetHex()
    {
        _hexController = _parentContainerHex.GetComponentInChildren<HexElementController>();
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
    public HexElementController GetHexElementController()
    {
        return _parentContainerHex.GetComponentInChildren<HexElementController>();
    }
    public HexElementController GetHexElementControllerSwap()
    {
        HexElementController[] hexes = _parentContainerHex.GetComponentsInChildren<HexElementController>();
        foreach (HexElementController item in hexes)
        {
            if (!item.IsDone)
                return item;
        }
        return null;
    }
 
    public bool CheckHexNeighBor(HexGridElement hex)
    {
        float dis = Vector2.Distance(transform.position, hex.transform.position);
        if (dis < 1.2f)
        {
            return true;
        }
        return false;
    }
    public void AddHextTolistNeghbor(HexGridElement hex)
    {
        if (!_listHexNeighbor.Contains(hex) && hex != this)
            _listHexNeighbor.Add(hex);
    }
    public void SetDirToArray()
    {
        // index 0 =>  DOWN
        // index 1 =>  TOP
        // index 2 =>  TOP_LEFT
        // index 3 =>  TOP_RIGHT
        // index 4 =>  DOWN_LEFT
        // index 5 =>  DOWN_RIGHT
        TypeDirHex typeDirHex = TypeDirHex.NONE;
        _listHexInfo = new();

        for (int i = 0; i < _listHexNeighbor.Count; i++)
        {
            HexGridInfo HexInfo = new();

            if (CalAngle(gameObject, _listHexNeighbor[i].gameObject) > -92 && CalAngle(gameObject, _listHexNeighbor[i].gameObject) < -88)
                typeDirHex = TypeDirHex.DOWN;
            if (CalAngle(gameObject, _listHexNeighbor[i].gameObject) > 88 && CalAngle(gameObject, _listHexNeighbor[i].gameObject) < 92)
                typeDirHex = TypeDirHex.TOP;
            if (CalAngle(gameObject, _listHexNeighbor[i].gameObject) > 148 && CalAngle(gameObject, _listHexNeighbor[i].gameObject) < 152)
                typeDirHex = TypeDirHex.TOP_LEFT;
            if (CalAngle(gameObject, _listHexNeighbor[i].gameObject) > 28 && CalAngle(gameObject, _listHexNeighbor[i].gameObject) < 32)
                typeDirHex = TypeDirHex.TOP_RIGHT;
            if (CalAngle(gameObject, _listHexNeighbor[i].gameObject) > -152 && CalAngle(gameObject, _listHexNeighbor[i].gameObject) < -148)
                typeDirHex = TypeDirHex.DOWN_LEFT;
            if (CalAngle(gameObject, _listHexNeighbor[i].gameObject) > -32 && CalAngle(gameObject, _listHexNeighbor[i].gameObject) < -28)
                typeDirHex = TypeDirHex.DOWN_RIGHT;
            Debug.Log(typeDirHex);
            HexInfo.TypeDir = typeDirHex;
            HexInfo.HexGrid = _listHexNeighbor[i];
            _listHexInfo.Add(HexInfo);
        }
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
    public HexGridElement GetHexNeighborOfType(TypeDirHex type)
    {
        foreach (var item in _listHexInfo)
        {
            if (item.TypeDir == type)
                return item.HexGrid;
        }
        return null;
    }
    public void TransitionState(TypeHexGrid type)
    {
        _typeHexGrid = type;
    }
    private float CalAngle(GameObject obj1, GameObject obj2)
    {
        Vector3 dir = obj2.transform.position - obj1.transform.position;
        double angle = Math.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return (float)angle;
    }
}
[System.Serializable]
public class HexGridInfo
{
    public TypeDirHex TypeDir;
    public HexGridElement HexGrid;
}