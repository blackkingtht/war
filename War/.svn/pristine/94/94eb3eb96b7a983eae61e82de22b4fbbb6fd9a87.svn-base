using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetNoticeView : UIViewBase
{

    protected override void OnBtnClick(GameObject go)
    {
        switch (go.name)
        {
            case "ConfirmNet":
                UIDispacher.Instance.DispachEvent("ClickConfirmNet", go);
                break;
        }
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
    }
}
