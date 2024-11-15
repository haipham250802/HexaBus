using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[CreateAssetMenu(menuName = "Data/Data Starter Pack",fileName = "Data Starter Pack")]
public class DataStarterPack : ScriptableObject
{
    [SerializeField] private int _quantityCoin;
    [SerializeField] private int _quantityBomb;
    [SerializeField] private int _quantityHammer;

    public int QuantityCoin => _quantityCoin;
    public int QuantityBomb => _quantityBomb;
    public int QuantityHammer => _quantityHammer;
}