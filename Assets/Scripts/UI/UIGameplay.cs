using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIGameplay : BaseUI
{
    public TextMeshProUGUI txtTurn;
    public TextMeshProUGUI txtTimer;
    public TextMeshProUGUI txtScoreA;
    public TextMeshProUGUI txtScoreB;
    public GameObject goGamerule;
    public Button btnCloseRule;
    public Button btnPause;
    private bool isClosedRule = false;
    private float timerDuration = 20f;

    public override void OnSetUp(UIParam param = null)
    {
        base.OnSetUp(param);
        isClosedRule = false;
        goGamerule.SetActive(true);
    }

    public void OnCloseRule_Clicked()
    {
        goGamerule.SetActive(false);
        isClosedRule = true;
    }

    public void OnPause_Clicked()
    {
        UIManager.Instance.ShowUI(UIIndex.UIPause);
    }
    
}
