using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
public class HexFlower : MonoBehaviour
{
    [SerializeField] private List<HexMoveOnRoad> HexesMoveOnRoad;
 
    int angle = 30;
    private void Start()
    {
        transform.localRotation = Quaternion.Euler(-50, 0, angle);
    }
    public void AddHexToList(HexMoveOnRoad hex)
    {
        if (!HexesMoveOnRoad.Contains(hex))
            HexesMoveOnRoad.Add(hex);
    }
    [Button]
    private void TestRotate()
    {
        angle -= 60;
        transform.DOLocalRotate(new Vector3(-50, 0, angle),0.3f);
    }
}
