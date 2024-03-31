using System.Collections;
using System.Collections.Generic;
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
    private List<Vector3> linePositions = new List<Vector3>();

    void Start()
    {
        for (int i = 0; i <virtualButtons.Count; i++)
        {
            virtualButtons[i].RegisterOnButtonPressed(OnButtonPressed);
        }
        linePositions.Clear();
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log("Button Pressed: " + vb.VirtualButtonName);

        // Get the index of the pressed button
        int buttonIndex = virtualButtons.IndexOf(vb);
        if (buttonIndex < lastClickedIndex)
        {
            Debug.LogWarning("Invalid button click order. Resetting drawing.");
            linePositions.Clear();
            DestroyImageTarget(lastClickedIndex);
            lastClickedIndex = buttonIndex;
            return;
        }

        lastClickedIndex = buttonIndex;


        if (buttonIndex >= 0 && buttonClickCount < 2)
        {

            buttonClickCount++;


            Vector3 buttonPos = buttonPositions[buttonIndex].position;


            linePositions.Add(buttonPos);


            lineRenderer.positionCount = buttonClickCount;
            lsAnimators[buttonIndex].SetTrigger("Pressed");
            lsAnimators[lastClickedIndex].SetTrigger("Pressed");
            lineRenderer.SetPositions(linePositions.ToArray());



            if (buttonClickCount == 2)
            {
                Debug.Log("All buttons clicked");

                buttonClickCount = 0;
                
                foreach (var vbtn in virtualButtons)
                {
                    vbtn.gameObject.SetActive(true);
                }
            }

        }
        virtualButtons[buttonIndex].gameObject.SetActive(false);
    }

    void DestroyImageTarget(int index)
    {
        if (index >= 0 && index < lsImageTarget.Count)
        {
            
            StartCoroutine(DelayedDestroy(index));
        }
    }

    IEnumerator DelayedDestroy(int index)
    {
        lsAnimators[index].SetTrigger("Hit");
        yield return new WaitForSeconds(10f); // Wait for 1 second
        lsImageTarget[index].gameObject.SetActive(false);
        linePositions.Clear();
        Debug.Log("Died");
        //lsImageTarget.RemoveAt(index);
    }
}
