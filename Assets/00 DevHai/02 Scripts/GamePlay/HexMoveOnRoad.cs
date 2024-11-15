using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
public class HexMoveOnRoad : MonoBehaviour
{
    public TypeDirHex TypeDirHex;
    public TypeColor TypeColor;
    [SerializeField] MeshRenderer[] _meshRenderer;
    [SerializeField] DataSOHex _dataSOHex;
    public HexGoalPending HexGoalPending;
    Tweener tweenRotate;
    public bool IsCanJump = false;
    public bool isJumping = false;
    public void InitHex()
    {
        DataSOHexModel data = _dataSOHex.GetDataSOHexModelOfType(TypeDirHex);
        for (int i = 0; i < _meshRenderer.Length; i++)
        {
            Debug.Log(TypeDirHex + "_" + TypeColor);

            _meshRenderer[i].material = data.DicsColorHex[TypeColor];


        }
    }

    public void MoveOnRoad(Vector3 pos)
    {
        transform.DOMove(pos, 50f)
            .SetSpeedBased(true)
            .SetEase(Ease.InSine)
            .OnComplete(() =>
                {
                    if (HexGoalPending == GameplayController.Ins.SpawnHexFillController.HexGoalPendingFirst)
                    {
                        IsCanJump = true;
                    }
                    SpawnHexFillController.A_AutoPutHexIntoBox?.Invoke();
                });
    }
    public void Rotate()
    {
        tweenRotate?.Kill(true);
        tweenRotate = transform.DORotate(new Vector3(-50, 0, -180), 0.25f)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                transform.DORotate(new Vector3(-50, 0, 0), 0.15f);
            });
    }
    public void RotateJum()
    {
        tweenRotate?.Kill(true);
        tweenRotate = transform.DORotate(new Vector3(-230, 180, -180), 0.25f)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                transform.DORotate(new Vector3(-50, 0, 30), 0);
            });
    }
    public void MoveToTargetForward(List<Vector3> ListPosMovetarget, BoxHex boxHex)
    {
        transform.DOPath(ListPosMovetarget.ToArray(), 12, PathType.CatmullRom, PathMode.Full3D)
                   .SetSpeedBased(true)
                   .SetEase(Ease.OutSine)
                   .OnComplete(() =>
                   {
                       boxHex.FillSlot();
                       transform.SetParent(boxHex.transform);
                       SpawnHexFillController.A_AutoPutHexIntoBox?.Invoke();
                   });
    }
    public void MoveToTargetForward(Transform target, HexGoalPending hex)
    {
        //  HexGoalPending = target.GetComponent<HexGoalPending>();
        transform.DOMove(target.position, 10f)
                   .SetSpeedBased(true)
                   .SetEase(Ease.InSine)
                   .OnComplete(() =>
                   {
                       if (hex == HexGoalPending)
                       {
                           IsCanJump = true;
                       }
                   });
    }
    private void OnDisable()
    {
        tweenRotate?.Kill();
    }
}
