using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Elements : MonoBehaviour
{
    public float time;
    public GameObject PatternButton;
    public Sprite[] allSprites;
    private List<int> spriteList = new List<int>();
    private List<List<Transform>> columnsList = new List<List<Transform>>();
    private List<List<int>> patternsList;
    private int[] numberOfElements = {1, 2, 3, 4, 5, 6, 7, 8, 9};

    void Start()
    {
        List<int> pn1 = new List<int>{1, 1, 1};
        List<int> pn2 = new List<int>{2, 2, 2};
        List<int> pn3 = new List<int>{3, 3, 3};
        List<int> pn4 = new List<int>{4, 4, 4};
        List<int> pn5 = new List<int>{5, 5, 5};
        patternsList = new List<List<int>>{pn1, pn2, pn3, pn4, pn5};
    }

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
                if (firstIndex >= patternsList.Count)
                {
                    return;
                }
                int secondIndex = 0;
                foreach (Transform image in column)
                {   
                    if (secondIndex >= patternsList[firstIndex].Count)
                    {
                        secondIndex = 0;
                    }

                    image.GetComponent<Image>().sprite = allSprites[patternsList[firstIndex][secondIndex]];
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
    }
    public void EnableOff()
    {
        Invoke ("Pattern", time);
        enabled = false;
    }
}
