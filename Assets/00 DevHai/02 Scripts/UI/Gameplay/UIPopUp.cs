using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIPopUp : MonoBehaviour
{
    public static UIPopUp Ins;

    private void Awake()
    {
        if (Ins == null)
            Ins = this;
        else
            DestroyImmediate(gameObject);
    }
    private PopUpBase Load(string path)
    {
        PopUpBase popup = Instantiate(Resources.Load<PopUpBase>(path));

        popup.transform.SetParent(transform);
        popup.transform.localScale = Vector3.one;
        popup.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        popup.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        popup.transform.SetAsLastSibling();

        return popup;
    }
    public void ShowPopUpWin()
    {
        Load("PopUpWin");
    }
    public void ShowPopUpLose()
    {
        Load("PopUpLose");
    }
    public void ShowPopUpSetting()
    {
        Load("PopUpSetting");
    }
    public void ShowPopUpPiggyBank()
    {
        Load("PopUpPiggyBank");
    }
    public void ShowPopUpNoAds()
    {
        Load("PopUpNoads");
    }
    public void ShowPopUpLuckyWheel()
    {
        Load("PopUpLuckyWheel");
    }
    public void ShowPopUpReward(List<RewardInfo> listRWInfo)
    {
        PopUpReward popup = Load("PopUpReward") as PopUpReward;
        popup.ShowReward(listRWInfo);
    }
    public void ShowPopUpShop()
    {
        Load("PopUpShop");
    }
    public void ShowPopUpDailyLogin()
    {
        Load("PopUpDailyLogin");
    }
    public void ShowPopUpRank()
    {
        Load("PopUpRank");
    }
    public void ShowPopUpUnlockHex(System.Action callback = null, TypePopUpUnlock typePopUpUnlock = TypePopUpUnlock.NONE)
    {
        PopUpUnlockHex poppup = Load("PopUpUnlockHexa") as PopUpUnlockHex;
        poppup.ActionButton = callback;
        poppup.SetIcon(poppup.DataPopUpUnlock.GetIconPopUpUnlock(typePopUpUnlock));
    }
    public void ShowPopUpDropPiggyBank()
    {
        Load("PopUpDropCoinPiggyBank");
    }
}
