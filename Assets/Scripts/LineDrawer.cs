using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Core;
using Unity.VisualScripting;
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
    private List<bool> buttonsClicked = new List<bool>();
    [SerializeField] private List<VirtualButtonBehaviour> lsVbType = new List<VirtualButtonBehaviour>();
    [SerializeField] private List<GameObject> goChar = new List<GameObject>();
    private int TypeA = -1;
    private int TypeB = -1;
    private int ScoreA = 0;
    private int ScoreB = 0;
    private int Turn = 1;
    private int Clear1 = 0;
    private int Clear2 = 0;
    void Start()
    {
        for (int i = 0; i < virtualButtons.Count; i++)
        {
            virtualButtons[i].RegisterOnButtonPressed(OnButtonPressed);
            buttonsClicked.Add(false);
        }

        uiGameplay.txtScoreA.text = "SCORE A: " + ScoreA;
        uiGameplay.txtScoreB.text = "SCORE B: " + ScoreB;
        uiGameplay.txtTurn.text = "TURN " + Turn;
        linePositions.Clear();
    }

    public void RestartGame()
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

        for (int i = 0; i < buttonsClicked.Count; i++)
        {
            buttonsClicked[i] = false;
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
        if (buttonClickCount == 0)
        {
            if (!buttonsClicked[buttonIndex])
            {
                lastClickedIndex = buttonIndex;
                Debug.Log(buttonIndex);
                buttonClickCount++;
                Vector3 buttonPos = buttonPositions[buttonIndex].position;
                //linePositions.Add(buttonPos);
                TypeA = CheckType(buttonIndex);
                buttonsClicked[buttonIndex] = true;
            }
        }

        if (buttonIndex > 0 && buttonClickCount < virtualButtons.Count)
        {
            if (lastClickedIndex != buttonIndex)
            {
                if (!buttonsClicked[buttonIndex])
                {
                    buttonClickCount++;
                    TypeB = CheckType(buttonIndex);
                    buttonsClicked[buttonIndex] = true;
                }
            }

            if (buttonClickCount == 2)
            {
                Clear1 = buttonIndex;
                Clear2 = lastClickedIndex;
                Debug.Log("Index Last"+lastClickedIndex+"Index This"+buttonIndex);
                //Draw line
                Vector3 buttonPos = buttonPositions[buttonIndex].position;
                //linePositions.Add(buttonPos);
                lineRenderer.positionCount = buttonClickCount;
                lineRenderer.SetPositions(linePositions.ToArray());

                Debug.Log("All buttons clicked");
                
                Vector3 pos1 = goChar[buttonIndex].transform.position;
                Vector3 pos2 = goChar[lastClickedIndex].transform.position;
                
                Vector3 dir1to2 = (pos2 - pos1).normalized;
                
                
                Vector3 dir2to1 = (pos1 - pos2).normalized;
                
                goChar[buttonIndex].transform.rotation = Quaternion.LookRotation(dir1to2);
                goChar[lastClickedIndex].transform.rotation = Quaternion.LookRotation(dir2to1);
                
                float distance = Vector3.Distance(pos1, pos2);
                Debug.Log("Distanced test "+ distance);
                if (TypeB == TypeA)
                {
                    lsAnimators[buttonIndex].SetTrigger("Pressed");
                    lsAnimators[lastClickedIndex].SetTrigger("Pressed");
                    DestroyImageTarget(lastClickedIndex, buttonIndex);
                }

                Debug.Log("Type a: " + TypeA + " Type b: " + TypeB);
                if (TypeA == 0 && TypeB != 0)
                {
                    if (TypeB == 3)
                    {
                        //A win
                        lsAnimators[lastClickedIndex].SetTrigger("Pressed");
                        DestroyImageTarget(buttonIndex);
                        ScoreA++;
                        uiGameplay.txtScoreA.text = "SCORE A: " + ScoreA;
                    }
                    else
                    {
                        //A lose
                        lsAnimators[buttonIndex].SetTrigger("Pressed");
                        DestroyImageTarget(lastClickedIndex);
                        ScoreB++;
                        uiGameplay.txtScoreB.text = "SCORE B: " + ScoreB;
                    }
                }

                if (TypeA == 1 && TypeB != 1)
                {
                    if (TypeB == 0)
                    {
                        lsAnimators[lastClickedIndex].SetTrigger("Pressed");
                        DestroyImageTarget(buttonIndex);
                        ScoreA++;
                        uiGameplay.txtScoreA.text = "SCORE A: " + ScoreA;
                    }
                    else
                    {
                        lsAnimators[buttonIndex].SetTrigger("Pressed");
                    
                        DestroyImageTarget(lastClickedIndex);
                        ScoreB++;
                        uiGameplay.txtScoreB.text = "SCORE B: " + ScoreB;
                    }
                }

                if (TypeA == 2 && TypeB != 2)
                {
                    if (TypeB == 1)
                    {
                        lsAnimators[lastClickedIndex].SetTrigger("Pressed");
                        
                        DestroyImageTarget(buttonIndex);
                        ScoreA++;
                        uiGameplay.txtScoreA.text = "SCORE A: " + ScoreA;
                    }
                    else
                    {
                        
                        lsAnimators[buttonIndex].SetTrigger("Pressed");
                        
                        DestroyImageTarget(lastClickedIndex);
                        ScoreB++;
                        uiGameplay.txtScoreB.text = "SCORE B: " + ScoreB;
                    }
                }

                if (TypeA == 3 && TypeB != 3)
                {
                    if (TypeB == 2)
                    {
                        lsAnimators[lastClickedIndex].SetTrigger("Pressed");
                        DestroyImageTarget(buttonIndex);
                        ScoreA++;
                        uiGameplay.txtScoreA.text = "SCORE A: " + ScoreA;
                    }
                    else
                    {
                        lsAnimators[buttonIndex].SetTrigger("Pressed");
                        DestroyImageTarget(lastClickedIndex);
                        ScoreB++;
                        uiGameplay.txtScoreB.text = "SCORE B: " + ScoreB;
                    }
                }

               
                buttonClickCount = 0;
            }

            if (Turn > 4)
            {
                StartCoroutine(CheckWinLoseDelay());
            }
        }
    }

    public void changeTurn()
    {
        Turn++;
        uiGameplay.txtTurn.text = "TURN " + Turn;
    }

    IEnumerator CheckWinLoseDelay()
    {
        yield return new WaitForSecondsRealtime(0f);

        if (ScoreA > ScoreB)
        {
            //A win

            UIManager.Instance.HideUI(UIIndex.UIGameplay);
            UIManager.Instance.ShowUI(UIIndex.UIWinLose, new UIWinLoseParam()
            {
                win = 1
            });
        }
        else
        {
            UIManager.Instance.HideUI(UIIndex.UIGameplay);
            UIManager.Instance.ShowUI(UIIndex.UIWinLose, new UIWinLoseParam()
            {
                win = 2
            });
            //B win
        }

        RestartGame();
    }

    public int CheckType(int index)
    {
        for (int i = 0; i < lsVbType.Count; i++)
        {
            if (lsVbType[i] == virtualButtons[index])
            {
                if (i < 3)
                {
                    Debug.Log(virtualButtons[index].name + " type 1");
                    return 0;
                }
                else if (i < 6)
                {
                    Debug.Log(virtualButtons[index].name + " type 2");
                    return 1;
                }
                else if (i < 9)
                {
                    Debug.Log(virtualButtons[index].name + " type 3");
                    return 2;
                }
                else
                {
                    Debug.Log(virtualButtons[index].name + " type 4");
                    return 3;
                }
            }
        }

        return -1;
    }

    public void ClearFromList(int a, int b)
    {
        Debug.Log("Index Last"+lastClickedIndex+"Index This"+buttonIndex);
        lsImageTarget[a].gameObject.SetActive(false);
        lsImageTarget[b].gameObject.SetActive(false);
        Clear1 = -1;
        Clear2 = -1;
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

    void DestroyImageTarget(int indexa, int indexb)
    {
        if (indexa >= 0 && indexa < lsImageTarget.Count)
        {
            StartCoroutine(DelayedDestroy(indexa, indexb));
        }
    }

    IEnumerator DelayedDestroy(int indexa, int indexb)
    {
        buttonClickCount = 0;
        lsAnimators[indexa].SetTrigger("Hit");
        lsAnimators[indexb].SetTrigger("Hit");
        yield return new WaitForSecondsRealtime(5f); // Wait for 1 second
        
        linePositions.Clear();
        ClearFromList(indexa,indexb);
        Debug.Log("Died");
        changeTurn();
    }

    IEnumerator DelayedDestroy(int index)
    {
        yield return new WaitForSecondsRealtime(.5f);
        buttonClickCount = 0;
        lsAnimators[index].SetTrigger("Hit");
        yield return new WaitForSecondsRealtime(5f); // Wait for 1 second

        linePositions.Clear();
        ClearFromList(Clear1,Clear2);
        Debug.Log("Died");
        changeTurn();
    }
}