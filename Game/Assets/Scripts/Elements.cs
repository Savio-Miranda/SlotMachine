using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Elements : MonoBehaviour
{
    public float time;
    public GameObject PatternButton;
    public Sprite[] allSprites;
    private List<int> spriteList = new List<int>();
    private List<List<Transform>> columnsList = new List<List<Transform>>();
    private int[] numberOfElements = {1, 2, 3, 4, 5, 6, 7, 8, 9};
    public Web request;
    
    void Update()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Image>().sprite = allSprites[Random.Range (0, numberOfElements.Length)];
        }
    }
    
    public void Pattern()
    {
        bool isPatternActive = PatternButton.GetComponent<Button>().IsActive();
        
        if (isPatternActive)
        {        
            int counter = 0;
            // Receiving columns
            for (int i = 0; i < 5; i++)
            {
                columnsList.Add(ColumnBuilder(transform, counter));
                counter += 3;
            }
            int firstIndex = 0;
            foreach (List<Transform> column in columnsList)
            {
                if (firstIndex >= Web.result.Count)
                {
                    return;
                }
                int secondIndex = 0;
                foreach (Transform image in column)
                {   
                    if (secondIndex >= Web.result[firstIndex].Count)
                    {
                        secondIndex = 0;
                    }

                    image.GetComponent<Image>().sprite = allSprites[Web.result[firstIndex][secondIndex]];
                    secondIndex++;
                }
                firstIndex++;
            }
        }
    }

    // Create the columns
    private List<Transform> ColumnBuilder(Transform element, int step)
    {
        List<Transform> column = new List<Transform>();
        int childCount = element.childCount;
        
        for (int i = step; i < childCount; i++)
        {
            column.Add(element.GetChild(i));
        }
        return column;
    }

    public void EnableOn()
    {
        enabled = true;
        spriteList.Clear();
        request.GetElements();
    }
    public void EnableOff()
    {
        Invoke ("Pattern", time);
        enabled = false;
    }
}
