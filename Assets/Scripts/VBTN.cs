using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
public class VBTN : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform pos1;
    
    public VirtualButtonBehaviour Vb;
    public Animator anim;
    private void Start()
    {
        
        Vb.RegisterOnButtonPressed(OnButtonPressed);
        Vb.RegisterOnButtonReleased(OnButtonReleased);
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log("Pressed Button");
        anim.SetTrigger("Pressed");
        
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        Debug.Log("Pressed Button");
        anim.ResetTrigger("Pressed");
    }
}
