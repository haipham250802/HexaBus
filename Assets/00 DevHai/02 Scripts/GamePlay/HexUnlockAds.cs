using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexUnlockAds : MonoBehaviour
{
    [SerializeField] HexGridElement _hexGridElement;
    [SerializeField] HexGroundUnlock _hexGroundUnlock;
    [SerializeField] LayerMask _layerMask;
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, Mathf.Infinity, _layerMask))
            {
                if (hit.collider != null)
                {
                    UnlockAds();
                }
            }
        }
    }
    private void UnlockAds()
    {
        UIGameplayController.Ins.UIPopUp.ShowPopUpUnlockHex(GiveUnlockAds,TypePopUpUnlock.UNLOCK_HEX);
    }
    private void GiveUnlockAds()
    {
        _hexGroundUnlock.gameObject.SetActive(false);
        _hexGridElement.TypeHexGrid = TypeHexGrid.SAFE;
    }
}
