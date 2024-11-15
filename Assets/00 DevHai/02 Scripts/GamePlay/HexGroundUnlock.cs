using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using UnityEditor;
public class HexGroundUnlock : MonoBehaviour
{
    [SerializeField] HexUnlockAds _hexUnlockAds;
    [SerializeField] HexUnlockTurnMove _hexUnlockTurnMove;
    public HexUnlockAds HexUnlockAds => _hexUnlockAds;
    public HexUnlockTurnMove HexUnlockTurnMove => _hexUnlockTurnMove;
    public void SetUnlockAds(bool value)
    {
        _hexUnlockAds.gameObject.SetActive(value);
    }
    public void SetUnlockTurnMove(bool value)
    {
        _hexUnlockTurnMove.gameObject.SetActive(value);
    }
  
    public void SetUnlock(TypeHexGrid type)
    {
        _hexUnlockAds.gameObject.SetActive(false);
        _hexUnlockTurnMove.gameObject.SetActive(false);

        switch (type)
        {
            case TypeHexGrid.UNLOCK_TURN_MOVE:
                _hexUnlockTurnMove.gameObject.SetActive(true);
                break;
            case TypeHexGrid.UNLOCK_ADS:
                _hexUnlockAds.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
}
