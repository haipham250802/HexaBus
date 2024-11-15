using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpRank : PopUpBase
{
    [SerializeField] Button _exitButton;
    [SerializeField] UIRankElement _uiRankElementPrefab;
    [SerializeField] UIRankUserElement _uiRankUserElement;
    [SerializeField] Transform _parent;
    private void Start()
    {
        InitButton();
        LoadRankElement();
    }
    private void InitButton()
    {
        _exitButton.onClick.AddListener(ExitClicked);
    }
    private void ExitClicked()
    {
        Hide();
    }
    private void LoadRankElement()
    {
        for (int i = 0; i < 50; i++)
        {
            UIRankElement uiRankElementClone = Instantiate(_uiRankElementPrefab);
            uiRankElementClone.transform.SetParent(_parent, false);
            uiRankElementClone.transform.localPosition = Vector3.zero;
            uiRankElementClone.transform.localScale = Vector3.one;
            uiRankElementClone.InitRank(
                GameController.Ins.RankController.ListRankInfo[i].IndexRank,
                GameController.Ins.RankController.ListRankInfo[i].Score,
                GameController.Ins.RankController.ListRankInfo[i].Name,
                i);
        }

        _uiRankUserElement.InitRankUser(GameController.Ins.RankController.GetRankUser().IndexRank,
            GameController.Ins.RankController.GetRankUser().Score,
            GameController.Ins.RankController.GetRankUser().Name);
    }
}

