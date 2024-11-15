using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum TypeLoading
{
    NONE,
    FIRST_GAME,
    IN_GAME
}
public class UILoading : MonoBehaviour
{
    [SerializeField] Text _dotTxt;
    [SerializeField] float _durationFristGame;
    [SerializeField] float _durationInGame;
    [SerializeField] float _durationOnGame;
    public int _indexScene;
    float _duration;
    float _timeCooldown = 0.25f;
    bool _isLoadedGame = false;
    public TypeLoading TypeLoading;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (_duration > 0)
        {
            _duration -= Time.deltaTime;
            if (_durationOnGame > 0) _durationOnGame -= Time.deltaTime;
            else
            {
                if (_isLoadedGame) return;
                _isLoadedGame = true;
                SceneManager.LoadSceneAsync(_indexScene, LoadSceneMode.Single);
            }
            if (_timeCooldown > 0) _timeCooldown -= Time.deltaTime;
            else
            {
                _timeCooldown = 0.25f;
                if (_dotTxt.text.Length >= 3)
                    _dotTxt.text = "";
                else
                    _dotTxt.text += ".";
            }
        }
        else
            DestroyImmediate(gameObject);
    }
    public void Load(TypeLoading typeLoading, int indexScene)
    {
        switch (typeLoading)
        {
            case TypeLoading.NONE:
                break;
            case TypeLoading.FIRST_GAME:
                _duration = _durationFristGame;
                break;
            case TypeLoading.IN_GAME:
                _duration = _durationInGame;
                break;
            default:
                break;
        }
        _indexScene = indexScene;
        _durationOnGame = _duration * 0.8f;
    }
}
