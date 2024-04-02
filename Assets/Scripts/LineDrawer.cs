using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Core;
using UnityEngine;
using Vuforia;

public class LineDrawer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public List<Transform> buttonPositions = new List<Transform>();
    public List<VirtualButtonBehaviour> virtualButtons = new List<VirtualButtonBehaviour>();
    [SerializeField] private List<GameObject> lsImageTarget = new List<GameObject>();
    public List<Animator> lsAnimators = new List<Animator>();
    private int lastClickedIndex = -1;
    private int buttonClickCount = 0;
    private int buttonIndex = -1;
    public UIGameplay uiGameplay;
    private List<Vector3> linePositions = new List<Vector3>();
    
    [SerializeField] private List<VirtualButtonBehaviour> lsVbType = new List<VirtualButtonBehaviour>();
    private int TypeA = -1;
    private int TypeB = -1;
    private int ScoreA = 0;
    private int ScoreB = 0;
    private int Turn = 1;
    void Start()
    {
        for (int i = 0; i <virtualButtons.Count; i++)
        {
            virtualButtons[i].RegisterOnButtonPressed(OnButtonPressed);
        }
        uiGameplay.txtScoreA.text = "SCORE A: " + ScoreA;
        uiGameplay.txtScoreB.text = "SCORE B: " + ScoreB;
        uiGameplay.txtTurn.text = "TURN " + Turn;
        linePositions.Clear();
        
    }

    void RestartGame()
    {
        ScoreA = 0;
        ScoreB = 0;
        uiGameplay.txtScoreA.text = "SCORE A: " + ScoreA;
        uiGameplay.txtScoreB.text = "SCORE B: " + ScoreB;

       
        Turn = 1;
        uiGameplay.txtTurn.text = "TURN " + Turn;

       
        foreach (var imageTarget in lsImageTarget)
        {
            imageTarget.SetActive(true);
        }

       
        foreach (var vb in virtualButtons)
        {
            vb.enabled = true;
        }
    }

   

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log("Button Pressed: " + vb.VirtualButtonName);

        // Get the index of the pressed button
       buttonIndex = virtualButtons.IndexOf(vb);
        if (buttonClickCount==0)
        {
            lastClickedIndex = buttonIndex;
            Debug.Log(buttonIndex);
            buttonClickCount++;
            Vector3 buttonPos = buttonPositions[buttonIndex].position;
            linePositions.Add(buttonPos);
            TypeA = CheckType(buttonIndex);
        }
        
        if (buttonIndex > 0 && buttonClickCount <virtualButtons.Count)
        {

            if (lastClickedIndex != buttonIndex)
            {
                buttonClickCount++;
                TypeB = CheckType(buttonIndex);
            }

            if (buttonClickCount == 2)
            {
               
                //Draw line
                Vector3 buttonPos = buttonPositions[buttonIndex].position;
                linePositions.Add(buttonPos);
                lineRenderer.positionCount = buttonClickCount;
                lineRenderer.SetPositions(linePositions.ToArray());
                
                
                //Set anim
                lsAnimators[buttonIndex].SetTrigger("Pressed");
                lsAnimators[lastClickedIndex].SetTrigger("Pressed");
                
                //set win lose +score
              
                Debug.Log("All buttons clicked");
                
                

                if (TypeB == TypeA)
                {
                    DestroyImageTarget(lastClickedIndex,buttonIndex);
                }

                if (TypeA == 0 && TypeB != 0)
                {
                    if (TypeB == 3)
                    {
                        //A win
                        DestroyImageTarget(buttonIndex);
                        ScoreA++;
                        uiGameplay.txtScoreA.text = "SCORE A: " + ScoreA;
                        
                    }
                    else
                    {
                        //A lose
                        DestroyImageTarget(lastClickedIndex);
                        ScoreB++;
                        uiGameplay.txtScoreB.text = "SCORE B: " + ScoreB;
                    }
                }

                if (TypeA == 1 && TypeB != 1)
                {
                    if (TypeB == 0)
                    {
                        DestroyImageTarget(buttonIndex);
                        ScoreA++;
                        uiGameplay.txtScoreA.text = "SCORE A: " + ScoreA;
                    }
                    else
                    {
                        DestroyImageTarget(lastClickedIndex);
                        ScoreB++;
                        uiGameplay.txtScoreB.text = "SCORE B: " + ScoreB;
                    }

                    
                }
                
                if (TypeA == 2 && TypeB != 3)
                {
                    if (TypeB == 1)
                    {
                        DestroyImageTarget(buttonIndex);
                        ScoreA++;
                        uiGameplay.txtScoreA.text = "SCORE A: " + ScoreA;
                    }
                    else
                    {
                        DestroyImageTarget(lastClickedIndex);
                        ScoreB++;
                        uiGameplay.txtScoreB.text = "SCORE B: " + ScoreB;
                    }
                }
                
                if (TypeA == 3 && TypeB != 3)
                {
                    if (TypeB == 2)
                    {
                        DestroyImageTarget(buttonIndex);
                        ScoreA++;
                        uiGameplay.txtScoreA.text = "SCORE A: " + ScoreA;
                    }
                    else
                    {
                        DestroyImageTarget(lastClickedIndex);
                        ScoreB++;
                        uiGameplay.txtScoreB.text = "SCORE B: " + ScoreB;
                    }
                }

                changeTurn();
                buttonClickCount = 0;
            }

            if (Turn == 4)
            {
                if (ScoreA > ScoreB)
                {
                    //A win
                   
                    UIManager.Instance.HideUI(UIIndex.UIGameplay);
                    UIManager.Instance.ShowUI(UIIndex.UIWinLose,new UIWinLoseParam()
                    {
                        win=1
                    });
                    
                }
                else
                {
                    
                    UIManager.Instance.HideUI(UIIndex.UIGameplay);
                    UIManager.Instance.ShowUI(UIIndex.UIWinLose,new UIWinLoseParam()
                    {
                        win=2
                    });
                    //B win
                }
                RestartGame();
            }
        }
    }

    public void changeTurn()
    {
        Turn++;
        uiGameplay.txtTurn.text = "TURN " + Turn;
    }

    public int CheckType(int index)
    {
        for (int i = 0; i < lsVbType.Count; i++)
        {
            if (lsVbType[i] == virtualButtons[index])
            {
                if (i < 3)
                {
                    Debug.Log(virtualButtons[index].name+" type 1");
                    return 0;
                }
                else if (i < 6)
                {
                    Debug.Log(virtualButtons[index].name+" type 2");
                    return 1;
                }
                else if (i < 9)
                {
                    Debug.Log(virtualButtons[index].name+" type 3");
                    return 2;
                }
                else
                {
                    Debug.Log(virtualButtons[index].name+" type 4");
                    return 3;
                }
            }
        }

        return -1;
    }

    public void ClearFromList()
    {
        lsImageTarget[buttonIndex].gameObject.SetActive(false);
        lsImageTarget[lastClickedIndex].gameObject.SetActive(false);
    
        // Remove the buttons from the list
        virtualButtons[buttonIndex].enabled = false;
        virtualButtons[lastClickedIndex].enabled = false;
        
        // Clear the line positions
        linePositions.Clear();
    }

    void DestroyImageTarget(int index)
    {
        if (index >= 0 && index < lsImageTarget.Count)
        {
            
            StartCoroutine(DelayedDestroy(index));
           
        }
    }
    void DestroyImageTarget(int indexa,int indexb)
    {
        if (indexa >= 0 && indexa < lsImageTarget.Count)
        {
            
            StartCoroutine(DelayedDestroy(indexa,indexb));
        }
    }
    IEnumerator DelayedDestroy(int indexa,int indexb)
    {
        buttonClickCount = 0;
        lsAnimators[indexa].SetTrigger("Hit");
        lsAnimators[indexb].SetTrigger("Hit");
        yield return new WaitForSeconds(5f); // Wait for 1 second
       
        linePositions.Clear();
        ClearFromList();
        Debug.Log("Died");
    }
    IEnumerator DelayedDestroy(int index)
    {
        buttonClickCount = 0;
        lsAnimators[index].SetTrigger("Hit");
        yield return new WaitForSeconds(5f); // Wait for 1 second
       
        linePositions.Clear();
        ClearFromList();
        Debug.Log("Died");
        
        
       
    }
}
