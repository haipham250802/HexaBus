using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class SpawnHexFillController : MonoBehaviour
{
    public static System.Action A_AutoPutHexIntoBox;
    public HexMoveOnRoad objectPrefab;
    public List<HexGoalPending> points;
    List<Vector3> listConvertToPos = new();
    private List<HexMoveOnRoad> spawnedObjects = new();
    [SerializeField] List<BoxHex> _listHexBox = new();
    [SerializeField] BeeController _beePrefab;
    public int Count;
    int indexPos = 0;
    public int CapacityBoxHex;

    public List<DirInfo> _listTypeDirHex = new();
    public HexGoalPending HexGoalPendingFirst;
    private int CountListDirHex = 0;

    public bool ActionPending;
    public float RhymTime = 1;

    bool _isLose;
    bool _isWin;
    void Start()
    {
        foreach (var item in points)
        {
            listConvertToPos.Add(item.transform.position);
        }
        listConvertToPos.Reverse();
        SpawnStart();
        A_AutoPutHexIntoBox += AutoPutIntoBox;
    }
    public bool CheckWin()
    {
        if (!_isWin)
        {
            if (spawnedObjects.Count <= 0)
            {
                _isWin = true;
                return true;
            }
            else
                return false;

        }
        return false;

    }
    public bool CheckLose()
    {
        if (!_isLose)
        {
            if (_listHexBox.Count > CapacityBoxHex)
            {
                _isLose = true;
                return true;
            }
            else
                return false;
        }
        return false;
    }
    private void OnDisable()
    {
        A_AutoPutHexIntoBox -= AutoPutIntoBox;
    }
    public void AddBoxHexToList(BoxHex box)
    {
        if (!_listHexBox.Contains(box))
        {
            _listHexBox.Add(box);
            if (CheckLose())
            {
                GameplayController.Ins.OnStateLose();
            }
        }
    }
    public void SpawnBee(Transform posbee, BoxHex box)
    {
        //9, -110, -80
        GameObject bee = SimplePool.Spawn(_beePrefab.gameObject, Vector3.zero, Quaternion.Euler(4.8f, -90, -40));
        BeeController beeController = bee.GetComponent<BeeController>();
        beeController.transform.position = new Vector3(6.7f, -5, 0);
        beeController.Move(posbee, box);
    }
    public void RemoveBoxHexToList(BoxHex box)
    {
        if (_listHexBox.Contains(box))
        {
            _listHexBox.Remove(box);
        }
    }
    [Button]
    private void SpawnStart()
    {
        StartCoroutine(IE_delaySpwawn());
    }

    private DirInfo GetTypeDirHex()
    {
        if (CountListDirHex > 0)
        {
            DirInfo dir = _listTypeDirHex[CountListDirHex - 1];
            CountListDirHex--;
            return dir;
        }
        else
            return null;
    }
    IEnumerator IE_delaySpwawn()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        GameplayController.Ins.InitListTypeDirPending();
        yield return new WaitForSecondsRealtime(0.1f);
        CountListDirHex = _listTypeDirHex.Count;
        yield return new WaitForSecondsRealtime(0.3f);
        if (listConvertToPos.Count - Count <= 0) yield break;

        for (int i = 0; i < listConvertToPos.Count - Count; i++)
        {
            HexMoveOnRoad currentObject = Instantiate(objectPrefab);
            currentObject.transform.position = new Vector3(10.88f, -10.72f, -2.03f);
            DirInfo dir = GetTypeDirHex();
            currentObject.TypeDirHex = dir.TypeDirHex;
            currentObject.TypeColor = dir.TypeColor;
            currentObject.InitHex();
            spawnedObjects.Add(currentObject);
            if (indexPos < points.Count)
            {
                currentObject.HexGoalPending = points[indexPos];
                currentObject.MoveOnRoad(points[indexPos].transform.position);
            }
            currentObject.Rotate();
            indexPos++;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        Count = listConvertToPos.Count;
        indexPos = listConvertToPos.Count - 1;
    }
    IEnumerator IE_delayInvokeAction()
    {
        yield return new WaitForSecondsRealtime(0.3f * RhymTime);
        A_AutoPutHexIntoBox?.Invoke();
    }
    private void Spawn()
    {
        if (listConvertToPos.Count - Count <= 0) return;

        for (int i = 0; i < listConvertToPos.Count - Count; i++)
        {
            DirInfo dir = GetTypeDirHex();
          
            if (dir != null)
            {
                TypeDirHex type = dir.TypeDirHex;
                HexMoveOnRoad currentObject = Instantiate(objectPrefab);
                currentObject.transform.position = new Vector3(10.88f, -10.72f, -2.03f);
                currentObject.TypeDirHex = type;
                currentObject.TypeColor = dir.TypeColor;
                currentObject.InitHex();
                spawnedObjects.Add(currentObject);
                if (indexPos < points.Count)
                {
                    currentObject.HexGoalPending = points[indexPos];
                    currentObject.MoveOnRoad(points[indexPos].transform.position);
                }
                currentObject.Rotate();
                indexPos++;
            }
            else
            {
                StartCoroutine(IE_delayInvokeAction());
            }
        }
        Count = listConvertToPos.Count;
        indexPos = listConvertToPos.Count - 1;
    }
    [Button]
    List<Vector3> listPosMoveToBox = new();

    private void JumHexPendingIntoBox(BoxHex box)
    {
        if (spawnedObjects.Count == 0)
            return;
        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            if (spawnedObjects[i])
            {
                Transform targetPosition = (i == 0) ? box.GetPosHexInbox().transform : spawnedObjects[i - 1].HexGoalPending.transform;

                if (i == 0)
                {
                    listPosMoveToBox.Add(box._posTopFall.position);
                    listPosMoveToBox.Add(targetPosition.position);
                    spawnedObjects[i].MoveToTargetForward(listPosMoveToBox, box);
                    spawnedObjects[i].RotateJum();
                    spawnedObjects[i].isJumping = true;
                }
                else
                {
                    spawnedObjects[i].MoveToTargetForward(targetPosition, HexGoalPendingFirst);
                    spawnedObjects[i].Rotate();
                }
            }
        }
        listPosMoveToBox.Clear();

        spawnedObjects.RemoveAt(0);
        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            if (spawnedObjects[i])
            {
                spawnedObjects[i].HexGoalPending = points[i];
            }
        }
        Count--;
        Spawn();
    }
    bool isJumped = false;
    private void AutoPutIntoBox()
    {
        if (isJumped) return;
        if (spawnedObjects.Count <= 0) return;
        for (int i = 0; i < _listHexBox.Count; i++)
        {
            if (i < CapacityBoxHex)
            {
                if (!spawnedObjects[0].IsCanJump) return;
                if (_listHexBox[i].IsFull()) continue;

                if (spawnedObjects[0].TypeColor == _listHexBox[i].TypeColor)
                {
                    isJumped = true;
                    JumHexPendingIntoBox(_listHexBox[i]);
                    isJumped = false;
                    return;
                }
            }
        }
    }
}
[System.Serializable]
public class DirInfo
{
    public TypeColor TypeColor;
    public TypeDirHex TypeDirHex;
}
