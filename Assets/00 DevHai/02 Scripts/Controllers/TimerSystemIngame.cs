using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerSystemIngame : MonoBehaviour
{
    public static TimerSystemIngame Ins;
    [SerializeField] LoadLevelController _loadLevelController;
    public LoadLevelController LoadLevelController => _loadLevelController;
    private void Awake()
    {
        if (Ins == null)
            Ins = this;
        else
            DestroyImmediate(gameObject);
    }
    private void Start()
    {
        if (!_loadLevelController.IsTestLevel)
            _loadLevelController.LoadLevel();
        StartCoroutine(IE_loadingOnGame());
    }
    IEnumerator IE_loadingOnGame()
    {
        while (!_loadLevelController.IsLoadedLevel)
        {
            if (_loadLevelController.IsLoadedLevel)
                break;
            yield return null;
        }
        yield return null;
        GameplayController.Ins.OnStart();
        yield return new WaitForSecondsRealtime(.8f);
        UIGameplayController.Ins.OnStart();
    }
}
