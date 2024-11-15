using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
public class BeeController : MonoBehaviour
{
    public void Move(Transform posBox, BoxHex box)
    {
        transform.DOMove(posBox.position + new Vector3(-0.3f, 0f, 0), 15f).SetSpeedBased(true).OnComplete(() =>
          {
              transform.DOMove(posBox.position + new Vector3(-0.3f, 0f,-0.1f), 0.1f).OnComplete(() =>
              {
                  box.transform.parent = transform;
                  if (GameplayController.Ins.SpawnHexFillController.CheckWin())
                  {
                      GameplayController.Ins.StateGame = E_STATE_GAME.WIN;
                      GameplayController.Ins.OnStateWin();
                  }
              });
              transform.DOMove(new Vector3(-10, 0, 0), 10f).SetSpeedBased(true).SetDelay(0.25f);
          });
    }
}
