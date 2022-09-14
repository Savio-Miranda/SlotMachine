using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Elements : MonoBehaviour
{
    public GameObject PatternButton;
    public Sprite[] newSprites;
    private List<int> spriteList = new List<int>();
    private List<List<Transform>> columnsList = new List<List<Transform>>();
    private int[] numberOfElements = {1, 2, 3, 4, 5, 6, 7, 8, 9};
    public float time;

    void Update()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Image>().sprite = newSprites[Random.Range (0, numberOfElements.Length)];
        }
    }
    
    public void Pattern()
    {
        bool isPatternActive = PatternButton.GetComponent<Button>().IsActive();
        
        if (isPatternActive)
        {
            int countChildren = transform.childCount;
        
            // Receiving columns
            for (int i = 0; i < 2; i++)
            {
                columnsList.Add(ColumnsMaker(countChildren, 3));
            }
            
            // Receiving the pattern of the columns (Futurely through API)
            foreach (List<Transform> column in columnsList)
            {
                foreach (Transform image in column)
                {
                    image.GetComponent<Image>().sprite = newSprites[5];
                }
            }
        }
        else
        {
            return;
        }
    }

    // Create the columns
    private List<Transform> ColumnsMaker(int elements, int step)
    {

        int counter = 0;
        List<Transform> column = new List<Transform>();
        
        for (int i = 0; i < elements; i++)
        {
            column.Add(transform.GetChild(i));
            counter++;
            if (counter >= step)
            {
                return column;
            }
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
