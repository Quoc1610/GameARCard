using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UI;


public class UIMainMenu : BaseUI
{
    public Button btnPlay;
    public Button btnDeck;

    public void OnPlay_Clicked()
    {
        UIManager.Instance.HideUI(this);
        UIManager.Instance.ShowUI(UIIndex.UIGameplay);
        
    } 
    public void OnDeck_Clicked()
    {
        UIManager.Instance.ShowUI(UIIndex.UIDeck);
        UIManager.Instance.HideUI(this);
       
        
    } 
}
