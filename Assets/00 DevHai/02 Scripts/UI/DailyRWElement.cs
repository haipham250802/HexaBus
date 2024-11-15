using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRWElement : MonoBehaviour
{
    [SerializeField] TypeReward _typeReward;
    [SerializeField] Button _claimButton;
    [SerializeField] Image _iconButtonClaim;
    [SerializeField] Image _iconReward;
    [SerializeField] Text _quantityReward;
    [SerializeField] Text dayTxt;
    [SerializeField] Sprite _iconCanClaim;
    [SerializeField] Sprite _iconCantClaim;
    [SerializeField] GameObject _gTickDone;
    [SerializeField] int _day;
    private void Start()
    {
        _claimButton.onClick.AddListener(ClaimClicked);
        CheckView();
    }
    private void CheckView()
    {
        int lastDayClaimed = GameController.Ins.DailyLoginController.LastDayClaimed;
        System.DateTime lastDate = new System.DateTime(GameController.Ins.DailyLoginController.LastTimeClaim);
        System.DateTime currentDate = System.DateTime.Now;
        if (_day <= lastDayClaimed)
        {
            OnStateClaimed();
            return;
        }
        if (lastDayClaimed + 1 == _day && lastDate.Date < currentDate.Date)
            OnStateClaiming();
        else
            OnStateClaimPending();
    }
    public void InitReward(Sprite iconRW, int quantityRW, int day , TypeReward typerw)
    {
        _iconReward.sprite = iconRW;
        _iconReward.SetNativeSize();
        _iconReward.transform.localScale = Vector3.one * 0.8f;
        _quantityReward.text = $"{quantityRW}";
        _day = day;
        dayTxt.text = _day.ToString();
        _typeReward = typerw;
    }
    private void ClaimClicked()
    {
        GameController.Ins.DailyLoginController.LastDayClaimed = _day;
        GameController.Ins.DailyLoginController.LastTimeClaim = System.DateTime.Now.Ticks;
        GameController.Ins.DailyLoginController.Claimed(_day);
        OnStateClaimed();
        DailyLoginController.A_OnCheckNoti?.Invoke();


        RewardInfo rw = new();
        rw.TypeReward = _typeReward;
        List<RewardInfo> listRWInf = new();
        listRWInf.Add(rw);
        UIPopUp.Ins.ShowPopUpReward(listRWInf);
    }
    private void OnStateClaimed()
    {
        _claimButton.gameObject.SetActive(false);
        _gTickDone.SetActive(true);
    }
    private void OnStateClaiming()
    {
        _iconButtonClaim.sprite = _iconCanClaim;
        dayTxt.color = Color.yellow;
    }
    private void OnStateClaimPending()
    {
        _iconButtonClaim.sprite = _iconCantClaim;
        dayTxt.color = Color.white;
    }
}
