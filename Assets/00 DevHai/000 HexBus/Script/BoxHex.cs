using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class BoxHex : MonoBehaviour
{
    public TypeDirHex TypeDirHex;
    public TypeColor TypeColor;
    [SerializeField] DataBox _dataBox;
    [SerializeField] MeshRenderer[] _meshRenderer;
    [SerializeField] TextMeshPro _capacityTxt;
    [SerializeField] GameObject _lit;
    public PosHexStay PosHexStay;
    public Transform posBee;
    public Transform _posTopFall;
    public Transform PosHexInBox;


    public float Capacity = 3.0f;
    private float step = 0f;
    private float height = 1.25f;
    private float pieceHeight = 0.3f;

    private int _currentQuantityHex = 0;
    private int _quantitySlotEmpty = 0;

    void Start()
    {
        step = (float)(height - (Capacity * pieceHeight)) / (Capacity - 1); //1.1f la khoang cach tu min den max
    }
    public void InitCapacity(int capacity)
    {
        Capacity = capacity;
        _capacityTxt.text = capacity.ToString();
        _quantitySlotEmpty = capacity;
    }
    private void ZoomText()
    {
        _capacityTxt.gameObject.transform.DOScale(1.2f, 0.1f)
            .OnComplete(() =>
            {
                _capacityTxt.gameObject.transform.DOScale(1, 0.1f);
            });
    }
    public bool IsFull()
    {
        if (_currentQuantityHex == Capacity) return true;
        return false;
    }
    public void FillSlot()
    {
        _quantitySlotEmpty--;
        if (_quantitySlotEmpty > 0)
            _capacityTxt.text = _quantitySlotEmpty.ToString();
        else
        {
            _lit.SetActive(true);
           
            _capacityTxt.gameObject.SetActive(false);
            GameplayController.Ins.SpawnHexFillController.SpawnBee(posBee,this);
            GameplayController.Ins.SpawnHexFillController.RemoveBoxHexToList(this);
            PosHexStay.IsDone = false;
        }
        ZoomText();
    }
    public Transform GetPosHexInbox()
    {
        if (_currentQuantityHex == 0)
        {
            _currentQuantityHex++;
            return PosHexInBox;
        }
        else
        {
            _currentQuantityHex++;
            Vector3 thePos = PosHexInBox.localPosition;
            thePos.y = thePos.y + 0.3f + step;
            PosHexInBox.localPosition = thePos;
            return PosHexInBox;
        }
    }
    public void InitBox()
    {
        DataboxInfo dataBoxinfo = _dataBox.GetDataBoxInfoOfTypeDir(TypeColor);
        foreach (var item in _meshRenderer)
        {
            item.material = dataBoxinfo.MaterialBox;
        }
    }
}
