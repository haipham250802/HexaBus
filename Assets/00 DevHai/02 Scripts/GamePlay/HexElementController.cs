using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using DG.Tweening;
using System.Linq;
using TMPro;
public enum TypeMoveDir
{
    NONE = -1,
    LEFT,
    RIGHT
}
public class HexElementController : SerializedMonoBehaviour
{
    #region CONFIG VARIABLE
    public static float ANGLE_TOP = 90;
    public static float ANGLE_TOP_LEFT = 150;
    public static float ANGLE_TOP_RIGHT = 30;

    public static float ANGLE_DOWN = -90;
    public static float ANGLE_DOWN_LEFT = -150;
    public static float ANGLE_DOWN_RIGHT = -30;
    #endregion
    #region PRIVATE SERIALIZED
    [Header("TypeDirHex:")]
    [EnumToggleButtons, HideLabel, OnValueChanged("SetDirArrow")]
    [SerializeField] private TypeDirHex _typeDirHex;
    [FoldoutGroup("List")] [SerializeField] private List<HexElement> _listHexElement = new();
    [FoldoutGroup("List")] [SerializeField] List<HexGridElement> _listVisited = new();
    [FoldoutGroup("Fields")] [SerializeField] private HexGridElement _hexGrid;
    [FoldoutGroup("Fields")] [SerializeField] private Hammer _hammerPrefab;
    [Header("Data:")]
    [FoldoutGroup("Fields")] [SerializeField] private DataSOHex _dataSOHex;
    [Header("Feature Unlock:")]
    [FoldoutGroup("Fields")] [SerializeField] private GameObject _shel;
    [FoldoutGroup("Fields")] [SerializeField] private GameObject _shelFroze;
    [FoldoutGroup("Fields")] [SerializeField] private TextMeshPro _quantityUnlockShel;
    [OnValueChanged("OnChangeValueUnlockShel")]
    [FoldoutGroup("Fields")] [SerializeField] private int _quantityUnlock;
    [OnValueChanged("OnChangeActiveShel")]
    [FoldoutGroup("Fields")] [SerializeField] private bool _activeShel;
    [OnValueChanged("OnchangeActiveSelFroze")]
    [FoldoutGroup("Fields")] [SerializeField] private bool _activeShelFroze;
    [Header("configs Variable:")]
    [FoldoutGroup("Fields")] [SerializeField] float _timeFrame;
    [Header("Fields Other:")]
    [FoldoutGroup("Fields")] [SerializeField] private Transform _hexes;
    [FoldoutGroup("Fields")] [SerializeField] private HexBroken _hexBroken;
    [FoldoutGroup("Fields")] [SerializeField] private HexBroken _hexBrokenFroze;
    [FoldoutGroup("Fields")] [SerializeField] private TextMeshPro _capacityTxt;


    public TypeMoveDir TypeMove = TypeMoveDir.NONE;
    public TypeColor TypeColor;



    public Stack<float> TimeCoolDownFall = new();
    #endregion
    #region PRIVATE FIELDS
    private bool _isBrokenFroze = false;
    private bool _isBroken = false;
    private bool _isSpawnFXBroken = false;
    private bool _isMinus = false;
    private bool _isDestroy = false;
    private Quaternion _initializeRotate;
    private Vector3 PosCacheStart = Vector3.zero;
    private Coroutine _coForward;
    private Coroutine _coBackward;
    private Coroutine _coDestroy;
    private HexGridElement _hexGridCache;
    private List<HexGridElement> _listHexMoveForwards = new();
    private List<HexGridElement> _listHexMoveBackWards = new();
    public List<HexElement> ListHexElement => _listHexElement;
    #endregion
    #region PROPERTIES
    public Transform Hexes
    {
        get { return _hexes; }
        set { _hexes = value; }
    }
    public List<HexGridElement> ListHexMoveFowards => _listHexMoveForwards;
    public List<HexGridElement> ListHexMoveBackWards => _listHexMoveBackWards;
    public List<HexGridElement> ListVisited => _listVisited;
    public int Capacity { get; set; }
    public int IndexMoveBackward { get; set; }
    public bool IsMinus
    {
        get { return _isMinus; }
        set { _isMinus = value; }
    }
    public bool MoveBack { get; set; }
    public bool IsMoveForward { get; set; }
    public bool IsShowAnimObstacleNear { get; set; }
    public bool IsSwapping { get; set; }
    public bool IsCheckResetStop { get; set; }
    public bool IsBrokenFroze
    {
        get { return _isBrokenFroze; }
        set { _isBrokenFroze = value; }
    }
    public TypeDirHex TypeDirHex
    {
        get { return _typeDirHex; }
        set { _typeDirHex = value; }
    }

