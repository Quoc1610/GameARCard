using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIPause : BaseUI
{
    public Button btnResume;
    public Button btnHome;
    public LineDrawer LineDrawer;
    public void OnResume_Clicked()
    {
        UIManager.Instance.HideUI(UIIndex.UIPause);
    }

    public void OnHome_Clicked()
    {
        UIManager.Instance.HideUI(UIIndex.UIPause);
        UIManager.Instance.HideUI(UIIndex.UIGameplay);
        LineDrawer.RestartGame();
        UIManager.Instance.ShowUI(UIIndex.UIMain);
    }
}
