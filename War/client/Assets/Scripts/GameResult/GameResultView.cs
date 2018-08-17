using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResultView : UIViewBase
{
    public Text resultText;

    public Text soldire_me;
    public Text building_me;
    public Text energy_me;
    public Text goldcoin_me;

    public Text soldire_friend;
    public Text building_friend;
    public Text energy_friend;
    public Text goldcoin_friend;


    protected override void OnBtnClick(GameObject go)
    {
        switch (go.name)
        {
            case "Confirm":
                UIDispacher.Instance.DispachEvent("ClickConfirm", this.gameObject);
                break;
        }
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
    }
}
