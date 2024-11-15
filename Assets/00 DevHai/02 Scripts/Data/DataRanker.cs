using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
#endif
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Data Ranker", fileName = "Data Ranker")]
public class DataRanker : ScriptableObject
{
    [SerializeField] private List<RankerModel> _listRankModel = new();
    public List<RankerModel> ListRankerModel => _listRankModel;
#if UNITY_EDITOR
    [Button("LoadDataUserName")]
    public void LoadDataUserName()
    {
        string url = "https://docs.google.com/spreadsheets/d/e/2PACX-1vQ0WAfa3Ak7sJYfOMa38qf3s2ebI05ytoqwYxY4wp9Pa7tlB16TLCEjXuA_X9ihdaep0UoJD6Kztmoj/pub?gid=0&single=true&output=csv";
        System.Action<string> actionComplete = new System.Action<string>((string str) =>
        {
            try
            {
                _listRankModel = new();
                RankerModel rank1 = new();
                rank1.name = "me";
                rank1.Score = 0;
                _listRankModel.Add(rank1);
                PlayerPrefs.SetInt($"{rank1.name}_score", rank1.Score);

                var data = CSVReader.ReadCSV(str);
                for (int i = 2; i < 100; i++)
                {
                    RankerModel rank = new();
                    string name = data[i][0].ToLower();
                    rank.name = name;
                    if (i < 50)
                        rank.Score = UnityEngine.Random.Range(5, 10);
                    else
                        rank.Score = UnityEngine.Random.Range(10, 18);
                    _listRankModel.Add(rank);
                }
                UnityEditor.EditorUtility.SetDirty(this);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                UnityEditor.EditorUtility.DisplayDialog("Notice", "Load Data Fail roi ku", "OK");
            }
        });
        EditorCoroutineUtility.StartCoroutine(DGHelper.IELoadData(url, actionComplete, true), this);
    }
#endif
    public void Reload()
    {
        foreach (RankerModel item in _listRankModel)
        {
            PlayerPrefs.SetInt($"{item.name}_score", item.Score);
        }
    }
}
[System.Serializable]
public class RankerModel
{
    public int IndexRank;
    public int Score;
    public string name;

    public int GetScore()
    {
        return PlayerPrefs.GetInt($"{name}_score",0);
    }
}

