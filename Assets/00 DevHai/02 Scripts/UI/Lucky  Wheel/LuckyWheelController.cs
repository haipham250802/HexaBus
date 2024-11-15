using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyWheelController : MonoBehaviour
{
    public int ProgressSpinWheel
    {
        get { return PlayerPrefs.GetInt("Progress_Spin_Wheel", 0); }
        set { PlayerPrefs.SetInt("Progress_Spin_Wheel", value); }
    }
}
