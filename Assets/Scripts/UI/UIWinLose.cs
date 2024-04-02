using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIWinLoseParam : UIParam
{
    public int win;
}
public class UIWinLose : BaseUI
{
    public UIWinLoseParam uiWinLoseParam;
    public TextMeshProUGUI txtStatus;
    public Button btnNext;
    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);

        uiWinLoseParam = (UIWinLoseParam)param;
        Debug.Log("UI Param"+uiWinLoseParam.win);
        if (uiWinLoseParam.win == 1)
        {
            txtStatus.text = "Player A Win";
        }
        if (uiWinLoseParam.win == 2)
        {
            txtStatus.text = "Player B Win";
        }
    }

    public void OnNextClick()
    {
        UIManager.Instance.HideUI(UIIndex.UIWinLose);
        UIManager.Instance.ShowUI(UIIndex.UIMain);
    }
}
