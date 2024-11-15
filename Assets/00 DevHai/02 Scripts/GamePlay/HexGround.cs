using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGround : MonoBehaviour
{
    [SerializeField] HexGridElement _hexGridElement;
    public HexGridElement HexGridElement => _hexGridElement;
}
