using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeInput
{
    NONE = -1,
    NORMAL,
    HAMMER,
    BOOM
}
public class InputController : MonoBehaviour
{
    [SerializeField] TypeInput _currentType = TypeInput.NORMAL;
    [SerializeField] LayerMask _layerHammer;
    [SerializeField] LayerMask _layerBoom;
    public static InputController Ins;
    public bool IsUsingHammer { get; set; }
    public bool IsUsingBoom { get; set; }
    private void Awake()
    {
        if (Ins == null)
            Ins = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (GameHelper.IsPointerOverUIObject()) return;
        if (Input.GetMouseButtonDown(0))
        {
            Clicked();
        }
    }
    private void Clicked()
    {
        if (!GameplayController.Ins.IsSetUpDone || GameplayController.Ins.StateGame != E_STATE_GAME.NONE) return;
        Action();
    }
    private void Action()
    {
        switch (_currentType)
        {
            case TypeInput.NONE:
                break;
            case TypeInput.NORMAL:
                OnStateNormal();
                break;
            case TypeInput.HAMMER:
                if (GameHelper.IsPointerOverUIObject()) return;
                OnStateHammer();
                break;
            case TypeInput.BOOM:
                if (GameHelper.IsPointerOverUIObject()) return;
                OnStateBoom();
                break;
            default: 
                break;
        }
    }
    private void OnStateNormal()
    {
        if (IsUsingBoom || IsUsingHammer) return;
        MMVibrationManager.Haptic(HapticTypes.RigidImpact, false, true, this);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, Mathf.Infinity))
        {
            HexElement hex = hit.collider.GetComponent<HexElement>();
            HexaSwapAxisController swap = hit.collider.GetComponent<HexaSwapAxisController>();
            if (hex)
            {
                if (hex.HexElementController.ActiveShel || hex.HexElementController.ActiveShelFroze
                    || !hex.HexElementController.IsCanSellect)
                    return;
                if (!hex.IsSwaping)
                {
                    hit.collider.GetComponent<HexElement>().HexElementController.IsCanSellect = false;
                    hit.collider.GetComponent<HexElement>().HexElementController.TimeCoolDownSellect = 0;
                    hit.collider.GetComponent<HexElement>().HexElementController.MoveForward();
                    if (GameplayController.Ins.HexGridController)
                        GameplayController.Ins.HexGridController.MinusMove();
                }
            }
            if (swap)
            {
                swap.ButtonDown();
            }
        }
    }
    private void OnStateHammer()
    {
        if (IsUsingHammer) return;
        MMVibrationManager.Haptic(HapticTypes.RigidImpact, false, true, this);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, Mathf.Infinity, _layerHammer))
        {
            HexElementController hex = hit.collider.GetComponent<HexElementController>();
            if (hex)
            {
                HexGridElement hexGrid = hex.GetComponentInParent<HexGridElement>();
                if (hexGrid)
                {
                    if (hexGrid.TypeHexGrid == TypeHexGrid.OBSTACLE
                        || hexGrid.TypeHexGrid == TypeHexGrid.ACTIVE_SHEL_FROZE
                        || hexGrid.TypeHexGrid == TypeHexGrid.ACTIVE_SHEL
                        || hexGrid.TypeHexGrid == TypeHexGrid.STOP)
                    {
                        hex.SpawnHammer();
                        IsUsingHammer = true;
                    }
                }
            }
        }
    }
    private void OnStateBoom()
    {
        if (IsUsingBoom) return;
        MMVibrationManager.Haptic(HapticTypes.RigidImpact, false, true, this);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, Mathf.Infinity, _layerBoom))
        {
            HexGridElement hex = hit.collider.GetComponentInParent<HexGridElement>();
            if (hex)
            {
                UIGameplayController.Ins.UIHintController.OnUseHint();
                if (hex.HexElementController == null || hex.HexElementController.IsDone)
                {
                    if (hex.TypeHexGrid != TypeHexGrid.UNLOCK_TURN_MOVE &&
                        hex.TypeHexGrid != TypeHexGrid.UNLOCK_ADS &&
                        hex.TypeHexGrid != TypeHexGrid.SWITCH_SWAP )
                    {
                        hex.SpawnBoom();
                        IsUsingBoom = true;
                    }
                }
               /* if (hex.TypeHexGrid == TypeHexGrid.SAFE || hex.TypeHexGrid == TypeHexGrid.STOP)
                {
                 
                }*/
            }
        }
    }
    public void SetTypeInput(TypeInput type)
    {
        _currentType = type;
    }
}
