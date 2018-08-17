using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameView : UIViewBase
{
    public GameObject exit;
    protected override void OnBtnClick(GameObject go)
    {
        switch (go.name)
        {
            case "Exit":
                UIDispacher.Instance.DispachEvent("ClickBtnExit", this.gameObject);
                break;
            case "Cancel":
                UIDispacher.Instance.DispachEvent("ClickBtnCancel", this.gameObject);
                break;
        }
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
    }
}