    [FoldoutGroup("Properties")]
    public HexGridElement HexCache
    {
        get { return _hexGridCache; }
        set { _hexGridCache = value; }
    }
    public bool ActiveShel
    {
        get { return _activeShel; }
        set { _activeShel = value; }
    }
    public bool ActiveShelFroze
    {
        get { return _activeShelFroze; }
        set { _activeShelFroze = value; }
    }
    public bool IsCanSellect = false;
    public HexElement _lastHex;
    public AudioSource Sound;
    public float TimeCoolDownSellect = 0;

    public bool IsHitObstacle = false;
    public bool IsDeactiveArrow { get; set; }
    public int IndexHitObstacle = 0;
    public bool IsBoom { get; set; }
    [FoldoutGroup("Properties")] [field: SerializeField] public HexElement HexTop;
    [FoldoutGroup("Properties")] [field: SerializeField] public HexElement HexBottom;
    [FoldoutGroup("Properties")] [field: SerializeField] public Vector3 _PosCacheTop;
    [FoldoutGroup("Properties")] [field: SerializeField] public Vector3 _PosCacheBottom;
    [FoldoutGroup("Properties")] [field: SerializeField] public bool IsOverlap { get; set; }

    #endregion
    public PosHexStay PosHexStay;
    private void Start()
    {
        ResetScan();
        _hexGridCache = transform.
            GetComponentInParent<HexGridElement>();
        PosCacheStart = transform.localPosition;
        _initializeRotate = _hexes.rotation;
        _PosCacheTop = HexTop.transform.localPosition;
        _PosCacheBottom = HexBottom.transform.localPosition;
        if (_activeShel)
        {
            OnChangeValueUnlockShel();
            HexGridController.OnChangeValueHexaFall += MinusQuantityActiveShel;
        }
        IsCanSellect = true;
        TimeCoolDownSellect = .5f;
        _typeDirHex = _hexGridCache.TypeDirHex;
        SetMaterial(_typeDirHex);
        SetAngleCapacityTxt();
        SetCapacity();

        Debug.Log("Chiều sâu: " + ListHexMoveFowards.Count);
     //   CheckCanExit();

    }
    private void SetCapacity()
    {
        int rand = Random.Range(2, 7);
        Capacity = rand;
        _capacityTxt.text = rand.ToString();
    }
    public void SetActiveCapacity(bool value)
    {
        _capacityTxt.gameObject.SetActive(value);
    }
    private void Update()
    {
        if (TimeCoolDownSellect < .5f && !IsCanSellect)
            TimeCoolDownSellect += Time.deltaTime;
        else
        {
            IsCanSellect = true;
            TimeCoolDownSellect = 0;
        }
    }
    public void SpawnFXBroken()
    {
        _quantityUnlockShel.gameObject.SetActive(false);
        _shel.SetActive(false);
        HexBroken hexBroken = Instantiate(_hexBroken);
        hexBroken.transform.parent = transform;
        hexBroken.transform.localRotation = Quaternion.Euler(-60, -90, -90);
        hexBroken.transform.localPosition = new Vector3(0, 0, 0.00087f);
        hexBroken.transform.localScale = new Vector3(0.003f, 0.003f, 0.003f);
    }
    public void SpawnFXBrokenFroze()
    {
        if (_isBrokenFroze) return;
        _isBrokenFroze = true;
        ActiveShelFroze = false;
        _shelFroze.SetActive(false);
        HexBroken hexBroken = Instantiate(_hexBrokenFroze);
        hexBroken.transform.parent = transform;
        hexBroken.transform.localRotation = Quaternion.Euler(-60, -90, -90);
        hexBroken.transform.localPosition = new Vector3(0, 0, 0.00087f);
        hexBroken.transform.localScale = new Vector3(0.003f, 0.003f, 0.003f);
    }
    #region ACTION
    public void RSActiveArrow()
    {
        if (IsDeactiveArrow)
            StartCoroutine(IE_delayRSDeactiveArrow());
    }
    IEnumerator IE_delayRSDeactiveArrow()
    {
        yield return new WaitForSecondsRealtime(_timeFrame * ListHexElement.Count - 1);
        IsDeactiveArrow = false;
    }
    private void MinusQuantityActiveShel()
    {
        if (!gameObject.activeInHierarchy)
            return;
        _quantityUnlock--;
        OnChangeValueUnlockShel();
        if (_quantityUnlock <= 0)
        {
            _activeShel = false;
            OnChangeActiveShel();
            HexGridController.OnChangeValueHexaFall -= MinusQuantityActiveShel;
        }
    }
    private bool _isRS = false;
    public void ResetPos()
    {
        if (_isRS) return;
        _isRS = true;
        StartCoroutine(IE_delayReset());
    }
    IEnumerator IE_delayReset()
    {
        yield return new WaitForSecondsRealtime(_timeFrame * 2);
        IsMoveForward = false;
        IsObstacle = false;
        isPlaySoundMove = false;
        _isRS = false;
        IndexObstacle = 0;
        IsChangeState = false;
    }
    public void OnchangeActiveSelFroze()
    {
        _shelFroze.SetActive(_activeShelFroze);
        _shel.SetActive(false);
        _quantityUnlockShel.gameObject.SetActive(false);
        if (HexTop.gameObject.activeInHierarchy && HexTop)
            if (HexTop.Arrow)
                HexTop.Arrow.SetActive(!_activeShel);
        _activeShel = false;
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
    public void OnChangeActiveShel()
    {
        _activeShelFroze = false;
        _shelFroze.SetActive(false);

        _shel.gameObject.SetActive(_activeShel);
        _quantityUnlockShel.gameObject.SetActive(_activeShel);
        if (HexTop.gameObject.activeInHierarchy && HexTop)
            if (HexTop.Arrow)
                HexTop.Arrow.SetActive(!_activeShel);
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
    private void OnChangeValueUnlockShel()
    {
        _quantityUnlockShel.text = _quantityUnlock.ToString();
    }
    public void SetQuantityUnlockShel(int quantity)
    {
        _activeShel = true;
        _quantityUnlock = quantity;
        OnChangeValueUnlockShel();
        OnChangeActiveShel();
    }
    #endregion
    private void LateUpdate()
    {
        if (IsSwapping)
            _hexes.rotation = _initializeRotate;
    }
    bool isMoveDown;
    public void MoveDown()
    {
        if (isMoveDown) return;
        isMoveDown = true;

        transform.DOLocalMoveZ(0, 0.2f);
    }
    public void SpawnHammer()
    {
        if (_isBroken) return;
        _isBroken = true;

        Hammer hammer = Instantiate(_hammerPrefab);
        hammer.transform.parent = transform;
        hammer.transform.localPosition = new Vector3(0.006451f, 0.00334f, 0.00874f);
        hammer.transform.localRotation = Quaternion.Euler(5.138f, 3.35f, -146.828f);
        hammer.transform.localScale = Vector3.one;
        hammer.ActionAnim(Ondisable);
    }
    public void ResetNearest()
    {
        StartCoroutine(IE_delayResetNearest());
    }
    IEnumerator IE_delayResetNearest()
    {
        yield return new WaitForSecondsRealtime(_timeFrame);
    }
    public void DosomethingObstacleNearest()
    {
        if (Sound)
            DestroyImmediate(Sound.gameObject);
        GameController.Ins.SoundManager.PlaySoundElement(TypeSound.HIT);
        HexElement hex = _listHexElement.OrderByDescending(p => p.transform.localPosition.z).FirstOrDefault();
        hex.RotateObstacleNear();
    }
    public void ResetPosY()
    {
        Vector3 thePos = transform.localPosition;
        thePos.z = PosCacheStart.z;
        transform.DOLocalMove(thePos, 0.15f);
    }
    public void SetParentNew(Transform parent)
    {
        transform.parent.GetComponentInParent<HexGridElement>().HexElementControllerStop = null;
        transform.parent.GetComponentInParent<HexGridElement>().HexElementController = null;
        transform.parent = null;
        transform.SetParent(parent);
        transform.parent.GetComponentInParent<HexGridElement>().HexElementController = this;
        if (transform.parent.GetComponentInParent<HexGridElement>().TypeHexGrid == TypeHexGrid.STOP)
            transform.parent.GetComponentInParent<HexGridElement>().HexElementControllerStop = this;
    }
    public void SetHexGrid(HexGridElement hexGrid)
    {
        _hexGrid = hexGrid;
    }
    public HexGridElement GetHexGrid()
    {
        return _hexGrid;
    }
    public void SetSwapping(bool value)
    {
        foreach (var item in _listHexElement)
        {
            item.IsSwaping = value;
        }
    }
    private void SetAngleCapacityTxt()
    {
        switch (_typeDirHex)
        {
            case TypeDirHex.DOWN:
                _capacityTxt.gameObject.transform.localRotation = Quaternion.Euler(0, -180, -180);
                break;
            case TypeDirHex.TOP:
                _capacityTxt.gameObject.transform.localRotation = Quaternion.Euler(0, -180, 0);
                break;
            case TypeDirHex.TOP_LEFT:
                _capacityTxt.gameObject.transform.localRotation = Quaternion.Euler(0, -180, 60);
                break;
            case TypeDirHex.TOP_RIGHT:
                _capacityTxt.gameObject.transform.localRotation = Quaternion.Euler(0, -180, -60);
                break;
            case TypeDirHex.DOWN_LEFT:
                _capacityTxt.gameObject.transform.localRotation = Quaternion.Euler(0, 180, -240);
                break;
            case TypeDirHex.DOWN_RIGHT:
                _capacityTxt.gameObject.transform.localRotation = Quaternion.Euler(0, -180, -120);
                break;
            default:
                break;
        }
    }
    public void SetDirArrow()
    {
        switch (_typeDirHex)
        {
            case TypeDirHex.DOWN:
                _hexes.localRotation = Quaternion.Euler(0, 0, 150);
                SetMaterial(TypeDirHex.DOWN);
                _capacityTxt.gameObject.transform.localRotation = Quaternion.Euler(0, -180, -180);
                break;
            case TypeDirHex.TOP:
                SetMaterial(TypeDirHex.TOP);
                _hexes.localRotation = Quaternion.Euler(0, 0, -30);
                _capacityTxt.gameObject.transform.localRotation = Quaternion.Euler(0, -180, 0);
                break;
            case TypeDirHex.TOP_LEFT:
                SetMaterial(TypeDirHex.TOP_LEFT);
                _hexes.localRotation = Quaternion.Euler(0, -180, 60);
                _capacityTxt.gameObject.transform.localRotation = Quaternion.Euler(0, -180, 60);
                break;
            case TypeDirHex.TOP_RIGHT:
                SetMaterial(TypeDirHex.TOP_RIGHT);
                _hexes.localRotation = Quaternion.Euler(0, 0, 30);
                _capacityTxt.gameObject.transform.localRotation = Quaternion.Euler(0, -180, -60);
                break;
            case TypeDirHex.DOWN_LEFT:
                SetMaterial(TypeDirHex.DOWN_LEFT);
                _hexes.localRotation = Quaternion.Euler(0, 0, 210);
                _capacityTxt.gameObject.transform.localRotation = Quaternion.Euler(0, 180, -240);
                break;
            case TypeDirHex.DOWN_RIGHT:
                SetMaterial(TypeDirHex.DOWN_RIGHT);
                _hexes.localRotation = Quaternion.Euler(0, 0, 90);
                _capacityTxt.gameObject.transform.localRotation = Quaternion.Euler(0, -180, -120);
                break;
            default:
                break;
        }
    }
    public Material MaterialHex;
    public void SetMaterial(TypeDirHex type)
    {
        int rand = Random.Range(0, 3);

        for (int i = 0; i < _listHexElement.Count; i++)
        {
            DataSOHexModel data = _dataSOHex.GetDataSOHexModelOfType(type);
            KeyValuePair<TypeColor, Material> item = new List<KeyValuePair<TypeColor, Material>>(data.DicsColorHex)[rand];
            TypeColor = item.Key;
            _listHexElement[i].SetMaterial(item.Value, item.Value, data.M_Arrow);
            _listHexElement[i].SetTexture(data._Texture);
            MaterialHex = item.Value;
        }
    }
    public bool CheckCanExit() // kiem tra xem co bi ket khong
    {
        _listVisited = new List<HexGridElement>();
        if (ListHexMoveFowards.Count <= 0)
        {
            return true;
        }
        HexGridElement currentArrow = ListHexMoveFowards[0];
        while (currentArrow != null && currentArrow != this && !_listVisited.Contains(currentArrow))
        {
            _listVisited.Add(currentArrow);
            if(currentArrow.GetHexElementController())
            {

                if (currentArrow.GetHexElementController().ListHexMoveFowards.Count > 0)
                    currentArrow = currentArrow.GetHexElementController().ListHexMoveFowards[0];
            }
        }
        if(_listVisited[_listVisited.Count - 1].GetHexElementController())
        {

            if (_listVisited[_listVisited.Count - 1].GetHexElementController().ListHexMoveFowards.Count <= 0)
                return true;
            else
                return false;
        }
        return false;
    }
    List<int> listType = new();
    int type;
    public void CheckSymmetry()
    {
        int indexType = (int)_typeDirHex;
        bool symmetryCheck = true;

        while (symmetryCheck)
        {
            symmetryCheck = CheckObjectAheadSymmetry();
            Debug.Log(symmetryCheck);
            if (!symmetryCheck)
                break;
            indexType++;
            if (indexType > 5)
                indexType = 0;
            _typeDirHex = (TypeDirHex)indexType;
        }
        SetDirArrow();
        _hexGridCache.TypeDirHex = _typeDirHex;
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
    public bool CheckObjectAheadSymmetry()
    {
        ResetScan();
        if (_listHexMoveForwards.Count == 0)
            return false;
        for (int i = 0; i < _listHexMoveForwards.Count; i++)
        {
            switch (_typeDirHex)
            {
                case TypeDirHex.NONE:
                    break;
                case TypeDirHex.DOWN:
                    if (_listHexMoveForwards[i].GetHexElementController().TypeDirHex == TypeDirHex.TOP)
                        return true;
                    break;
                case TypeDirHex.TOP:
                    if (_listHexMoveForwards[i].GetHexElementController().TypeDirHex == TypeDirHex.DOWN)
                        return true;
                    break;
                case TypeDirHex.TOP_LEFT:
                    if (_listHexMoveForwards[i].GetHexElementController().TypeDirHex == TypeDirHex.DOWN_RIGHT)
                        return true;
                    break;
                case TypeDirHex.TOP_RIGHT:
                    if (_listHexMoveForwards[i].GetHexElementController().TypeDirHex == TypeDirHex.DOWN_LEFT)
                        return true;
                    break;
                case TypeDirHex.DOWN_LEFT:
                    if (_listHexMoveForwards[i].GetHexElementController().TypeDirHex == TypeDirHex.TOP_RIGHT)
                        return true;
                    break;
                case TypeDirHex.DOWN_RIGHT:
                    if (_listHexMoveForwards[i].GetHexElementController().TypeDirHex == TypeDirHex.TOP_LEFT)
                        return true;
                    break;
                default:
                    break;
            }
        }
        return false;
    }
    public void Check()
    {
        listType = new();
        type = (int)_typeDirHex;
        CheckArrown();
    }
    private void CheckArrown()
    {
        if (CheckCanExit())
            return;
        type++;
        if (type > 5)
            type = 0;
        if (listType.Contains(type))
            return;
        listType.Add(type);
        _typeDirHex = (TypeDirHex)type;
        SetDirArrow();
        ResetScan();
        Check();
    }
    [Button]
    public void ResetScan()
    {
        _listHexMoveForwards = new();
        _hexGridCache = transform.
          GetComponentInParent<HexGridElement>();
        _hexGrid = _hexGridCache;
        ScanHexAround();
    }
    public void SetUpMoveForward()
    {
        _hexGrid = _hexGridCache;
        _listHexMoveForwards = new();
        ScanHexAround();
    }
    public void SetDirGenerate()
    {
        int indexDir = Random.Range(0, 6);
        _typeDirHex = (TypeDirHex)indexDir;
        SetDirArrow();
    }
    public void ScanHexAround()
    {
        if (_hexGrid)
        {
            if (_hexGrid.GetHexNeighborOfType(_typeDirHex))
            {
                if (!_listHexMoveForwards.Contains(_hexGrid.GetHexNeighborOfType(_typeDirHex)))
                {
                    _listHexMoveForwards.Add(_hexGrid.GetHexNeighborOfType(_typeDirHex));
                    _hexGrid = _hexGrid.GetHexNeighborOfType(_typeDirHex);
                    ScanHexAround();
                }
            }
        }

    }
    public int GetIndexObstacle()
    {
        for (int i = 0; i < _listHexMoveForwards.Count; i++)
        {
            if (_listHexMoveForwards[i].TypeHexGrid == TypeHexGrid.OBSTACLE)
                return i + 1;
        }
        return 0;
    }
    public bool IsObstacle = false;
    public int IndexObstacle = 0;
    public bool CheckObstacle(int currentIndexStay)
    {
        if (GetIndexObstacle() - 2 == currentIndexStay)
        {
            if (ListHexMoveFowards[GetIndexObstacle()].TypeHexGrid == TypeHexGrid.OBSTACLE)
            {
                return true;
            }
        }
        return false;
    }
    public void SetIsDone()
    {
        if (!IsDone)
        {
            if (CheckIsDone())
            {
                /* for (int i = 0; i < ListHexElement.Count; i++)
                 {
                     ListHexElement[i].Arrow.SetActive(false);
                 }*/
                IsDone = true;
                MinusMove();
            }
        }
    }
    public bool CheckIsDone()
    {
        foreach (var item in _listHexMoveForwards)
        {
            if (item.TypeHexGrid != TypeHexGrid.SAFE)
                return false;
        }
        return true;
    }
    private void DestroyGameObject()
    {
        if (_isDestroy)
        {
            if (_coDestroy != null)
                StopCoroutine(_coDestroy);
            return;
        }
        _isDestroy = true;
        _coDestroy = StartCoroutine(IE_delayDestroy());
    }
    IEnumerator IE_delayDestroy()
    {
        yield return new WaitForSecondsRealtime(2f);
        if (gameObject.activeInHierarchy)
            DestroyImmediate(gameObject);
    }
    public void Ondisable()
    {
        transform.SetParent(null);
        if (HexCache)
        {
            var hex = HexCache.gameObject.GetComponentsInChildren<HexElementController>();
            if (hex.Length < 1)
            {
                HexCache.TypeHexGrid = TypeHexGrid.SAFE;
            }
        }
        if (!_isSpawnFXBroken)
        {
            foreach (var item in _listHexElement)
            {
                item.SpawnFXBroken();
            }
            if (_activeShelFroze)
                SpawnFXBrokenFroze();
            if (_activeShel)
                SpawnFXBroken();
            _isSpawnFXBroken = true;
            MinusMove();
            DestroyGameObject();
        }
    }
    private bool _isSetPosStay = false;
    public void MinusMove()
    {
        if (!IsMinus)
        {
            IsMinus = true;
            HexGridController.OnChangeValueHexaFall?.Invoke();
            IsDone = true;
        }
    }
    public void InitPosStay()
    {
        if (!_isSetPosStay)
        {
            _isSetPosStay = true;
            PosHexStay = GameplayController.Ins.PosPendingHandler.GetPosHexStay();
        }
    }
    public void MoveForward()
    {

        if (IsMoveForward) return;

        if (!IsMoveForward)
            IsMoveForward = true;
        SetUpMoveForward();
        if (_coForward != null)
            StopCoroutine(_coForward);
        _coForward = StartCoroutine(IE_delayMoveForward());
    }
    public bool isPlaySoundMove { get; set; }
    public void PlaySoundMove()
    {
        if (isPlaySoundMove) return;
        isPlaySoundMove = true;
        Sound = GameController.Ins.SoundManager.PlaySoundElement(TypeSound.MOVE_PIECE);
    }
    public void MoveBackward()
    {
        if (MoveBack) return;
        if (!MoveBack)
            MoveBack = true;
        _listHexMoveBackWards = new();
        for (int i = IndexMoveBackward - 1; i >= 0; i--)
        {
            _listHexMoveBackWards.Add(_listHexMoveForwards[i]);
        }
        _listHexMoveBackWards.Add(_hexGridCache);
        if (_coBackward != null)
            StopCoroutine(_coBackward);
        _coBackward = StartCoroutine(IE_delayTestMoveBack());
    }
    public void CheckCurrentHexStop()
    {
        if (IsCheckResetStop) return;
        if (!_hexGridCache) return;
        if (_hexGridCache.TypeHexGrid == TypeHexGrid.STOP)
        {
            if (CheckStateSafeHexGrid())
            {
                if (_hexGridCache)
                    _hexGridCache.HexElementControllerStop = null;
                IsCheckResetStop = true;
            }
        }
    }
    public void ResetStop(float time, Transform parent, HexGridElement hexCache)
    {
        if (isRSStop) return;
        isRSStop = true;
        StartCoroutine(IE_delayActionStop(time, parent, hexCache));
    }
    bool isRSStop = false;
    IEnumerator IE_delayActionStop(float time, Transform parent, HexGridElement hexCache)
    {
        yield return new WaitForSecondsRealtime(time);
        SetParentNew(parent);
        ResetPosWhenStop();
        HexCache = hexCache;
        isRSStop = false;
    }
    public bool IsDone;
    public bool IsChangeState = false;
    public HexElement HexElementTallest()
    {
        return _listHexElement.OrderByDescending(_ => _.PosCacheStart.z).FirstOrDefault();
    }
    public void ChangeStateHexGrid()
    {
        if (!_hexGridCache) return;
        if (IsChangeState) return;
        IsChangeState = true;
        if (_hexGridCache.TypeHexGrid == TypeHexGrid.STOP)
        {
            return;
        }
        if (CheckStateSafeHexGrid() || CheckStateSafeHexStop())
        {
            if (_hexGridCache)
                _hexGridCache.TypeHexGrid = TypeHexGrid.SAFE;
            GetComponent<Collider>().enabled = false;
        }
    }
    private bool CheckStateSafeHexStop()
    {
        ResetScan();
        if (_listHexMoveForwards.Count > 1)
        {
            for (int i = 1; i < _listHexMoveForwards.Count; i++)
            {
                if (_listHexMoveForwards[i].TypeHexGrid == TypeHexGrid.OBSTACLE)
                    return false;
                if (_listHexMoveForwards[i].TypeHexGrid == TypeHexGrid.STOP)
                    return true;
            }
            return false;
        }
        return false;
    }
    public void DeactiveArrow()
    {
        if (IsDeactiveArrow) return;
        IsDeactiveArrow = true;
        for (int i = 0; i < ListHexElement.Count; i++)
        {
            ListHexElement[i].Arrow.SetActive(false);
        }
        SetActiveCapacity(false);
    }
    public void ResetPosWhenStop()
    {
        for (int i = 0; i < _listHexElement.Count; i++)
        {
            _listHexElement[i].transform.parent = null;
        }
        transform.localPosition = Vector3.zero;
        for (int i = 0; i < _listHexElement.Count; i++)
        {
            _listHexElement[i].transform.parent = _hexes;
        }
        isMoveDown = false;
    }
    private bool IsCanOut()
    {
        return CheckStateSafeHexGrid();
    }
    public bool CheckStateSafeHexGrid()
    {
        ResetScan();
        for (int i = 0; i < _listHexMoveForwards.Count; i++)
        {
            if (_listHexMoveForwards[i].TypeHexGrid != TypeHexGrid.SAFE)
                return false;
        }
        return true;
    }
    IEnumerator IE_delayMoveForward()
    {
        if (_listHexElement.Count > 0)
        {
            _listHexElement = _listHexElement.OrderByDescending(p => p.transform.localPosition.z).ToList();
            for (int i = 0; i < _listHexElement.Count; i++)
            {
                _listHexElement[i].MoveForward();
                yield return new WaitForSecondsRealtime(_timeFrame);
            }
            if (IsShowAnimObstacleNear)
                IsShowAnimObstacleNear = false;
        }
    }
    IEnumerator IE_delayTestMoveBack()
    {
        yield return new WaitForSecondsRealtime(_timeFrame * (_listHexElement.Count - 2));
        for (int i = _listHexElement.Count - 1; i >= 0; i--)
        {
            _listHexElement[i].rsBack();
            _listHexElement[i].MoveBackward();
            yield return new WaitForSecondsRealtime(_timeFrame);
        }
        for (int i = 0; i < _listHexElement.Count; i++)
        {


        }
        MoveBack = false;
    }
    private void OnDisable()
    {
        if (HexCache)
        {
            var hex = GetComponentsInChildren<HexElementController>();
            if (hex.Length < 1)
            {
                HexCache.TypeHexGrid = TypeHexGrid.SAFE;
            }
        }
        HexGridController.OnChangeValueHexaFall -= MinusQuantityActiveShel;
        if (_coDestroy != null)
            StopCoroutine(_coDestroy);
    }
}
