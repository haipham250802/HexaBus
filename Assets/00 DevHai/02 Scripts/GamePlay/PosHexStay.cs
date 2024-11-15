using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosHexStay : MonoBehaviour
{
    [SerializeField] List<Transform> _posStay = new();
    [SerializeField] Transform _posBettween;
    public Transform PosBettwen => _posBettween;
    public bool IsDone;
    [SerializeField] BoxHex objHop;
    public List<GameObject> _listHexMakeBox = new();
    public Transform GetPos(int indexPos)
    {
        return _posStay[indexPos];
    }
    private void Start()
    {
        transform.localScale = Vector3.one;
    }
    public void ActiveBox(TypeDirHex typeDir,TypeColor typeColor,int Capacity)
    {
        //   objHop.gameObject.SetActive(true);
        GameObject hopClone = SimplePool.Spawn(objHop.gameObject, Vector3.zero, Quaternion.Euler(60, -83, -84));
        hopClone.transform.parent = transform;
        hopClone.transform.localRotation = Quaternion.Euler(90, -83, -84);
        hopClone.transform.localScale = Vector3.one * 0.65f;
        hopClone.transform.localPosition = new Vector3(0, 0f, 0.475f);
        BoxHex box = hopClone.GetComponent<BoxHex>();
        box.TypeDirHex = typeDir;
        box.TypeColor = typeColor;
        box.InitBox();
        box.InitCapacity(Capacity);
        box.PosHexStay = this;
        GameplayController.Ins.SpawnHexFillController.AddBoxHexToList(box);
        SpawnHexFillController.A_AutoPutHexIntoBox?.Invoke();
    }
    public void DeativeHex()
    {
        foreach (var item in _listHexMakeBox)
        {
            item.SetActive(false);
        }
    }
}
