using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "DataBoxHex",fileName = "DataBoxHex")]
public class DataBox : ScriptableObject
{
    [SerializeField] List<DataboxInfo> _listDataBoxInfo = new();



    public DataboxInfo GetDataBoxInfoOfTypeDir(TypeColor typedir)
    {
        foreach (var item in _listDataBoxInfo)
        {
            if (item.TypeColor == typedir)
                return item;
        }
        return null;
    }
}
[System.Serializable]
public class DataboxInfo
{
    public TypeColor TypeColor;
    public Material MaterialBox;
}
