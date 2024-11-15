using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public enum TypeSound
{
    NONE = -1,
    BG_LOBBY,
    BG_GAMEPLAY,
    CLICK_BUTTON,
    MOVE_PIECE,
    STOP_POINT,
    HAMMER,
    BOMB,
    CLEAR,
    WIN,
    LOSE,
    SWITCH_SWAP,
    HIT 
}
[CreateAssetMenu(menuName = "Data/Sound Data",fileName = "Sound Data")]
public class SoundData : ScriptableObject
{
    [Header("Sounds Element: ")]
    [SerializeField][HideLabel] List<SoundInfo> _listSoundInfo = new();
    public SoundInfo GetSoundInfoOfType(TypeSound typeSound)
    {
        foreach (SoundInfo item in _listSoundInfo)
        {
            if (item.TypeSound == typeSound)
                return item;
        }
        return null;
    }
}
[System.Serializable]
public class SoundInfo
{
    public TypeSound TypeSound;
    public AudioClip Sound;
    [Range(0f, 1f)]
    public float Volume = 1;
}