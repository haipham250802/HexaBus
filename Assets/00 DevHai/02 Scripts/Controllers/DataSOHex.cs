using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[CreateAssetMenu(menuName = "Data/Data SO Hex", fileName = "Data SO Hex")]
public class DataSOHex : SerializedScriptableObject
{
    [SerializeField] private List<DataSOHexModel> _listData = new();
    public DataSOHexModel GetDataSOHexModelOfType(TypeDirHex type)
    {
        foreach (DataSOHexModel item in _listData)
        {
            if (item.TypeDirHex == type)
                return item;
        }
        return null;
    }
}
public class DataSOHexModel
{
    public TypeDirHex TypeDirHex;
    public Material M_Boder;
    public Material M_Glass;
    public Material M_Arrow;
    public Texture _Texture;

    public Dictionary<TypeColor, Material> DicsColorHex = new();
}
