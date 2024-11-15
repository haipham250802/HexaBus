using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRankUserElement : MonoBehaviour
{
    [SerializeField] Text _indexRankText;
    [SerializeField] Text _scoreText;
    [SerializeField] Text _nameUserText;

    public void InitRankUser(int indexRank , int score ,  string name)
    {
        _indexRankText.text = indexRank.ToString();
        _scoreText.text = score.ToString();
        _nameUserText.text = name;
        if (indexRank > 50)
            _indexRankText.text = "...";
    }
}
