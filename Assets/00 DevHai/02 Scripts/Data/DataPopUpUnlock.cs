using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[CreateAssetMenu(menuName = "Data/Data PopUp Unlock", fileName = "Data PopUp Unlock")]
public class DataPopUpUnlock : SerializedScriptableObject
{
    [SerializeField] Dictionary<TypePopUpUnlock, Sprite> _dataIconsPopUpUnlock;
    public Sprite GetIconPopUpUnlock(TypePopUpUnlock type)
    {
        return _dataIconsPopUpUnlock[type];
    }
}