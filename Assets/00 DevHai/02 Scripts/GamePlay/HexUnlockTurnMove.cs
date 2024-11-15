using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;

public class HexUnlockTurnMove : MonoBehaviour
{
    [SerializeField] HexGridElement _hexGridElemet;
    [SerializeField] HexGroundUnlock _hexGroundUnlock;
    [SerializeField] TextMeshPro _turnMoveTxt;
    [OnValueChanged("OnChangeValueQuantityTurnMove")]
    [SerializeField] int _quantityTurnMove;
    bool _unlocked = false;
    private void Start()
    {
        HexGridController.OnChangeValueHexaFall += MinusTurnMove;
    }
    public void OnChangeValueQuantityTurnMove()
    {
        _turnMoveTxt.text = _quantityTurnMove.ToString();
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
    public void SetQuantityTurnMove(int quantity)
    {
        _quantityTurnMove = quantity;
        OnChangeValueQuantityTurnMove();
    }
    public void MinusTurnMove()
    {
        if (_unlocked) return;
        _quantityTurnMove--;
        _turnMoveTxt.text = _quantityTurnMove.ToString();
        if (_quantityTurnMove <= 0) 
        {
            if (_hexGroundUnlock)
                _hexGroundUnlock.gameObject.SetActive(false);
            _hexGridElemet.TypeHexGrid = TypeHexGrid.SAFE;
            _unlocked = true;
            HexGridController.OnChangeValueHexaFall -= MinusTurnMove;
        }
    }
}