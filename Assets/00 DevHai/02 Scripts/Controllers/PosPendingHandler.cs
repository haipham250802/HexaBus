using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PosPendingHandler : MonoBehaviour
{
    [SerializeField] PosHexStay[] _posHexStay;    
    public PosHexStay GetPosHexStay()
    {
        foreach (var item in _posHexStay)
        {
            if (!item.IsDone)
            {
                item.IsDone = true;
                return item;
            }
        }
        Debug.Log("da full cho");
        return null;
    }
}
