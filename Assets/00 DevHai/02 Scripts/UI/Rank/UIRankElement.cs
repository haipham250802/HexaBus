using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class UIRankElement : MonoBehaviour
{
    [SerializeField] Text _indexRankTxt;
    [SerializeField] Text _nameTxt;
    [SerializeField] Text _scoreTxt;
    [SerializeField] RawImage _countryFlag;
    [SerializeField] Image _bgRankuser;

    public void InitRank(int indexRank, int score, string name, int index)
    {
        if (index % 2 == 0)
        {
            SetColorTextContent(Color.black);
            _bgRankuser.color = GameController.Ins.RankController._colorWhite;
        }
        else
        {
            _bgRankuser.color = GameController.Ins.RankController._colorBlue;
            SetColorTextContent(Color.black);
        }
        if (index == 0)
        {
            _bgRankuser.color = GameController.Ins.RankController._colorYeallow;
            SetColorTextContent(GameController.Ins.RankController._colorOrange);
        }
        _indexRankTxt.text = $"{indexRank}.";
        _scoreTxt.text = $"{score}";
        _nameTxt.text = name;
    }
    private void SetColorTextContent(Color color)
    {
        _indexRankTxt.color = color;
        _nameTxt.color = color;
        _scoreTxt.color = color;
    }
}
