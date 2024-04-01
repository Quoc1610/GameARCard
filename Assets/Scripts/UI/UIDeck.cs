using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Core;
using Unity.VisualScripting;
using UnityEditor;


public class UIDeck : BaseUI
{
    public Button btnNext;
    public Button btnClose;
    public List<GameObject> lsimgDeck = new List<GameObject>();
    private int index = 0;
    public void Start()
    {
        index = 0;
        lsimgDeck[index].SetActive(true);
    }

    public override void OnCloseClick()
    {
        base.OnCloseClick();
        UIManager.Instance.ShowUI(UIIndex.UIMain);
    }

    public void OnNext_Clicked()
    {
        Debug.Log("Index"+index);
        if (index < lsimgDeck.Count-1)
        {
            lsimgDeck[index].SetActive(false);
            index++;
            lsimgDeck[index].SetActive(true);
        }
        else
        {
            lsimgDeck[index].SetActive(false);
            index = 0;
            lsimgDeck[index].SetActive(true);
        }
    }
}
