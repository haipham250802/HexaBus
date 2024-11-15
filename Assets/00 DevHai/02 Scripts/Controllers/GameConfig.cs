using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : MonoBehaviour
{
    [SerializeField] Color _colorSuggestBreakMin;
    [SerializeField] Color _colorSuggestBreakMax;
    public Color ColorSuggestBreakMin => _colorSuggestBreakMin;
    public Color ColorSuggestBreakMax => _colorSuggestBreakMax;
}
