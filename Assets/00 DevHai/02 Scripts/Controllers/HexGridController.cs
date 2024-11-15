using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEditor;

public class HexGridController : MonoBehaviour
{
    #region EVENT
    public static Action OnChangeValueHexaFall;
    #endregion
    [FoldoutGroup("Elements")] [SerializeField] HexGridElement _hexGridElementPrefab;
    [FoldoutGroup("Elements")] [SerializeField] HexElementController _containerHexPrefab;
    [FoldoutGroup("Elements")] [SerializeField] private List<HexGridElement> _listHexaClone = new();

    [FoldoutGroup("Configs Level")] [SerializeField] int gridWidth = 10;
    [FoldoutGroup("Configs Level")] [SerializeField] int gridHeight = 10;
    [FoldoutGroup("Configs Level")] [SerializeField] float hexWidth = 1.0f;
    [FoldoutGroup("Configs Level")] [SerializeField] float hexHeight = 1.0f;
    [FoldoutGroup("Configs Level")] [SerializeField] private AnimationCurve _curveOnStart;

    [FoldoutGroup("Configs Level")] [field: SerializeField] public int CountMove { get; set; }
    public List<HexGridElement> ListHexaClone => _listHexaClone;
    public AnimationCurve CurHexOnStart => _curveOnStart;
    private void Start()
    {
        var arr = FindObjectsOfType<HexElementController>();
        foreach (var item in arr)
        {
            if (item)
            {
                if (item.gameObject.activeInHierarchy)
                    GameplayController.Ins.NumebrHexCanWin++;
            }
        }
        foreach (var item in _listHexaClone)
        {
            if (item)
            {
                if (!item.gameObject.activeInHierarchy)
                    item.TypeHexGrid = TypeHexGrid.SAFE;
            }
        }
    }
    [Button("Creat Grid")]
    void CreateHexGrid()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        _listHexaClone = new();
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                SpawnHexGridElement(x, y);
            }
        }
        SetHexNeighBor();
        transform.localRotation = Quaternion.Euler(110, 0, 0);
        transform.localPosition = new Vector3(0, -5.5f, 8);
        ResizeCameraWithLevel();
        ScanAndSetdir();
        ForceResolve();
    }
    public void ResizeCameraWithLevel()
    {
        /*if (gridWidth <= 4)
        {
            Camera.main.fieldOfView = 16;
        }
        else if (gridWidth >= 5 && gridWidth <= 6)
        {
            Camera.main.fieldOfView = 18;
        }
        else if (gridWidth > 6 && gridWidth <= 7)
        {
            Camera.main.fieldOfView = 21;
        }
        else if (gridWidth >= 8)
        {
            Camera.main.fieldOfView = 26;
        }*/
    }
    [Button]
    void ClearHex()
    {
        foreach (var item in _listHexaClone)
        {
            if (item)
                DestroyImmediate(item.gameObject);
        }
    }
    private void ScanAndSetdir()
    {
        foreach (var item in _listHexaClone)
        {
            item.GetHexElementController().ScanHexAround();
        }
        CheckDir();
    }
    private void ForceResolve()
    {
        foreach (var item in ListHexaClone)
        {
            item.GetHexElementController().SetDirArrow();
        }
        foreach (var item in ListHexaClone)
        {
            item.SetHex();
        }
    }
    private void CheckDir()
    {
        foreach (var item in _listHexaClone)
        {
            item.GetHexElementController().Check();
        }
        foreach (var item in ListHexaClone)
        {
            item.GetHexElementController().CheckSymmetry();
        }
    }
    HexGridElement hexGridlone;

    private void SpawnHexGridElement(int x, int y)
    {
        Vector3 position = CalculateHexPosition(x, y);
#if UNITY_EDITOR
        hexGridlone = PrefabUtility.InstantiatePrefab(_hexGridElementPrefab) as HexGridElement;
#endif
        hexGridlone.gameObject.name = $"hexa_element_[{x},{y}]";
        hexGridlone.transform.position = position;
        hexGridlone.transform.rotation = Quaternion.Euler(-180, 0, 0);
        hexGridlone.transform.SetParent(transform);
        SpawnContainerHex(hexGridlone);
        _listHexaClone.Add(hexGridlone);

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
    HexElementController containerHexClone;
    private void SpawnContainerHex(HexGridElement hex)
    {
#if UNITY_EDITOR
        containerHexClone = PrefabUtility.InstantiatePrefab(_containerHexPrefab) as HexElementController;
#endif
        containerHexClone.transform.SetParent(hex.ParentContainerHex);
        containerHexClone.transform.localPosition = Vector3.zero;
        containerHexClone.transform.localRotation = Quaternion.identity;
        containerHexClone.transform.localScale = Vector3.one;
        hex.TransitionState(TypeHexGrid.OBSTACLE);
        containerHexClone.SetHexGrid(hex);
        containerHexClone.HexCache = hex;
        containerHexClone.SetDirGenerate();
        hex.HexElementController = containerHexClone;

    }
    Vector3 CalculateHexPosition(int x, int y)
    {
        float width = hexWidth * Mathf.Sqrt(3) / 2;
        float height = hexHeight * 0.75f;

        float xPos = x * width - (gridWidth - 1) * width / 2;
        float yPos = y * height - (gridHeight - 1) * height / 2;
        if (x % 2 == 1)
        {
            yPos += height / 2;
        }
        return new Vector3(xPos, yPos, 0);
    }
    private void SetHexNeighBor()
    {
        for (int i = 0; i < ListHexaClone.Count; i++)
        {
            for (int j = 0; j < ListHexaClone.Count; j++)
            {
                if (ListHexaClone[i].CheckHexNeighBor(ListHexaClone[j]))
                {
                    ListHexaClone[i].AddHextTolistNeghbor(ListHexaClone[j]);
                }
            }
        }
        foreach (var item in ListHexaClone)
        {
            item.SetDirToArray();
        }
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
    [Button]
    private void Force()
    {
        foreach (var item in _listHexaClone)
        {
            if(item)
            {
                if(item.HexElementController)
                {
                    item.TypeDirHex = item.HexElementController.TypeDirHex;
                    item.HexElementController.SetMaterial(item.TypeDirHex);
                }
            }
        }
    }
    public void MinusMove()
    {
        if (CountMove > 0)
        {
            CountMove--;
            UITopGameplay.A_OnChangeValueCountMove?.Invoke(CountMove);
            if (CountMove < 1 && GameplayController.Ins.StateGame == E_STATE_GAME.NONE)
            {
               // GameplayController.Ins.OnStateLose();
            }
        }
    }
}
