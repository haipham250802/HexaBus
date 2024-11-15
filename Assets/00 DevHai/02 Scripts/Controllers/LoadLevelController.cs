using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Sirenix.OdinInspector;
using UnityEditor;

public class LoadLevelController : MonoBehaviour
{
    [SerializeField] bool _isTestLevel;
    public bool IsTestLevel => _isTestLevel;
    public bool IsLoadedLevel { get; set; }
    public void LoadLevel()
    {
        ClearLevelOnHierachy();
        if (!GameController.Ins.LevelTest)
        {
            string pathNormalize = $"Level{CurrentLevel}.prefab";
            AsyncOperationHandle<GameObject> asyncOperationHandle =
                  Addressables.LoadAssetAsync<GameObject>(pathNormalize);
            asyncOperationHandle.Completed += AsyncOperationHandle_Completed;
        }
        else
        {
            Instantiate(GameController.Ins.LevelTest);
            IsLoadedLevel = true;
        }
    }
    private void AsyncOperationHandle_Completed(AsyncOperationHandle<GameObject> asyncOperationHandle)
    {
        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(asyncOperationHandle.Result);
            IsLoadedLevel = true;
        }
        else
            Debug.Log("Hinh nhu index level sai hoac chua keo vao addressable a oiii >< !");
    }
    public int CurrentLevel
    {
        get
        {
            return PlayerPrefs.GetInt("_current_level", 1);
        }
        set { PlayerPrefs.SetInt("_current_level", value); }
    }
#if UNITY_EDITOR
    [Button]
    private void LoadLevel(int level)
    {
        if (!_isTestLevel)
        {
            Debug.LogWarning("tick vao IsTestLevel nha anhhh !!!");
            return;
        }
        ClearLevelOnHierachy();
        EditorApplication.EnterPlaymode();
        GameplayController.Ins.HexGridController = FindObjectOfType<HexGridController>();
        CurrentLevel = level;
        LoadLevel();
    }
#endif
    private void ClearLevelOnHierachy()
    {
        LevelController[] levels = FindObjectsOfType<LevelController>();
        foreach (var item in levels)
        {
            DestroyImmediate(item.gameObject);
        }
    }
}
